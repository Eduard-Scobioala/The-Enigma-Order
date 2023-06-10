using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    public GameData gameData { get; private set; }
    private List<IDataPersistance> DataPersistanceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    private Coroutine autoSaveCoroutine;
    public bool onExitButtonPressed = false;

    public static DataPersistanceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one Data Persistance instance found. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize the data handler object
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        // Update the profile to use for the saving
        this.selectedProfileId = newProfileId;

        // Load the GameData for the new profileId
        LoadGame();
    }

    #region Scene Management

    private void OnEnable()
    {
        // Subscribe functions
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe functions
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Initialize the list with all the persistent objects
        DataPersistanceObjects = FindAllDataPersistenceObjects();

        LoadGame();

        // Start up the auto saving coroutine
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    #endregion

    #region Save&Load

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadSave()
    {
        // Load any saved game from the file used by the data handler
        gameData = dataHandler.Load(selectedProfileId);
    }

    public void LoadGame()
    {
        // Return if data persistence is disabled
        if (disableDataPersistence) return;

        // For testing purpose we might need a new game when debugging diffrent scenes
        if (gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        // If no data can be loaded, don't continue
        if (gameData == null)
        {
            Debug.Log("No data found. A New Game needs to be started before data can be loaded");
            return;
        }

        // Push the loaded data to all the scripts that requires it
        foreach(IDataPersistance dataPersistanceObj in DataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // Return if data persistence is disabled
        if (disableDataPersistence) return;

        //If we don't have any data to save, log a warning
        if (gameData == null)
        {
            Debug.Log("No data found. A New Game needs to be started before data can be loaded");
        }

        // Pass the data to all the scripts so that it can be updated
        foreach (IDataPersistance dataPersistanceObj in DataPersistanceObjects)
        {
            // Avoid saving data for deleted objects
            if (dataPersistanceObj != null)
            {
                dataPersistanceObj.SaveData(gameData);
            }
        }

        // Save GameData to a file
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        // Save the game if it forced quited
        if (!onExitButtonPressed)
        {
            SaveGame();
        }
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        // Using using System.Linq get all the objects that implement MonoBehaviour and IDataPersistance
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public bool HasTheGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved Game");
        }
    }

    #endregion
}

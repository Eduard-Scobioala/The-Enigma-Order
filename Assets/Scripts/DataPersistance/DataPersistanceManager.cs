using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistance> DataPersistanceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "test";

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
    }

    #region Scene Management

    private void OnEnable()
    {
        // Subscribe functions
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        // Unsubscribe functions
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Initialize the list with all the persistent objects
        DataPersistanceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    #endregion

    #region Save&Load

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved game from the file used by the data handler
        gameData = dataHandler.Load(selectedProfileId);

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
        //If we don't have any data to save, log a warning
        if (gameData == null)
        {
            Debug.Log("No data found. A New Game needs to be started before data can be loaded");
        }

        // Pass the data to all the scripts so that it can be updated
        foreach (IDataPersistance dataPersistanceObj in DataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(gameData);
        }

        // Save GameData to a file
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        // Using using System.Linq get all the objects that implement MonoBehaviour and IDataPersistance
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>()
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

    #endregion
}

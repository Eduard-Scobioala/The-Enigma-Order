using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour, IDataPersistance
{
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one GameSceneManager instance found. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] ScreenTint screenTint;
    [SerializeField] CameraConfiner cameraConfiner;
    private string currentScene;

    // Variable for async operations control
    private List<AsyncOperation> asyncOperations = new();

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }


    public void InitSwitchScene(string newScene, Vector3 targetPosition)
    {
        StartCoroutine(Transition(newScene, targetPosition));
        //SwitchScene(newScene, targetPosition);
    }

    IEnumerator Transition(string to, Vector3 targetPosition)
    {
        screenTint.Tint();

        // Wait for the tint to finish
        yield return new WaitForSeconds(1f / screenTint.tintSpeed + 0.1f);

        SwitchScene(to, targetPosition);

        // Wait for the asyncOperations to be finished
        while (asyncOperations.Any(x => !x.isDone))
        {
            yield return new WaitForSeconds(0.1f);
        }
        asyncOperations.Clear();

        // Refrence the bounds for the Camera Confiner
        //cameraConfiner.SetBounds();

        screenTint.UnTint();
    }

    public void SwitchScene(string newScene, Vector3 targetPosition)
    {
        DataPersistanceManager.Instance.gameData.playerPosition = targetPosition;

        // If loading from MainMenu load the Essential Scene
        string oldScene = currentScene;
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu")
        {
            asyncOperations.Add(SceneManager.LoadSceneAsync("Essential"));
        }

        asyncOperations.Add(SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive));

        try
        {
            //asyncOperations.Add(SceneManager.UnloadSceneAsync(currentScene));
            SceneManager.UnloadSceneAsync(oldScene);
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        currentScene = newScene;


        // Place the main character in the desired position
        //Transform playerTransform = GameManager.instance.player.transform;

        //// Reposition the camera
        //CinemachineBrain currentCamera = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        //currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
        //    playerTransform,
        //   targetPosition - playerTransform.position
        //    );

        //playerTransform.position = targetPosition;
    }

    #region Save&Load

    public void LoadData(GameData data)
    {
        //this.currentScene = data.currentScene;
    }

    public void SaveData(GameData data)
    {
        data.currentSceneName = currentScene;
    }

    #endregion
}

using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] ScreenTint screenTint;
    [SerializeField] CameraConfiner cameraConfiner;
    string currentScene;

    // Variables for async operations control
    AsyncOperation load;
    AsyncOperation unload;

    void Start()
    {
        currentScene  = SceneManager.GetActiveScene().name;
    }

    public void InitSwitchScene(string to, Vector3 targetPosition)
    {
        StartCoroutine(Transition(to, targetPosition));
    }

    IEnumerator Transition(string to, Vector3 targetPosition)
    {
        screenTint.Tint();

        // Wait for the tint to finish
        yield return new WaitForSeconds(1f / screenTint.tintSpeed + 0.1f);

        SwitchScene(to, targetPosition);

        // Wait for the load and unload to be finished
        while (load != null & unload != null)
        {
            if (load.isDone) { load = null; }
            if (unload.isDone) { unload = null; }

            // Wait for the next check cicle
            yield return new WaitForSeconds(0.1f);
        }

        // Refrence the bounds for the Camera Confiner
        cameraConfiner.SetBounds();

        screenTint.UnTint();
    }

    public void SwitchScene(string to, Vector3 targetPosition)
    {
        load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        unload = SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;

        // Place the main character in the desired position
        Transform playerTransform = GameManager.instance.player.transform;

        // Reposition the camera
        CinemachineBrain currentCamera = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
            playerTransform,
           targetPosition - playerTransform.position
            );

        playerTransform.position = new Vector3(
            targetPosition.x,
            targetPosition.y,
            targetPosition.z);
    }
}

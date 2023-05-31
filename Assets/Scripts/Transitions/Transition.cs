using UnityEngine;

public enum TransitionType
{
    Warp,
    Scene
}

public class Transition : MonoBehaviour
{
    [SerializeField] TransitionType transitionType;
    [SerializeField] string sceneToTransition;
    [SerializeField] Vector3 targetPosition;

    Transform destination;

    void Start()
    {
        destination = transform.GetChild(1);
    }

    internal void InitiateTransition(Transform toTransition)
    {
        switch (transitionType)
        {
            case TransitionType.Warp:
                Cinemachine.CinemachineBrain currentCamera =
                    Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
                // Event for repositioning of the camera
                currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
                toTransition,
                destination.position - toTransition.position
                );

                // Change the position of the character
                toTransition.position = new Vector3(
                    destination.position.x,
                    destination.position.y,
                    toTransition.position.z
                );
                break;
            case TransitionType.Scene:
                // Load the next scene with the character on a target position
                GameSceneManager.instance.InitSwitchScene(sceneToTransition, targetPosition);
                break;
        }
    }
}

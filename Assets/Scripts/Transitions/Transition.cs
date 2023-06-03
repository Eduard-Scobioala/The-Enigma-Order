using UnityEditor;
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
    [SerializeField] Vector3 nextScenePosition;
    [SerializeField] Collider2D wrapConfiner;

    CameraConfiner cameraConfiner;
    [SerializeField] Transform destination;

    void Start()
    {
        if (wrapConfiner != null)
        {
            cameraConfiner = FindObjectOfType<CameraConfiner>();
        }
    }

    internal void InitiateTransition(Transform toTransition)
    {
        switch (transitionType)
        {
            case TransitionType.Warp:
                Cinemachine.CinemachineBrain currentCamera =
                    Camera.main.GetComponent<Cinemachine.CinemachineBrain>();

                // The camera confiner has changed
                if (cameraConfiner != null)
                {
                    cameraConfiner.UpdateBounds(wrapConfiner);
                }

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
                GameSceneManager.instance.InitSwitchScene(sceneToTransition, nextScenePosition);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (transitionType == TransitionType.Scene)
        {
            Handles.Label(transform.position, "to " + sceneToTransition);
        }

        if (transitionType == TransitionType.Warp)
        {
            Gizmos.DrawLine(transform.position, destination.position);
        }
    }
}

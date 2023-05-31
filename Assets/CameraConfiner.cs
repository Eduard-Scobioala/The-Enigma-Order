using Cinemachine;
using System;
using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    [SerializeField] CinemachineConfiner confiner;

    private void Start()
    {
        SetBounds();
    }

    public void SetBounds()
    {
        GameObject obj = GameObject.Find("CameraConfiner");

        // Verify for the Camera Confier to be defined
        if (obj == null)
        {
            confiner.m_BoundingShape2D = null;
            return;
        }

        // Refrence the bounds for Camera Confier
        Collider2D bounds = obj.GetComponent<Collider2D>();
        confiner.m_BoundingShape2D = bounds;
    }
}

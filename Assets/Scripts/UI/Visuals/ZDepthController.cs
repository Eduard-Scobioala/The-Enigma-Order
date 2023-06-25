using UnityEngine;

public class ZDepthController : MonoBehaviour
{
    Transform t;
    [SerializeField] bool stationary = true;

    private void Start()
    {
        t = transform;
    }

    private void LateUpdate()
    {
        // Set the z value in dependece of y value of the transform
        Vector3 position = transform.position;
        position.z = position.y * 0.0001f;
        transform.position = position;

        // If stationary there is no need to calculate z more that once
        if (stationary) { Destroy(this); }
    }
}

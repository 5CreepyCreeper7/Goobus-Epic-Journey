using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = layer stays fixed in world space (moves normally with the level, feels close). " +
             "1 = layer sticks to the camera (barely moves relative to it, feels distant/sky-like).")]
    [SerializeField, Range(0f, 1f)] private float parallaxFactor = 0.5f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;

        // Ignore tiny back-and-forth jitter, only respond to real, deliberate camera movement
        if (delta.sqrMagnitude < 0.0004f) { // tune this threshold — ~0.02 units squared
            lastCameraPosition = cameraTransform.position;
            return;
        }

        transform.position += delta * parallaxFactor;
        lastCameraPosition = cameraTransform.position;
    }
}
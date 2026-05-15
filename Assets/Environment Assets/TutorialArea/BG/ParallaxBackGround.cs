using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;

    public float horizontalParallax = 0.5f;
    public float verticalParallax = 0.1f;

    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(
            deltaMovement.x * horizontalParallax,
            deltaMovement.y * verticalParallax,
            0f
        );

        lastCameraPosition = cameraTransform.position;
    }
}
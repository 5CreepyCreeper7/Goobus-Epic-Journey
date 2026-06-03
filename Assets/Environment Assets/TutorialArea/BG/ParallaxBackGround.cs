using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;

    public float horizontalParallax = 0.5f;
    public float verticalParallax = 0.1f;

    private Vector3 startingLocalPosition;
    private Vector3 startingCameraPosition;

    private void Awake()
    {
        startingLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        if (cameraTransform != null)
        {
            startingCameraPosition = cameraTransform.position;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 deltaMovement = cameraTransform.position - startingCameraPosition;

        transform.localPosition = startingLocalPosition + new Vector3(
            deltaMovement.x * horizontalParallax,
            deltaMovement.y * verticalParallax,
            0f
        );
    }
}
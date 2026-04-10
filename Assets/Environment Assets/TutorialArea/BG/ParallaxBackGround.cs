using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public Transform cameraTransform;
    public float[] scrollSpeeds;

    private Vector3 lastCameraPosition;
    private Renderer[] renderers;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;

        renderers = new Renderer[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            renderers[i] = transform.GetChild(i).GetComponent<Renderer>();
        }
    }

    void LateUpdate()
    {
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;

        for (int i = 0; i < renderers.Length; i++)
        {
            float offset = deltaX * scrollSpeeds[i];

            Vector2 currentOffset = renderers[i].material.mainTextureOffset;
            currentOffset.x += offset;

            renderers[i].material.mainTextureOffset = currentOffset;
        }

        lastCameraPosition = cameraTransform.position;
    }
}

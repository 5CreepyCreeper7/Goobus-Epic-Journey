using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public CinemachineConfiner2D Confiner;
    public CinemachineCamera VirtualCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SnapToTarget(Transform target, Vector3 positionDelta)
    {
        if(VirtualCamera != null)
        {
            VirtualCamera.OnTargetObjectWarped(target, positionDelta);
        }
    }
}
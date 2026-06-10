using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraVariance : MonoBehaviour
{
    public static CameraVariance instance;

    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void ShakeCamera(Vector2 direction, float force) {
        impulseSource.GenerateImpulse(direction.normalized * force);
        
    }
}
//testing
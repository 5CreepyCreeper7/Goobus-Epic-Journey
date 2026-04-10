using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
public class CameraTransition : MonoBehaviour
{
    public CinemachineCamera TargetCamera;
    public CinemachineCamera MainCamera;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            if(TargetCamera.Priority < MainCamera.Priority) {
                TargetCamera.Priority = 20;
                MainCamera.Priority = 10;
            } else {
                TargetCamera.Priority = 10;
                MainCamera.Priority = 20;
            }
            
        }
    }
}

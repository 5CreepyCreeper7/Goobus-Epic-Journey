using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private int TargetSpawnPointID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player")) {
            return;
        }
        
        if(RoomManager.Instance == null) {
            Debug.LogError("No RoomManager Exists.");
            return;
        }

        RoomManager.Instance.TransitionToRoom(targetSceneName, TargetSpawnPointID, RoomTransitionType.Normal);
    }
}


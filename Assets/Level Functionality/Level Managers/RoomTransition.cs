using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private int TargetRoomID;
    [SerializeField] private int TargetSpawnPointID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) {
            RoomManager.Instance.TransitionToRoom(TargetRoomID, TargetSpawnPointID);
        }
    }
}


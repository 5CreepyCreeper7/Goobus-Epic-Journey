using UnityEngine;

public class PlaceCharacter : MonoBehaviour
{
    public void PlacePlayer()
    {
        RoomSpawnPoint spawnPoint = RoomManager.Instance.GetCurrentSpawnPoint();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("No current spawn point found.");
        }
    }
}

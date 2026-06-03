using UnityEngine;

public class PlaceCharacter : MonoBehaviour
{
    public void PlacePlayer()
    {
        Transform spawnPoint = RoomManager.Instance.GetCurrentSpawnPoint();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No current spawn point found.");
        }
    }
}

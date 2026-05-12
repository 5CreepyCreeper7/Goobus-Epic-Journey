using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField] public int RoomID;
    [SerializeField] private RoomSpawnPoint[] spawnPoints;

    public Transform GetSpawnPoint(int spawnPointID) {
       foreach(RoomSpawnPoint spawnPoint in spawnPoints) {
            if(spawnPoint.spawnID == spawnPointID) {
                return spawnPoint.transform;
            }
        }
        return null;
    }
}

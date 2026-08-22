using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField] private RoomSpawnPoint[] spawnPoints;
    [SerializeField] private Collider2D cameraBounds;

    public Collider2D CameraBounds => cameraBounds;
    
    public RoomSpawnPoint GetSpawnPoint(int spawnPointID) {
       foreach(RoomSpawnPoint spawnPoint in spawnPoints) {
            if(spawnPoint.spawnID == spawnPointID) {
                return spawnPoint;
            }
        }
        return null;
    }
}

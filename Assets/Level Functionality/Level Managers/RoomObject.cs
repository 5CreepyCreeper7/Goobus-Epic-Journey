using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField] public int RoomID;
    [SerializeField] private RoomSpawnPoint[] spawnPoints;
    [SerializeField] private Collider2D cameraBounds;

    public Collider2D CameraBounds => cameraBounds;
    
    public Transform GetSpawnPoint(int spawnPointID) {
       foreach(RoomSpawnPoint spawnPoint in spawnPoints) {
            if(spawnPoint.spawnID == spawnPointID) {
                return spawnPoint.transform;
            }
        }
        return null;
    }
}

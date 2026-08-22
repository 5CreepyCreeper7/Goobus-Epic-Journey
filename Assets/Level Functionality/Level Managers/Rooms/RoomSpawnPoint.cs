using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    [SerializeField] public int spawnID;
    [SerializeField] private RoomEntranceType entranceType;
    [SerializeField] private Transform entryStartPos;
    [SerializeField] private Transform entryEndPos;

    public int SpawnID => spawnID;
    public RoomEntranceType EntranceType => entranceType;
    public Vector3 EntryStart => entryStartPos.position;
    public Vector3 EntryEnd => entryEndPos.position;
}

using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private RoomObject[] rooms;
    [SerializeField] private TransitionAnimation transitionAnimation;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private int startingRoomID = 1;

    private int currentRoomID;
    private RoomObject currentRoom;
    private bool inTransition = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        foreach (RoomObject room in rooms) {
            room.gameObject.SetActive(false);
        }

        currentRoom = GetRoomByID(startingRoomID);

        if (currentRoom != null) {
            currentRoom.gameObject.SetActive(true);
        } else {
            Debug.LogError("Failed to initialize the starting room.");
        }
    }

    public void TransitionToRoom(int targetRoomID, int targetSpawnPointID) {
        if(inTransition) return;

        StartCoroutine(TransitionRoutine(targetRoomID, targetSpawnPointID));
    }

    private IEnumerator TransitionRoutine(int targetRoomID, int targetSpawnPointID)
{
        inTransition = true;

        RoomObject targetRoom = GetRoomByID(targetRoomID);

        if (targetRoom == null)
        {
            inTransition = false;
            yield break;
        }

        Transform spawnPoint = targetRoom.GetSpawnPoint(targetSpawnPointID);

        if (spawnPoint == null)
        {
            inTransition = false;
            yield break;
        }

        yield return transitionAnimation.FadeOut();

        DisableCurrentRoom();
        EnableNextRoom(targetRoom);

        playerPosition.position = spawnPoint.position;
        currentRoom = targetRoom;

        yield return new WaitForSeconds(0.5f);

        yield return transitionAnimation.FadeIn();

        inTransition = false;
    }

    private RoomObject GetRoomByID(int roomID) {
        foreach (RoomObject room in rooms) {
            if (room.RoomID == roomID) {
                return room;
            }
        }
        return null;
    }

    private void DisableCurrentRoom() {
        if (currentRoom != null) {
            currentRoom.gameObject.SetActive(false);
        }
    }

    private void EnableNextRoom(RoomObject nextRoom) {
        nextRoom.gameObject.SetActive(true);
        confiner.BoundingShape2D = nextRoom.CameraBounds;
        confiner.InvalidateBoundingShapeCache();
    }
}

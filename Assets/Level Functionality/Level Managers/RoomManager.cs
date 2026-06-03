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
    private int currentSpawnPointID;
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
            currentRoomID = startingRoomID;
            currentSpawnPointID = 1;

            if (confiner != null && currentRoom.CameraBounds != null){
                confiner.BoundingShape2D = currentRoom.CameraBounds;
                confiner.InvalidateBoundingShapeCache();
            }
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
        currentRoomID = targetRoomID;
        currentSpawnPointID = targetSpawnPointID;

        yield return null;

        confiner.BoundingShape2D = targetRoom.CameraBounds;
        confiner.InvalidateBoundingShapeCache();

        Physics2D.SyncTransforms();

        yield return new WaitForEndOfFrame();
        
        yield return transitionAnimation.FadeIn();

        inTransition = false;
        currentSpawnPointID = targetSpawnPointID;
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
    }

    public int GetCurrentSpawnPointID() {
        return  currentSpawnPointID;
    }

    public Transform GetCurrentSpawnPoint() {
        if (currentRoom == null)
            return null;

        return currentRoom.GetSpawnPoint(currentSpawnPointID);
    }

}

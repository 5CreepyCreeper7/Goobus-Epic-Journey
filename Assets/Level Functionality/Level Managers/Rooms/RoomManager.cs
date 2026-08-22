using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Persistent References")]
    [SerializeField] private TransitionAnimation transitionAnimation;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private CinemachineConfiner2D confiner;

    [Header("Starting Room")]
    [SerializeField] private int startingSpawnPointID = 1;

    [Header("PlayerEntranceAnimation")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D playerRigidBody;
    [SerializeField] private float entranceWalkSpeed = 2f;
    [SerializeField] private float entranceRiseSpeed = 5f;

    private string currentRoomScene;
    private int currentSpawnPointID;
    private RoomObject currentRoom;
    private bool inTransition;

    public bool InTransition => inTransition;

    private string bonusReturnScene;
    private int bonusReturnSpawnPointID;
    private bool hasBonusReturnLocation;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private IEnumerator Start() {
        currentRoomScene = SceneManager.GetActiveScene().name;
        currentSpawnPointID = startingSpawnPointID;

        yield return null;

        currentRoom = FindRoomObjectInActiveScene();

        if(currentRoom == null) {
            Debug.LogError($"No RoomObject found in scene {currentRoomScene}");
            yield break;
        }

        UpdateCameraBounds();
    }

    public void TransitionToRoom(string targetSceneName, int targetSpawnPointID) {
        if(inTransition) return;

        StartCoroutine(TransitionRoutine(targetSceneName, targetSpawnPointID));
    }

    private IEnumerator TransitionRoutine(string targetSceneName, int targetSpawnPointID)
{
        inTransition = true;

        if(transitionAnimation != null) {
            yield return transitionAnimation.FadeOut();
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

        if(loadOperation == null) {
            Debug.LogError($"could not load the scene {targetSceneName}" + "Add it to the build Profile");

            inTransition = false;
            yield break;
        }

        while(!loadOperation.isDone) {
            yield return null;
        }

        yield return null;

        currentRoomScene = targetSceneName;
        currentRoom = FindFirstObjectByType<RoomObject>();

        if(currentRoom == null) {
            Debug.LogError($"no roomobject was found in scene {targetSceneName}");

            inTransition = false;
            yield break;
        }

        RoomSpawnPoint targetSpawnPoint = currentRoom.GetSpawnPoint(targetSpawnPointID);

        if(targetSpawnPoint == null) {
            Debug.LogError($"Spawn Point {targetSpawnPointID} was not found" + $"in scene {targetSceneName}");

            inTransition = false;
            yield break;
        }

        playerMovement.SetControlsLocked(true);
        playerRigidBody.linearVelocity = Vector2.zero;

        playerPosition.position = targetSpawnPoint.transform.position;
        currentSpawnPointID = targetSpawnPointID;

        Physics2D.SyncTransforms();

        UpdateCameraBounds();

        yield return new WaitForEndOfFrame();

        if(transitionAnimation != null) {
            yield return transitionAnimation.FadeIn();
        }

        yield return PlayEntrance(targetSpawnPoint);

        playerRigidBody.linearVelocity = Vector2.zero;
        playerMovement.SetControlsLocked(false);

        inTransition = false;
    }

    private RoomObject FindRoomObjectInActiveScene() {
        return FindRoomObjectInScene(SceneManager.GetActiveScene());
    }

    private RoomObject FindRoomObjectInScene(Scene scene) {
        if (!scene.IsValid() || !scene.isLoaded)
            return null;

        foreach (GameObject rootObject in scene.GetRootGameObjects()) {
            RoomObject roomObject =
                rootObject.GetComponentInChildren<RoomObject>(true);

            if (roomObject != null)
                return roomObject;
        }

        return null;
    }

    private void UpdateCameraBounds() {
        if(confiner == null || currentRoom == null || currentRoom.CameraBounds == null) {
            return;
        }

        confiner.BoundingShape2D = currentRoom.CameraBounds;
        confiner.InvalidateBoundingShapeCache();
    }

    public int GetCurrentSpawnPointID() {
        return currentSpawnPointID;
    }

    public RoomSpawnPoint GetCurrentSpawnPoint() {
        if(currentRoom == null) {
            return null;
        }

        return currentRoom.GetSpawnPoint(currentSpawnPointID);
    }

    private IEnumerator PlayEntrance(RoomSpawnPoint spawnPoint)
    {
        switch(spawnPoint.EntranceType)
        {
            case RoomEntranceType.Walk:
                yield return PlayWalkEntrance(spawnPoint.EntryEnd);
                break;
            case RoomEntranceType.DropDown:
                yield return PlayFallEntrance(spawnPoint.EntryEnd);
                break;
            case RoomEntranceType.RiseUp:
                yield return PlayRiseUpEntrance(spawnPoint.EntryEnd);
                break;
            case RoomEntranceType.Stand:
                yield return PlayStandEntrance();
                break;
        }
    }

    private IEnumerator PlayStandEntrance()
    {
        playerMovement.isCrouching = true;

        yield return new WaitForSeconds(1);

        playerMovement.isCrouching = false;
    }

    private IEnumerator PlayWalkEntrance(Vector2 endPoint)
    {
        while(Mathf.Abs(playerPosition.position.x - endPoint.x) > 0.05f)
        {
            float direction = Mathf.Sign(endPoint.x - playerPosition.position.x);

            playerRigidBody.linearVelocity = new Vector2(direction * entranceWalkSpeed, playerRigidBody.linearVelocity.y);

            yield return new WaitForFixedUpdate();
        }

        playerPosition.position = new Vector2(endPoint.x, playerPosition.position.y);

        playerRigidBody.linearVelocity = Vector2.zero;
    }

    private IEnumerator PlayFallEntrance(Vector2 endPoint)
    {
        while(playerPosition.position.y > endPoint.y)
        {
            yield return new WaitForFixedUpdate();
        }  
    }

    private IEnumerator PlayRiseUpEntrance(Vector2 endPoint)
    {
        while(playerPosition.position.y < endPoint.y)
        {
            playerRigidBody.linearVelocity = new Vector2(0f, entranceRiseSpeed);

            yield return new WaitForFixedUpdate();
        }

        playerPosition.position = new Vector2(playerPosition.position.x, endPoint.y);

        playerRigidBody.linearVelocity = Vector2.zero;
    }

    public void EnterBonusStage(string bonusScene, int bonusSpawnPointID, int returnSpawnPointID)
    {
        if(inTransition)
        {
            return;
        }

        bonusReturnScene = currentRoomScene;
        bonusReturnSpawnPointID = returnSpawnPointID;
        hasBonusReturnLocation = true;

        TransitionToRoom(bonusScene, bonusSpawnPointID);
    }

    public void ExitBonusStage()
    {
        if(inTransition || !hasBonusReturnLocation)
        {
            return;
        }

        string targetScene = bonusReturnScene;
        int targetSpawnPoint = bonusReturnSpawnPointID;

        hasBonusReturnLocation = false;

        TransitionToRoom(targetScene, targetSpawnPoint);
    }
}

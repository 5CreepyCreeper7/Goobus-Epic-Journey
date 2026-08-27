using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;


public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Persistent References")]
    private Transform playerPosition => PlayerMovement.Instance.transform;

    [Header("Starting Room")]
    [SerializeField] private int startingSpawnPointID = 1;

    [Header("PlayerEntranceAnimation")]
    private Rigidbody2D playerRigidBody => PlayerMovement.Instance.rb;
    [SerializeField] private float entranceWalkSpeed = 2f;
    [SerializeField] private float entranceRiseSpeed = 5f;

    [Header("BonusRoomTransitionSettings")]
    [SerializeField] private float effectRampDuration;
    [SerializeField] private float effectIntensity;
    [SerializeField] private float transitionEndAberration;
    [SerializeField] private float defaultEffectValue;
    [SerializeField] private float transitionEndEffectRampDuration;
    [SerializeField] private float transitionEndIntensity;
    [SerializeField] private float exitEffectRampDuration;
    [SerializeField] private float transitionEndHue;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Skip the bootstrap scene itself — there's no RoomObject there and nothing to do yet
        if (scene.name == "BootStrapScene" || scene.name == "TitleScreen") {
            return;
        }

       InitializeForScene(scene);
       
    }

    public void InitializeForScene(Scene scene)
    {
        currentRoomScene = scene.name;
        currentSpawnPointID = startingSpawnPointID;

        currentRoom = FindRoomObjectInScene(scene);

        if(currentRoom == null) {
            Debug.LogError($"No RoomObject found in scene {currentRoomScene}");
            return;
        }

        UpdateCameraBounds();
    }

    public void TransitionToRoom(string targetSceneName, int targetSpawnPointID, RoomTransitionType transitionType = RoomTransitionType.Normal) {
        if(inTransition) return;

        StartCoroutine(TransitionRoutine(targetSceneName, targetSpawnPointID, transitionType));
    }

    private IEnumerator TransitionRoutine(string targetSceneName, int targetSpawnPointID, RoomTransitionType transitionType)
    {
        inTransition = true;

        PlayerMovement.Instance.SetControlsLocked(true);

        if(transitionType == RoomTransitionType.EnterBonus)
        {
            //Animation for eating berries.

            BonusLevelShaderController.Instance.RampWaveTo(effectIntensity, effectRampDuration);

            yield return new WaitForSecondsRealtime(effectRampDuration / 2);

            PlayerMovement.Instance.playerAnimationScript.ForceCrouching();

            yield return new WaitForSecondsRealtime(effectRampDuration / 2);

            BonusLevelShaderController.Instance.RampHueTo(defaultEffectValue, effectRampDuration);
            BonusLevelShaderController.Instance.RampAberrationTo(defaultEffectValue, effectRampDuration);

            yield return new WaitForSecondsRealtime(effectRampDuration);
        } else if(transitionType == RoomTransitionType.ExitBonus)
        {
            PlayerMovement.Instance.playerAnimationScript.ForceCrouching();
            yield return new WaitForSecondsRealtime(effectRampDuration / 2);
        }

        if(TransitionAnimation.Instance != null) {
            yield return TransitionAnimation.Instance.FadeOut();
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
        currentRoom = FindAnyObjectByType<RoomObject>();

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

        PlayerMovement.Instance.SetControlsLocked(true);
        playerRigidBody.linearVelocity = Vector2.zero;

        Vector3 previousPosition = playerPosition.position;
        playerPosition.position = targetSpawnPoint.transform.position;
        Vector3 positionDelta = targetSpawnPoint.transform.position - previousPosition;

        currentSpawnPointID = targetSpawnPointID;

        Physics2D.SyncTransforms();

        if (transitionType == RoomTransitionType.EnterBonus || transitionType == RoomTransitionType.ExitBonus) {
            PlayerMovement.Instance.playerAnimationScript.ForceCrouching();
        }   

        CameraController.Instance?.SnapToTarget(playerPosition, positionDelta);

        UpdateCameraBounds();

        yield return null;
        yield return null;

        yield return new WaitForSecondsRealtime(0.75f);

        if(TransitionAnimation.Instance != null) {
            yield return TransitionAnimation.Instance.FadeIn();
        }

        //yield return PlayEntrance(targetSpawnPoint);

        switch(transitionType)
        {
            case RoomTransitionType.EnterBonus:
                BonusLevelShaderController.Instance.RampWaveTo(transitionEndIntensity, transitionEndEffectRampDuration);
                BonusLevelShaderController.Instance.RampAberrationTo(transitionEndAberration, transitionEndEffectRampDuration);
                BonusLevelShaderController.Instance.RampHueTo(transitionEndHue, transitionEndEffectRampDuration);
                yield return new WaitForSecondsRealtime(transitionEndEffectRampDuration);
                PlayerMovement.Instance.playerAnimationScript.ForceStanding();
                break;
            case RoomTransitionType.ExitBonus:
                BonusLevelShaderController.Instance.RampTo(0f, exitEffectRampDuration);
                yield return new WaitForSecondsRealtime(exitEffectRampDuration);
                PlayerMovement.Instance.playerAnimationScript.ForceStanding();
                break;
            case RoomTransitionType.Normal:
            default:
                yield return PlayEntrance(targetSpawnPoint);
                break;
        }

        playerRigidBody.linearVelocity = Vector2.zero;
        PlayerMovement.Instance.SetControlsLocked(false);

        inTransition = false;
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
        if(currentRoom == null || currentRoom.CameraBounds == null || CameraController.Instance == null) {
            return;
        }

        CameraController.Instance.Confiner.BoundingShape2D = currentRoom.CameraBounds;
        CameraController.Instance.Confiner.InvalidateBoundingShapeCache();
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
        PlayerMovement.Instance.isCrouching = true;

        yield return new WaitForSeconds(1);

        PlayerMovement.Instance.isCrouching = false;
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

        TransitionToRoom(bonusScene, bonusSpawnPointID, RoomTransitionType.EnterBonus);
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

        TransitionToRoom(targetScene, targetSpawnPoint, RoomTransitionType.ExitBonus);
    }

    public void LoadSceneAfterTeardown(string sceneName) {
        StartCoroutine(LoadSceneAfterTeardownRoutine(sceneName));
    }

    private IEnumerator LoadSceneAfterTeardownRoutine(string sceneName) {
        yield return null; 
        SceneManager.LoadScene(sceneName);
    }
}

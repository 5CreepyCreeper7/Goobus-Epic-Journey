using UnityEngine;
using UnityEngine.SceneManagement;

public class DevSceneBootstrapper : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private GameObject persistentGameManagersPrefab;
    [SerializeField] private GameObject persistentGameplayObjectsPrefab;

    private void Awake()
    {
        // If RoomManager already exists, we came through the normal Bootstrap → Title → Start
        // flow — nothing to do here.
        if (RoomManager.Instance != null) {
            return;
        }

        Debug.Log("DevSceneBootstrapper: no persistent managers found, spawning for direct-play testing.");

        Instantiate(persistentGameManagersPrefab);
        Instantiate(persistentGameplayObjectsPrefab);

        Scene currentScene = SceneManager.GetActiveScene();
        RoomManager.Instance.InitializeForScene(currentScene);

        RoomSpawnPoint spawnPoint = RoomManager.Instance.GetCurrentSpawnPoint();
        
        if (spawnPoint != null) {
            PlayerMovement.Instance.transform.position = spawnPoint.transform.position;
        }
    }
#endif
}
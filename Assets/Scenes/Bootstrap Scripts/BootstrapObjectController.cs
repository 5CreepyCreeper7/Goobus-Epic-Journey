using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapObjectController : MonoBehaviour
{
    public  GameObject[] bootstrapObjects;

    public string[] disabledScenes;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        bool shouldDisable = false;

        foreach (string sceneName in disabledScenes) {
            if (scene.name == sceneName) {
                shouldDisable = true;
                break;
            }
        }

        foreach (GameObject obj in bootstrapObjects) {
            obj.SetActive(!shouldDisable);
        }
    }
}
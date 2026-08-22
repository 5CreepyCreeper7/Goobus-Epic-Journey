using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistObject : MonoBehaviour
{
    private static PersistObject instance;

    private void Awake()
    {
        Debug.Log($"PersistObject Awake: {gameObject.name}");

        if (instance != null && instance != this) {
            Debug.Log($"Destroying duplicate: {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"Persistent object registered: {gameObject.name}");
    }

    private void OnDestroy() {
        Debug.Log($"PersistObject destroyed: {gameObject.name}");
    }
}

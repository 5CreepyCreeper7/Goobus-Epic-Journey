using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistObject : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // keep persistent
    }
}

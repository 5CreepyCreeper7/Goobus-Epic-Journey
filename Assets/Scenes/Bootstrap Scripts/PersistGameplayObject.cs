using UnityEngine;

public class PersistGameplayObject : MonoBehaviour
{
    public static PersistGameplayObject Instance {get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void TearDown()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }

}

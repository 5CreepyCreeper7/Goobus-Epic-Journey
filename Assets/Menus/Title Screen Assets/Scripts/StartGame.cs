using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject persistentGameObjectsPrefab;

    public void LoadGame()
    {
        Instantiate(persistentGameObjectsPrefab);
        SceneManager.LoadScene("Room001");
    }
}

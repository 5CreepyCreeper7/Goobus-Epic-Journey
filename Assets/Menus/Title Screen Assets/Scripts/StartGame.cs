using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Room001");
    }
}

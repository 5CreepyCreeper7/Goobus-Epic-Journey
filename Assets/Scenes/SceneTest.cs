using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            SceneManager.LoadScene("TestScene");
        }
    }
}

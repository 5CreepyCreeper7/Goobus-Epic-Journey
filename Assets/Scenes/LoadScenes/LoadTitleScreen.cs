using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTitleScreen : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

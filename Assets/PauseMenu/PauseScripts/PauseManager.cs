using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject videoSettingsMenu;
    public GameObject audioSettingsMenu;

    public InputActionReference pauseAction;

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseAction.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        optionsMenu.SetActive(true);
        Time.timeScale = 0f; 

        isPaused = true;
    }

    public void ResumeGame()
    {
        optionsMenu.SetActive(false);
        audioSettingsMenu.SetActive(false);
        videoSettingsMenu.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void ExitToTitleScreen()
    {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }

    public void OpenVideoSettingsMenu()
    {
        optionsMenu.SetActive(false);
        videoSettingsMenu.SetActive(true);
    }

    public void OpenAudioSettingsMenu()
    {
        optionsMenu.SetActive(false);
        audioSettingsMenu.SetActive(true);
    }

    public void CloseVideoSettingsMenu()
    {
        videoSettingsMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseAudioSettingsMenu()
    {
        audioSettingsMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}

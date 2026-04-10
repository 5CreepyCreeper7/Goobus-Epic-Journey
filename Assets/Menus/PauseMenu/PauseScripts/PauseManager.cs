using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject optionsMenu;
    public GameObject videoSettingsMenu;
    public GameObject audioSettingsMenu;
    public GameObject dialogPanel;

    public InputActionReference pauseAction;
    
    public PlayerMovement playerMovement;
    public DialogLogicScript DialogLogicScript;

    public bool isPaused = false;
    public bool wasInDialog = false;

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

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(scene.name == "TutorialScene" || scene.name == "SampleScene") {
            PauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            videoSettingsMenu.SetActive(false);
            audioSettingsMenu.SetActive(false);
            Time.timeScale = 1f; 
            isPaused = false;
        }

        if(scene.name == "TitleScreen") {
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void PauseGame()
    {
        playerMovement.enabled = false;
        PauseMenu.SetActive(true);

        if(DialogLogicScript.isDialogActive) {
            wasInDialog = true;
            dialogPanel.SetActive(false);
        }

        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void ResumeGame()
    {
        optionsMenu.SetActive(false);
        audioSettingsMenu.SetActive(false);
        videoSettingsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        if(wasInDialog) {
            dialogPanel.SetActive(true);
            wasInDialog = false;
        }
        
        Time.timeScale = 1f; 
        playerMovement.enabled = true;
        isPaused = false;
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

    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void ReturnToTitleScreen() {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }


}

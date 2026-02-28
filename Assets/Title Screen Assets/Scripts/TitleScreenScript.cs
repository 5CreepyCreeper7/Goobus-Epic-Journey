using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour
{

    public GameObject TitlePanel;
    public GameObject OptionsPanel;
    public GameObject AudioSettingsPanel;
    public GameObject VideoSettingsPanel;

    private void Start()
    {
        TitlePanel.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void OpenOptionsMenu()
    {
        TitlePanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        TitlePanel.SetActive(true);
        OptionsPanel.SetActive(false);
    }

    public void OpenAudioSettingsMenu()
    {
        OptionsPanel.SetActive(false);
        AudioSettingsPanel.SetActive(true);
    }

    public void OpenVideoSettingsMenu()
    {
        OptionsPanel.SetActive(false);
        VideoSettingsPanel.SetActive(true);
    }

    public void CloseAudioSettingsMenu()
    {
        AudioSettingsPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void CloseVideoSettingsMenu()
    {
        VideoSettingsPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }
}

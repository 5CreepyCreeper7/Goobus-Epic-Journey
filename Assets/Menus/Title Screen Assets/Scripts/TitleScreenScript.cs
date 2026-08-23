using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TitleScreenScript : MonoBehaviour
{

    public GameObject TitlePanel;
    public GameObject OptionsPanel;
    public GameObject AudioSettingsPanel;
    public GameObject VideoSettingsPanel;
    public GameObject goobus;

    private TitleGoobusAnimations goobusAnimations;

    private void Start()
    {
        goobusAnimations = goobus.GetComponent<TitleGoobusAnimations>();
        if (goobusAnimations == null) {
            Debug.LogWarning("TitleGoobusAnimations not found on goobus object.");
        }
    }

    public void OpenOptionsMenu()
    {
        TitlePanel.SetActive(false);
        OptionsPanel.SetActive(true);
        goobusAnimations.setMenuInt(1);
    }

    public void CloseOptionsMenu()
    {
        TitlePanel.SetActive(true);
        OptionsPanel.SetActive(false);
        goobusAnimations.setMenuInt(0);
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

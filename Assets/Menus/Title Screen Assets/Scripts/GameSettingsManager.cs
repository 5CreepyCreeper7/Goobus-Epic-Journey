using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSettingsManager : MonoBehaviour
{
    public Toggle muteToggle;
    public Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        SetupResolutions();
        LoadSavedSettings();
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void LoadSavedSettings()
    {
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.SetIsOnWithoutNotify(isFullScreen);

        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        AudioListener.volume = isMuted ? 0 : 1;
        muteToggle.SetIsOnWithoutNotify(isMuted);
    }

    public void ToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleMute(bool isMuted)
    {
        AudioListener.volume = isMuted ? 0 : 1;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
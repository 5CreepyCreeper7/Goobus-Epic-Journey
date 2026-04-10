using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider dialogueSlider;
    public Toggle muteToggle;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        dialogueSlider.value = AudioManager.Instance.GetDialogueVolume();
        muteToggle.isOn = AudioManager.Instance.IsMuted();

        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        dialogueSlider.onValueChanged.AddListener(AudioManager.Instance.SetDialogueVolume);
        muteToggle.onValueChanged.AddListener(AudioManager.Instance.SetMute);
    }
}

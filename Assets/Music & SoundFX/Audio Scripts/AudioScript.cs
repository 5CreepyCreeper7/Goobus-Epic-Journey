using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;

    private bool isMuted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumes();
    }

    private void LoadVolumes()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
        SetDialogueVolume(PlayerPrefs.GetFloat("DialogueVolume", 1f));
        SetMute(PlayerPrefs.GetInt("IsMuted", 0) == 1);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SoundFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetDialogueVolume(float volume)
    {
        audioMixer.SetFloat("Dialogue", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("DialogueVolume", volume);
    }

    public float GetMusicVolume() { return PlayerPrefs.GetFloat("MusicVolume", 1f); }

    public float GetSFXVolume() { return PlayerPrefs.GetFloat("SFXVolume", 1f); }

    public float GetDialogueVolume(){ return PlayerPrefs.GetFloat("DialogueVolume", 1f); }

    public void ToggleMute()
    {
        SetMute(!isMuted);
    }

    public void SetMute(bool mute)
    {
        isMuted = mute;
        audioMixer.SetFloat("Master", mute ? -80f : 0f);
        PlayerPrefs.SetInt("IsMuted", mute ? 1 : 0);
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
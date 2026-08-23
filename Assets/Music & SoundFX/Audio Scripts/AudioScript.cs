using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float dialogueVolume = 1f;

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
        musicVolume = volume;
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        audioMixer.SetFloat("SoundFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetDialogueVolume(float volume)
    {
        dialogueVolume = volume;
        audioMixer.SetFloat("Dialogue", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("DialogueVolume", volume);
        PlayerPrefs.Save();
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
        PlayerPrefs.Save();
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
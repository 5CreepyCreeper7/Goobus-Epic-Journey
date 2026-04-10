using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource musicSource;

    public AudioClip titleTheme;
    public AudioClip TutorialTheme;
    public AudioClip Lvl1Theme;

    private void Awake()
    {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "TitleScreen":
                PlayMusic(titleTheme);
                break;
            case "TutorialScene":
                PlayMusic(TutorialTheme);
                break;
            case "SampleScene":
                PlayMusic(Lvl1Theme);
                break;
            default:
                musicSource.Stop();
                break;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Avoid restarting the same music

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        // This method can be expanded to set the volume of sound effects as well
    }
}



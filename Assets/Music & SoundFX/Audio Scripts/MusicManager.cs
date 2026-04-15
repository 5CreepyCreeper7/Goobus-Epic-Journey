using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    
    public AudioManager audioManager;

    public AudioSource musicSource;

    public AudioClip titleTheme;
    public AudioClip TutorialTheme;
    public AudioClip Lvl1Theme;

    private Coroutine fadeCoroutine;

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
                StartMusicWithFade(titleTheme, 2f);
                break;
            case "TutorialScene":
                StartMusicWithFade(TutorialTheme, 2f);
                break;
            case "SampleScene":
                StartMusicWithFade(Lvl1Theme, 2f);
                break;
            default:
                musicSource.Stop();
                break;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void fadeInMusic(float duration) {
        StartCoroutine(FadeInMusicCoroutine(duration));
    }

    private IEnumerator FadeInMusicCoroutine(float duration) {
        float targetVolume = AudioManager.Instance.GetMusicVolume();
        musicSource.volume = 0f;
        musicSource.Play();

        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = targetVolume; // Ensure it ends at the target volume
        fadeCoroutine = null;
    }

    private void StartMusicWithFade(AudioClip clip, float duration)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        musicSource.clip = clip;
        musicSource.loop = true;
        fadeCoroutine = StartCoroutine(FadeInMusicCoroutine(duration));
    }
}



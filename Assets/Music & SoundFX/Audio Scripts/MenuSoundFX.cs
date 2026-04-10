using UnityEngine;

public class MenuSoundFX : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip backSound;
    [SerializeField] private AudioClip openMenuSound;

    [SerializeField] private AudioSource uiAudioSource;

    public void PlayButtonClick()
    {
        uiAudioSource.PlayOneShot(buttonClickSound);
    }

    public void PlayHoverSound()
    {
        uiAudioSource.pitch = Random.Range(0.90f, 1.10f);
        uiAudioSource.PlayOneShot(hoverSound);
        uiAudioSource.pitch = 1f;
    }

    public void PlayBackSound()
    {
        uiAudioSource.PlayOneShot(backSound);
    }

    public void PlayOpenMenuSound()
    {
        uiAudioSource.PlayOneShot(openMenuSound);
    }
}

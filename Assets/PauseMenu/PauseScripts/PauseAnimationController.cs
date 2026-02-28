using UnityEngine;

public class PauseAnimationController : MonoBehaviour
{
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PauseAnimation() {
        anim.SetBool("IsPaused", true);
        anim.SetBool("InAudio", false);
        anim.SetBool("InVideo", false);
    }

    public void AudioSettingsAnimation() {
        anim.SetBool("InAudio", true);
        anim.SetBool("IsPaused", false);
        anim.SetBool("InVideo", false);
    }

    public void VideoMenuAnimation() {
        anim.SetBool("InVideo", true);
        anim.SetBool("IsPaused", false);
        anim.SetBool("InAudio", false);
    }
}

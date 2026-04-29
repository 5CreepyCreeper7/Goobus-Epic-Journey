using UnityEngine;
using System.Collections;

public class PlayerSoundFX : MonoBehaviour
{

    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip deathSound;
    public AudioClip walkSound;
    public AudioClip fallingSound;
    public AudioClip dashSound;
    public AudioClip hurtSound;

    public float walkSoundMinPitch = 1f;
    public float walkSoundMaxPitch = 2f;

    public float jumpSoundMinPitch = 0.8f;
    public float jumpSoundMaxPitch = 1f;
    
    public float walkSoundSpeed = .8f;
    public float jumpSoundSpeed = .8f;

    private AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void playJumpSound() {
        StartCoroutine(pitchJumpingSound());
    }

    public void playDashSound() {
        audioSource.PlayOneShot(dashSound);
    }

    public void playDeathSound() {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(deathSound);
    }

    public void playHurtSound() {
        audioSource.PlayOneShot(hurtSound);
    }

    public void playWalkSound() {
        StartCoroutine(pitchWalkSound());
    }

    public void playFallingSound() {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(fallingSound);
    }

    public void playLandingSound() {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(landingSound);
    }

    IEnumerator pitchWalkSound() {
        float randomPitch = Random.Range(walkSoundMinPitch, walkSoundMaxPitch);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(walkSound);
        yield return new WaitForSeconds(walkSoundSpeed);
        audioSource.pitch = 1f;
    }

    IEnumerator pitchJumpingSound() {
        float randomPitch = Random.Range(jumpSoundMinPitch, jumpSoundMaxPitch);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(jumpSound);
        yield return new WaitForSeconds(jumpSoundSpeed);
        audioSource.pitch = 1f;
    }

    public void stopFallingSound() {
        audioSource.Stop();
    }
}

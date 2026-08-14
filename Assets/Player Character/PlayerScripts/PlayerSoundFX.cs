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
    public AudioClip dashAttackSound;

    public float walkSoundMinPitch = 1f;
    public float walkSoundMaxPitch = 2f;

    public float jumpSoundMinPitch = 0.8f;
    public float jumpSoundMaxPitch = 1f;
    
    public float walkSoundSpeed = .8f;
    public float jumpSoundSpeed = .8f;

    public AudioSource MovementAudioSource;
    public AudioSource OtherAudioSource;

    void Awake() {
        MovementAudioSource = GetComponent<AudioSource>();
    }

    public void playJumpSound() {
        StartCoroutine(pitchJumpingSound());
    }

    public void playDashSound() {
        MovementAudioSource.PlayOneShot(dashSound);
    }

    public void playDeathSound() {
        MovementAudioSource.pitch = 1f;
        MovementAudioSource.PlayOneShot(deathSound);
    }

    public void playHurtSound() {
        MovementAudioSource.PlayOneShot(hurtSound);
    }

    public void playWalkSound() {
        StartCoroutine(pitchWalkSound());
    }

    public void playFallingSound() {
        MovementAudioSource.pitch = 1f;
        MovementAudioSource.PlayOneShot(fallingSound);
    }

    public void playLandingSound() {
        MovementAudioSource.pitch = 1f;
        MovementAudioSource.PlayOneShot(landingSound);
    }

    public void playDashAttackSound() {
        OtherAudioSource.pitch = 1f;
        OtherAudioSource.PlayOneShot(dashAttackSound);
    }

    IEnumerator pitchWalkSound() {
        float randomPitch = Random.Range(walkSoundMinPitch, walkSoundMaxPitch);
        MovementAudioSource.pitch = randomPitch;
        MovementAudioSource.PlayOneShot(walkSound);
        yield return new WaitForSeconds(walkSoundSpeed);
        MovementAudioSource.pitch = 1f;
    }

    IEnumerator pitchJumpingSound() {
        float randomPitch = Random.Range(jumpSoundMinPitch, jumpSoundMaxPitch);
        MovementAudioSource.pitch = randomPitch;
        MovementAudioSource.PlayOneShot(jumpSound);
        yield return new WaitForSeconds(jumpSoundSpeed);
        MovementAudioSource.pitch = 1f;
    }

    public void stopFallingSound() {
        MovementAudioSource.Stop();
    }
}

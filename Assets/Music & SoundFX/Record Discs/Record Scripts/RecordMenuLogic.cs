using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordMenuLogic : MonoBehaviour
{
    public Sprite[] recordSprites = {};
    public AudioClip[] songs = {};
    public CurrentlyPlaying[] songEntries = {};

    public ParticleSystem EighthNoteParticles;
    public ParticleSystem QuarterNoteParticles;

    public Image spinningRecordImage;
    public Image recordArmImage;

    public Slider ProgressBar;

    public TextMeshProUGUI timeElapsedText;
    public TextMeshProUGUI TotalTimeText;

    public Button pauseButton;
    private Image pauseButtonImage;
    public Sprite pauseIcon;
    public Sprite playIcon;

    public float armInitialAngle = 0f;
    public float armTargetAngle = -50f;
    public float armRotationSpeed = 100f;

    private bool moveArm = false;
    private bool isPaused = false;
    private bool isDraggingSlider = false;

    private AudioSource audioSource;
    private AudioSource mainAudioSource;

    private void Awake() {
        audioSource = GameObject.FindGameObjectWithTag("RecordPlayer").GetComponent<AudioSource>();
        mainAudioSource = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioSource>();
        pauseButtonImage = pauseButton.GetComponent<Image>();
        ProgressBar.value = 0f;
    }

    private void Update() {
        if(!isPaused) {
            RecordSpinning();
        }

        UpdateProgressBar();

        if(moveArm) {
            RotateArm();
        }

        updateTimeElapsed();
    }

    public void PlayRecord(int recordIndex) {
        Debug.Log("Clicked record index: " + recordIndex);

        if(recordIndex < 0 || recordIndex >= songs.Length || recordIndex >= recordSprites.Length) {
            Debug.LogError("Invalid record index: " + recordIndex);
            return;
        }

        Debug.Log("Song at index is: " + songs[recordIndex]);
        Debug.Log("Sprite at index is: " + recordSprites[recordIndex]);


        isPaused = false;

        StopCurrentRecord();

        updatePauseIcon();

        spinningRecordImage.sprite = recordSprites[recordIndex];
        spinningRecordImage.enabled = true;

        PauseMainAudio();

        audioSource.clip = songs[recordIndex];
        getTotalTime();
        audioSource.Play();

        HighlightCurrentSong(recordIndex);

        StartArmMovement();
    }

    public void StopCurrentRecord() {
        if (audioSource.clip != null) {
            audioSource.Stop();
            audioSource.clip = null;
        }

        isPaused = false;

        ProgressBar.value = 0f;
        timeElapsedText.text = "0:00";

        ResetSongColors();

        if (EighthNoteParticles != null && EighthNoteParticles.isPlaying) {
            EighthNoteParticles.Stop();
        }
        if (QuarterNoteParticles != null && QuarterNoteParticles.isPlaying) {
            QuarterNoteParticles.Stop();
        }

        spinningRecordImage.enabled = false;
    }

    public void ResumeMainAudio() {
        if(!mainAudioSource.isPlaying) {
            mainAudioSource.UnPause();
        }
    }

    public void PauseMainAudio() {
        if(mainAudioSource.isPlaying) {
            mainAudioSource.Pause();
        }
    }

    private void RecordSpinning() {
        if(spinningRecordImage.enabled) {
            spinningRecordImage.transform.Rotate(0f, 0f, -100f * Time.deltaTime);
        }
    }

    private void RotateArm() {
        float currentZ = recordArmImage.transform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, armTargetAngle, armRotationSpeed * Time.deltaTime);

        recordArmImage.transform.localEulerAngles = new Vector3(0, 0, newZ);

        if(Mathf.Abs(Mathf.DeltaAngle(newZ, armTargetAngle)) < 0.1f) {
            moveArm = false;

            if (EighthNoteParticles != null && audioSource.isPlaying) {
                EighthNoteParticles.Play();
            }
            if (QuarterNoteParticles != null && audioSource.isPlaying) {
                QuarterNoteParticles.Play();
            }
        }
    }

    public void ResetArm() {
        recordArmImage.transform.localEulerAngles = new Vector3(0, 0, armInitialAngle);
        moveArm = false;
    }

    public void StartArmMovement() {
        moveArm = true;
    }

    public void PauseButton() {
        if(audioSource.clip == null) {
            return;
        }

        if(audioSource.isPlaying) {
            audioSource.Pause();
            isPaused = true;
            updatePauseIcon();
            if(EighthNoteParticles != null && EighthNoteParticles.isPlaying) {
                EighthNoteParticles.Stop();
            }
            if(QuarterNoteParticles != null && QuarterNoteParticles.isPlaying) {
                QuarterNoteParticles.Stop();
            }
        } else {
            audioSource.UnPause();
            isPaused = false;
            updatePauseIcon();
            if(EighthNoteParticles != null) {
                EighthNoteParticles.Play();
            }
            if(QuarterNoteParticles != null) {
                QuarterNoteParticles.Play();
            }
        }
    }

    public void UpdateProgressBar() {
        if(audioSource.clip != null && !isDraggingSlider) {
            ProgressBar.value = audioSource.time / audioSource.clip.length;
        } 
    }

    public void ResetProgressBar() {
        ProgressBar.value = 0f;
    }

    public void OnSliderPointerDown() {
        isDraggingSlider = true;
    }

    public void OnSliderPointerUp() {
        if(audioSource.clip != null) {
            audioSource.time = ProgressBar.value * audioSource.clip.length;
        }
        isDraggingSlider = false;
    }

    public void updateTimeElapsed() {
        if(audioSource.clip != null) {
            float currentTime = audioSource.time;
            timeElapsedText.text = FormatTime(currentTime);
        }
    }

    public void getTotalTime() {
        if(audioSource.clip != null) {
            float totalTime = audioSource.clip.length;
            TotalTimeText.text = FormatTime(totalTime);
        }
    }

    public void updatePauseIcon() {
        if(isPaused) {
            pauseButtonImage.sprite = playIcon;
        } else {
            pauseButtonImage.sprite = pauseIcon;
        }
    }

    private string FormatTime(float time) {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        return minutes.ToString() + ":" + seconds.ToString("00");
    }

    private void HighlightCurrentSong(int index)
    {
        for (int i = 0; i < songEntries.Length; i++)
        {
            songEntries[i].SetPlaying(i == index);
        }
    }

    public void ResetSongColors()
    {
        for (int i = 0; i < songEntries.Length; i++)
        {
            songEntries[i].SetPlaying(false);
        }
    }

    public void ResetTimeText() {
        timeElapsedText.text = "0:00";
        TotalTimeText.text = "0:00";
    }

    public void ResetRecordSprite() {
        spinningRecordImage.enabled = false;
    }

    public void ResetMenu() {
        isPaused = false;
        updatePauseIcon();
        ResetArm();
        StopCurrentRecord();
        ResumeMainAudio();
        ResetProgressBar();
        ResetSongColors();
        ResetRecordSprite();
        ResetTimeText();
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    public Button shuffleButton;
    private Image shuffleButtonImage;
    public Sprite shuffleIcon;
    public Sprite shuffleActiveIcon;

    public Button loopButton;
    private Image loopButtonImage;
    public Sprite loopIcon;
    public Sprite loopActiveIcon;

    public bool shuffling = false;
    public bool looping = false;
    private int currentRecordIndex = -1;

    private List<int> songHistory = new List<int>();
    private int historyIndex = -1;

    [Header("Record Arm Settings")]
    public float armInitialAngle = 0f;
    public float armTargetAngle = -50f;
    public float currentArmTargetAngle = 0f;
    public float armRotationSpeed = 100f;

    [Header("Record Disc Settings")]
    public float maxSpinSpeed = 100f;
    public float spinAcceleration = 4f;
    public float spinDeceleration = 2f;

    private float currentSpinSpeed = 0f;
    private bool shouldSpin = false;

    private float lastSliderValue = 0f;
    public float seekSpinMultiplier = 200f;

    private bool moveArm = false;
    private bool isPaused = false;
    private bool isDraggingSlider = false;

    private AudioSource audioSource;
    private AudioSource mainAudioSource;

    [Header("Visualizer Settings")]
    public float pulseIntensity = 1.5f;
    public float pulseSmoothness = 5f;

    private Vector3 recordBaseScale;
    private float[] audioSamples = new float[512];

    private void Awake() {
        audioSource = GameObject.FindGameObjectWithTag("RecordPlayer").GetComponent<AudioSource>();
        mainAudioSource = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioSource>();
        pauseButtonImage = pauseButton.GetComponent<Image>();
        shuffleButtonImage = shuffleButton.GetComponent<Image>();
        loopButtonImage = loopButton.GetComponent<Image>();
        recordBaseScale = spinningRecordImage.transform.localScale;
        ProgressBar.value = 0f;
    }

    private void Update() {
        if(isDraggingSlider) {
            SeekingRecordSpinEffect();
        } else {
            RecordSpinning();
        }
        
        PulseRecord();

        UpdateProgressBar();

        if(moveArm) {
            RotateArm();
        }

        updateTimeElapsed();

        CheckIfSongEnded();
    }

    public void PlayRecord(int recordIndex) {
        PlayRecord(recordIndex, true);
    }

    public void PlayRecord(int recordIndex, bool updateHistory) {
        Debug.Log("Clicked record index: " + recordIndex);

        if(recordIndex < 0 || recordIndex >= songs.Length || recordIndex >= recordSprites.Length) {
            Debug.LogError("Invalid record index: " + recordIndex);
            return;
        }

        Debug.Log("Song at index is: " + songs[recordIndex]);
        Debug.Log("Sprite at index is: " + recordSprites[recordIndex]);

        currentRecordIndex = recordIndex;

        if(updateHistory) {
            AddToHistory(recordIndex);
        }

        isPaused = false;

        StopCurrentRecord();

        updatePauseIcon();

        spinningRecordImage.sprite = recordSprites[recordIndex];
        spinningRecordImage.enabled = true;

        PauseMainAudio();

        audioSource.clip = songs[recordIndex];
        getTotalTime();

        HighlightCurrentSong(recordIndex);

        StartArmMovement(armTargetAngle);
    }

    public void StopCurrentRecord() {
        if (audioSource.clip != null) {
            audioSource.Stop();
            audioSource.clip = null;
        }

        shouldSpin = false;
        currentSpinSpeed = 0f;

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

        StartArmMovement(armInitialAngle);

        spinningRecordImage.enabled = false;
    }

    private void CheckIfSongEnded() {
        if(audioSource.clip == null || isPaused || moveArm) {
            return;
        }

        if(!audioSource.isPlaying && audioSource.time > 0f) {
            if(shuffling) {
                PlayRandomRecord();
            } else if(looping){
                PlayRecord(currentRecordIndex);
            } else {
                StopCurrentRecord();
            }
        }
    }

    private void PlayRandomRecord() {
        if(songs.Length == 0) {
            Debug.LogError("No songs available to play.");
            return;
        }

        int randomIndex = Random.Range(0, songs.Length);

        if(songs.Length > 1) {
            while(randomIndex == currentRecordIndex) {
                randomIndex = Random.Range(0, songs.Length);
            }
        }
        PlayRecord(randomIndex);
    }

    public void SkipToNextRecord() {
        if(songs.Length == 0) {
            return;
        }

        if(shuffling) {
            PlayRandomRecord();
            return;
        }

        int nextIndex = currentRecordIndex + 1;

        if(nextIndex >= songs.Length) {
            nextIndex = 0;
        }

        PlayRecord(nextIndex);
    }

    public void SkipToPreviousRecord() {
        if(songHistory.Count > 1 && historyIndex > 0) {
            historyIndex--;

            PlayRecord(songHistory[historyIndex], false);
            return;
        }

        if(!shuffling) {
            int previousIndex = currentRecordIndex - 1;

            if(previousIndex < 0) {
                previousIndex = songs.Length - 1;
            }

            PlayRecord(previousIndex);
        }
    }

    public void ToggleShuffle() {
        shuffling = !shuffling;
        audioSource.loop = false;
        updateShuffleIcon();
    }

    public void ToggleLoop() {
        looping = !looping;
        audioSource.loop = looping;
        updateLoopIcon();
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
        if(!spinningRecordImage.enabled) {
            return;
        }

        float targetSpinSpeed = audioSource.isPlaying ? maxSpinSpeed : 0f;

        currentSpinSpeed = Mathf.Lerp(currentSpinSpeed, targetSpinSpeed, (shouldSpin ? spinAcceleration : spinDeceleration) * Time.deltaTime);

        spinningRecordImage.transform.Rotate(0f, 0f, currentSpinSpeed * Time.deltaTime);
    }

    private void SeekingRecordSpinEffect() {
        if(!isDraggingSlider || !spinningRecordImage.enabled) {
            return;
        }

        float sliderDelta = ProgressBar.value - lastSliderValue;

        float seekSpin = sliderDelta * seekSpinMultiplier;

        spinningRecordImage.transform.Rotate(0f, 0f, seekSpin * Time.deltaTime);

        lastSliderValue = ProgressBar.value;
    }

    private void PulseRecord() {
        if(audioSource.clip == null || !audioSource.isPlaying) {
            spinningRecordImage.transform.localScale = Vector3.Lerp(
                spinningRecordImage.transform.localScale,
                recordBaseScale,
                pulseSmoothness * Time.deltaTime
            );

            return;
        }

        audioSource.GetOutputData(audioSamples, 0);

        float sum = 0f;

        for(int i = 0; i < audioSamples.Length; i++) {
            sum += Mathf.Abs(audioSamples[i]);
        }

        float average = sum / audioSamples.Length;

        float targetScale = 1f + average * pulseIntensity;

        spinningRecordImage.transform.localScale = Vector3.Lerp(
            spinningRecordImage.transform.localScale,
            recordBaseScale * targetScale,
            pulseSmoothness * Time.deltaTime
        );
    }

    private void RotateArm() {
        float currentZ = recordArmImage.transform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, currentArmTargetAngle, armRotationSpeed * Time.deltaTime);

        recordArmImage.transform.localEulerAngles = new Vector3(0, 0, newZ);

        if(Mathf.Abs(Mathf.DeltaAngle(newZ, currentArmTargetAngle)) < 0.1f) {
            moveArm = false;

            recordArmImage.transform.localEulerAngles = new Vector3(0, 0, currentArmTargetAngle);

            if(currentArmTargetAngle == armTargetAngle){
                if(!isPaused) {
                    if(audioSource.time > 0f)
                    {
                        audioSource.UnPause();
                        shouldSpin = true;
                    }
                    else
                    {
                        audioSource.Play();
                        shouldSpin = true;
                    }
                }

                if(EighthNoteParticles != null)
                {
                    EighthNoteParticles.Play();
                }

                if(QuarterNoteParticles != null)
                {
                    QuarterNoteParticles.Play();
                }
            }
        }
    }

    public void ResetArm() {
        recordArmImage.transform.localEulerAngles = new Vector3(0, 0, armInitialAngle);
        moveArm = false;
    }

    public void StartArmMovement(float targetAngle) {
        currentArmTargetAngle = targetAngle;
        moveArm = true;
    }

    private void AddToHistory(int recordIndex) {
        if(historyIndex < songHistory.Count - 1) {
            songHistory.RemoveRange(historyIndex + 1, songHistory.Count - historyIndex - 1);
        }
        songHistory.Add(recordIndex);
        historyIndex = songHistory.Count - 1;
    }

    public void PauseButton() {
        if(audioSource.clip == null) {
            return;
        }

        if(audioSource.isPlaying) {
            audioSource.Pause();
            shouldSpin = false;
            isPaused = true;
            updatePauseIcon();
            StartArmMovement(armInitialAngle);
            if(EighthNoteParticles != null && EighthNoteParticles.isPlaying) {
                EighthNoteParticles.Stop();
            }
            if(QuarterNoteParticles != null && QuarterNoteParticles.isPlaying) {
                QuarterNoteParticles.Stop();
            }
        } else {
            isPaused = false;
            updatePauseIcon();
            StartArmMovement(armTargetAngle);
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
        lastSliderValue = ProgressBar.value;
    }

    public void OnSliderPointerUp() {
        if(audioSource.clip != null) {
            audioSource.time = ProgressBar.value * audioSource.clip.length;
        }
        isDraggingSlider = false;
    }

    public void updateTimeElapsed() {
        if(audioSource.clip != null) {
            float currentTime;

            if(isDraggingSlider) {
                currentTime = ProgressBar.value * audioSource.clip.length;
            } else {
                currentTime = audioSource.time;
            }

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

    public void updateShuffleIcon() {
        if(shuffling) {
            shuffleButtonImage.sprite = shuffleActiveIcon;
        } else {
            shuffleButtonImage.sprite = shuffleIcon;
        }
    }

    public void updateLoopIcon() {
        if(looping) {
            loopButtonImage.sprite = loopActiveIcon;
        } else {
            loopButtonImage.sprite = loopIcon;
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
        shuffleButtonImage.sprite = shuffleIcon;
        loopButtonImage.sprite = loopIcon;
        shuffling = false;
        looping = false;
        audioSource.loop = false;
        songHistory.Clear();
        historyIndex = -1;
        currentRecordIndex = -1;
    }
}

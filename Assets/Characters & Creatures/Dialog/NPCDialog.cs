using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    public DialogLogicScript dialogLogicScript;
    public AudioSource dialogAudioSource;
    public AudioClip[] dialogAudioClips = {};
    public int speakerIndex;

    public string[] lines = {};

    public string currentSpeakerName;

    public void playDialogAudio() {
        dialogAudioSource.Play();
    }

    public void triggerDialog() {
        dialogLogicScript.beginDialog(this);
    }

    public void RandomizeDialogAudio() {
        if(dialogAudioClips.Length > 0) {
            int randomIndex = Random.Range(0, dialogAudioClips.Length);
            dialogAudioSource.clip = dialogAudioClips[randomIndex];
        }
    }
}

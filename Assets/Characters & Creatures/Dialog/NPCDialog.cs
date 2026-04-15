using UnityEngine;

public class NPCDialog : MonoBehaviour, IInteractable
{
    public DialogLogicScript dialogLogicScript;
    private InteractionIndication interactionIndicationScript;
    public AudioSource dialogAudioSource;
    public AudioClip[] dialogAudioClips = {};
    public int speakerIndex;

    public string[] lines = {};

    public string currentSpeakerName;

    private void Awake() {
        interactionIndicationScript = GetComponent<InteractionIndication>();
    }

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

    public void Interact() {
        if(interactionIndicationScript != null) {
            interactionIndicationScript.HideIndicator();
        }
        triggerDialog();
    }
}

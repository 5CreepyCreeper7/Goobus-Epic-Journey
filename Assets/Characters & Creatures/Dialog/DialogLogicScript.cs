using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogLogicScript : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMPro.TextMeshProUGUI dialogText;
    private NPCDialog currentNPC;
    public GameObject continueSign;

    public PlayerMovement playerMovementScript;
    public PauseManager pauseManager;

    public bool isDialogActive = false;
    private bool isTyping = false;

    private int currentSpeakerIndex;
    private string currentSpeakerName;
    
    /*  
        Index 0: Landyn
        Index 1: Duncan
        Index 2: Michael
        Index 3: Kenzie
        Index 4: Kyle
    */
    public Sprite[] speakerPortraits;
    public Image currentSpeakerPortrait;

    int dialogIndex = 0;
    private string[] dialog;

    [SerializeField] private float typingSpeed = 0.02f;
    private Coroutine typingCoroutine;

    public void beginDialog(NPCDialog NPC) {
        if(!playerMovementScript.isGrounded) {
            return;
        }

        if(pauseManager.isPaused) {
            return;
        }

        playerMovementScript.GetComponent<PlayerAnimationScript>().ForceIdle();
        Rigidbody2D rb = playerMovementScript.rb;
        rb.linearVelocity = Vector2.zero;

        currentNPC = NPC;
        currentSpeakerPortrait.sprite = speakerPortraits[NPC.speakerIndex];
        dialog = NPC.lines; 
        currentSpeakerName = NPC.currentSpeakerName;
        dialogIndex = 0;

        isDialogActive = true;

        playerMovementScript.enabled = false;
        dialogPanel.SetActive(true);

        showLine();
    }

    public void nextLine() {
        if (isTyping){
            StopCoroutine(typingCoroutine);
            dialogText.text = currentSpeakerName + ": " + dialog[dialogIndex];
            isTyping = false;
            continueSign.SetActive(true);
            return;
        }

        dialogIndex++;

        if (dialogIndex < dialog.Length) {
            showLine();
        }
        else{
            endDialog();
        }
    }

    public void endDialog() {
        dialogPanel.SetActive(false);
        playerMovementScript.enabled = true;
        isDialogActive = false;
    }

    public void showLine(){
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(typeLine(currentSpeakerName + ": " + dialog[dialogIndex]));

        if (currentNPC.dialogAudioSource != null)
        {
            currentNPC.RandomizeDialogAudio();
            currentNPC.dialogAudioSource.Play();
        }
    }   

    IEnumerator typeLine(string line) {
        isTyping = true;

        dialogText.text = "";

        continueSign.SetActive(false);

        foreach (char c in line){
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        continueSign.SetActive(true);

        isTyping = false;
    }
}

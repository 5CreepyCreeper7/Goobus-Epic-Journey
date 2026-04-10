using UnityEngine;
using UnityEngine.InputSystem;

public class BeginDialog : MonoBehaviour
{
    private GameObject currentNPC;
    public DialogLogicScript dialogLogic;
    private NPCDialog npcDialog;
    private PlayerMovement playerMovementScript;

    private bool playerInRange = false;

    private void Awake() {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnInteract(InputValue value) {
        if(dialogLogic.isDialogActive) {
            dialogLogic.nextLine();
        } else {
            playerInteraction();
        }
    }

    private void playerInteraction() {
        if(!playerInRange) {
            return;
        }

        NPCDialog npcDialog = currentNPC.GetComponent<NPCDialog>();

        if(npcDialog != null) {
            dialogLogic.beginDialog(npcDialog);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("NPC")) {
            currentNPC = collision.gameObject;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("NPC")) {
            currentNPC = null;
            playerInRange = false;
        }
    }
}

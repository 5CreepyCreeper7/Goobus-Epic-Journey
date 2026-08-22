using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private DialogLogicScript dialogLogic;

    private Collider2D currentTarget;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnInteract(InputValue value)
    {
        if (!value.isPressed)
            return;

        // Interact advances an already-active conversation.
        if (dialogLogic.isDialogActive)
        {
            dialogLogic.nextLine();
            return;
        }

        if (playerMovement.controlsLocked || currentTarget == null)
            return;

        InteractWithCurrentTarget();
    }

    private void InteractWithCurrentTarget()
    {
        // Check for berries and other general interactables.
        IInteractable interactable =
            currentTarget.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact();
            return;
        }

        // Otherwise, check for your existing NPC/sign dialog data.
        NPCDialog npcDialog =
            currentTarget.GetComponentInParent<NPCDialog>();

        if (npcDialog != null)
            dialogLogic.beginDialog(npcDialog);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<IInteractable>() != null ||
            collision.GetComponentInParent<NPCDialog>() != null)
        {
            currentTarget = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == currentTarget)
            currentTarget = null;
    }
}
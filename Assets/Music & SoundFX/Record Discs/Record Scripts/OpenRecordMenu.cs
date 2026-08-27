using UnityEngine;
using UnityEngine.InputSystem;

public class OpenRecordMenu : MonoBehaviour
{
    public GameObject GoobusUI;
    public GameObject recordMenuPanel;
    private PlayerMovement playerMovementScript;
    private RecordMenuLogic RecordMenuLogic;
    private RecordUIAnimation RecordUIAnimation;

    private bool playerInRange = false;
    private bool isMenuOpen = false;

    private void Awake() {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        RecordMenuLogic = recordMenuPanel.GetComponent<RecordMenuLogic>();
        RecordUIAnimation = recordMenuPanel.GetComponent<RecordUIAnimation>();
    }

    private void OnEnable()
    {
        if(RecordUIAnimation != null)
        {
            RecordUIAnimation.OnCloseComplete += HandleCloseComplete;
        }
    }

    private void OnDisable() {
        if (RecordUIAnimation != null) {
            RecordUIAnimation.OnCloseComplete -= HandleCloseComplete;
        }
    }

    private void OnInteract(InputValue value) {
        if(RecordMenuLogic == null) {
            return;
        }

        if(PauseManager.Instance != null && PauseManager.Instance.isPaused) {
            return;
        }

        if(recordMenuPanel.activeSelf) {
            CloseMenu();
        } else {
            TryToOpenMenu();
        }
    }

    private void TryToOpenMenu() {
        if(!playerInRange || isMenuOpen) {
            return;
        }

        if(RecordMenuLogic != null) {
            OpenMenu();
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("RecordPlayer")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("RecordPlayer")) {
            playerInRange = false;
        }
    }

    public void OpenMenu() {
        GoobusUI.SetActive(false);
        isMenuOpen = true;
        playerMovementScript.enabled = false;
        playerMovementScript.GetComponent<PlayerAnimationScript>().ForceIdle();
        Rigidbody2D rb = playerMovementScript.rb;
        rb.linearVelocity = Vector2.zero;

        RecordUIAnimation.OpenMenu();
    }

    public void CloseMenu() {
        RecordMenuLogic.ResetMenu();
        isMenuOpen = false;

        RecordUIAnimation.CloseMenu();
    }

    private void HandleCloseComplete()
    {
        GoobusUI.SetActive(true);
        playerMovementScript.enabled = true;
    }
}

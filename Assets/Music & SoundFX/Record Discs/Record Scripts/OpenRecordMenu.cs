using UnityEngine;
using UnityEngine.InputSystem;

public class OpenRecordMenu : MonoBehaviour
{
    public GameObject GoobusUI;
    public GameObject recordMenuPanel;
    private PlayerMovement playerMovementScript;
    private RecordMenuLogic RecordMenuLogic;

    private bool playerInRange = false;
    private bool isMenuOpen = false;

    private void Awake() {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        RecordMenuLogic = recordMenuPanel.GetComponent<RecordMenuLogic>();
    }

    private void OnInteract(InputValue value) {
        if(RecordMenuLogic == null) {
            return;
        }

        if(recordMenuPanel.activeSelf) {
            CloseMenu(recordMenuPanel);
        } else {
            TryToOpenMenu();
        }
    }

    private void TryToOpenMenu() {
        if(!playerInRange || isMenuOpen) {
            return;
        }

        if(RecordMenuLogic != null) {
            OpenMenu(recordMenuPanel);
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

    public void OpenMenu(GameObject recordMenuPanel) {
        recordMenuPanel.SetActive(true);
        GoobusUI.SetActive(false);
        isMenuOpen = true;
        playerMovementScript.enabled = false;
        playerMovementScript.GetComponent<PlayerAnimationScript>().ForceIdle();
        Rigidbody2D rb = playerMovementScript.rb;
        rb.linearVelocity = Vector2.zero;
    }

    public void CloseMenu(GameObject recordMenuPanel) {
        RecordMenuLogic.ResetMenu();
        recordMenuPanel.SetActive(false);
        GoobusUI.SetActive(true);
        playerMovementScript.enabled = true;
        isMenuOpen = false;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    private bool isGrounded;

    private Vector2 direction;

    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;
    public InputActionReference crouchAction;

    private SpriteRenderer spriteRenderer;
    private PlayerAnimationScript playerAnimationScript;

    void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerAnimationScript = GetComponent<PlayerAnimationScript>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        crouchAction.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        direction = moveAction.action.ReadValue<Vector2>();
        
        flipSprite();
        handleRunningAnimation();
        handleJumping();       
        handleSprinting(); 
        handleCrouching();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    private void flipSprite()
    {
        if (direction.x > 0)
        {
            this.spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            this.spriteRenderer.flipX = true;
        }
    }

    private void handleRunningAnimation()
    {
        if (direction.x == 0)
        {
            this.playerAnimationScript.IdleAnimation();
        }
        else
        {
            this.playerAnimationScript.RunningAnimation();
        }
    }

    private void handleJumping()
    {
        if (jumpAction.action.WasPressedThisFrame() && isGrounded)
        {
            //playerAnimationScript.JumpingAnimation();
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void handleSprinting()
    {
        if (sprintAction.action.IsPressed())
        {
            this.playerAnimationScript.SprintingAnimation();
            moveSpeed = 10f; 
        }
        else
        {
            moveSpeed = 5f; 
        }
    }

    private void handleCrouching() {
        if (crouchAction.action.IsPressed())
        {
            this.playerAnimationScript.CrouchingAnimation();
            moveSpeed = 0f;
        }
        else
        {
            moveSpeed = 5f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void handlePause()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Time.timeScale = 0f; 
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    // Movement variables
    public float currentMoveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public float walljumpForce;
    public float wallSlideSpeed;
    private float walkTimer = 0f;
    private float walkingInterval = 0.5f;
    private float wallJumpTimer = 0.25f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    // State bool variables
    public bool isGrounded;
    private bool wasGrounded;
    public bool isCrouching;
    private bool canDash = true;
    private bool isDashing;
    private bool canWallJump;
    private bool OnWallLeft;
    private bool OnWallRight;
    private bool isWallSliding;
    //private bool justWallJumped = false;
    
    // Ground check variables
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    // Wall check variables
    [SerializeField] private Transform wallLeftCheckPoint;
    [SerializeField] private Transform wallRightCheckPoint;
    [SerializeField] private float wallCheckRadius = 0.1f;

    // Dash variables
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float upwardDashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private float cooldownTimer;
    private float dashTimer;

    private Vector2 dashDirection;

    [SerializeField] private Material cannotDashIndicatorMaterial;
    [SerializeField] private Material defaultMaterial;

    public float originalGravityScale;

    private Vector2 direction;

    // Input actions
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;
    public InputActionReference crouchAction;
    public InputActionReference dashAction;
    public InputActionReference lookingUpAction;

    // Component references
    private SpriteRenderer spriteRenderer;
    private PlayerAnimationScript playerAnimationScript;
    private PlayerSoundFX playerSoundFX;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerAnimationScript = GetComponent<PlayerAnimationScript>();
        this.playerSoundFX = GetComponent<PlayerSoundFX>();
        this.boxCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        crouchAction.action.Enable();
        dashAction.action.Enable();
        lookingUpAction.action.Enable();

        originalGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        OnWallLeft = Physics2D.OverlapCircle(wallLeftCheckPoint.position, wallCheckRadius, groundLayer);
        OnWallRight = Physics2D.OverlapCircle(wallRightCheckPoint.position, wallCheckRadius, groundLayer);
        canWallJump = OnWallLeft || OnWallRight;

        direction = moveAction.action.ReadValue<Vector2>();

        if(!wasGrounded && isGrounded && rb.linearVelocity.y <= 0) {
            playerSoundFX.playLandingSound();
        }

        playWalkSound(direction);
        
        flipSprite();

        if(canWallJump && !isGrounded) {
            handleWallJumping();
        } else {
            handleJumping(); 
        }
        
        updateState();   
        handleMovementSpeed(); 
        wallSlide();

        if(wallJumpTimer > 0f) {
            wallJumpTimer -= Time.deltaTime;
        }

        if(!canDash) {
            spriteRenderer.material = cannotDashIndicatorMaterial;
            cooldownTimer -= Time.deltaTime;

            if(cooldownTimer <= 0f && isGrounded) {
                spriteRenderer.material = defaultMaterial;
                canDash = true;
            }
        }

        if(dashAction.action.WasPressedThisFrame() && canDash && !isCrouching && !isDashing) {
            StartDash();
        }

        playerAnimationScript.updateAnimations(direction.x, isGrounded, isCrouching, isDashing, UnityEngine.Random.Range(0, 10000));
    }

    private void FixedUpdate()
    {
        if(isDashing) {
            if(dashDirection == Vector2.up) {
                rb.linearVelocity = dashDirection * upwardDashForce;
            } else if(dashDirection == Vector2.down) {
                rb.linearVelocity = dashDirection * upwardDashForce;
            } else {
                rb.linearVelocity = dashDirection * dashForce;
            }
         
            dashTimer -= Time.fixedDeltaTime;

            if(dashTimer <= 0f) {
                isDashing = false;
                rb.gravityScale = originalGravityScale;
                cooldownTimer = dashCooldown;
            }

            return;
        }

        HandleMovement();
        
        if(!isDashing && !isWallSliding) {
            ControlledJump();
        }
    }

    private void flipSprite() {
        if(!isCrouching) {
                if (direction.x > 0)
            {
                this.spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                this.spriteRenderer.flipX = true;
            }
        }
    }
       

    private void updateState()
    {
        isCrouching = crouchAction.action.IsPressed();
    }

    private void HandleMovement()
    {
        if(wallJumpTimer > 0f) {
            return;
        }

        float targetVelocityX = direction.x;
        rb.linearVelocity = new Vector2(direction.x * currentMoveSpeed, rb.linearVelocity.y);
    }

    private void handleMovementSpeed() {
        if(crouchAction.action.IsPressed() && isGrounded) {
            currentMoveSpeed = 0f;
        }
        else if (sprintAction.action.IsPressed())
        {
            currentMoveSpeed = sprintSpeed;
            walkingInterval = 0.2f;
        }
        else
        {
            currentMoveSpeed = walkSpeed; 
            walkingInterval = 0.5f;
        }
    }

    private void handleJumping()
    {
        if (jumpAction.action.WasPressedThisFrame() && isGrounded && !isCrouching && !isDashing)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerSoundFX.playJumpSound();
        }

        /*if(rb.linearVelocity.y < 0) {
            playerSoundFX.playFallingSound();
        } else {
            playerSoundFX.stopFallingSound();
        }*/
    }

    private void ControlledJump() {
        if(rb.linearVelocity.y < 0) {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } else if(rb.linearVelocity.y > 0 && !jumpAction.action.IsPressed()) {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void wallSlide() {
        if((OnWallLeft && direction.x < 0 || OnWallRight && direction.x > 0) && !isGrounded) 
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        } else {
            isWallSliding = false;
        }
    }

    private void wallJump(Vector2 wallJumpDirection) {
        if(wallJumpTimer > 0f) {
            return;
        }
        float horizontalForce = wallJumpDirection.x * walljumpForce;
        float verticalForce = jumpForce;

        rb.linearVelocity = new Vector2(horizontalForce, verticalForce);
        playerSoundFX.playJumpSound();
        wallJumpTimer = 0.25f;
    }

    private void handleWallJumping() {
        if (jumpAction.action.WasPressedThisFrame() && canWallJump && !isCrouching && !isGrounded)
        {
            if(OnWallLeft) {
                wallJump(new Vector2(1f, 0f));
            } else if(OnWallRight) {
                wallJump(new Vector2(-1f, 0f));
            }
        }
    }

    Vector2 GetDashDirection() {
        if(lookingUpAction.action.IsPressed()) {
            return Vector2.up;
        }

        if(crouchAction.action.IsPressed()) {
            return Vector2.down;
        }

        float dashX = direction.x;

        if(dashX == 0) {
            dashX = spriteRenderer.flipX ? -1 : 1;
        }

        return new Vector2(dashX, 0);
    }

    void StartDash() {
        canDash = false;
        isDashing = true;
        dashDirection = GetDashDirection().normalized;
        rb.gravityScale = 0f;

        dashTimer = dashDuration;
        //ghostTimer = 0f;
    }

    public bool getIsDashing() {
        return isDashing;
    }

    private void playWalkSound(Vector2 direction) {
        if(direction.x != 0 && isGrounded && !isCrouching) {
            walkTimer -= Time.deltaTime;

            if(walkTimer <= 0f) {
                playerSoundFX.playWalkSound();
                walkTimer = walkingInterval;
            }
        } else {
            walkTimer = 0f;
        }
    }
}

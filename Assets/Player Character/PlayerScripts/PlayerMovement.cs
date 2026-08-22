using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    // Movement variables
    [Header("Movement Settings")]
    public float currentMoveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public float walljumpForce;
    public float wallSlideSpeed;
    private float walkTimer = 0f;
    private float walkingInterval = 0.5f;
    private float wallJumpTimer = 0.25f;
    [SerializeField] private float wallJumpTimerDuration = 0.25f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    // State bool variables
    [Header("State Variables")]
    public bool isGrounded;
    private bool wasGrounded;
    public bool isCrouching;
    public bool canDash = true;
    private bool dashedInAir;
    private bool isDashing;
    private bool canWallJump;
    private bool OnWallLeft;
    private bool OnWallRight;
    private bool isWallSliding;
    public bool isLaunched;
    public bool controlsLocked;
    private bool jumpQueued;
    
    // Ground check variables
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    // Wall check variables
    [Header("Wall Check Settings")]
    [SerializeField] private Transform wallLeftCheckPoint;
    [SerializeField] private Transform wallRightCheckPoint;
    [SerializeField] private float wallCheckRadius = 0.1f;

    // Dash variables
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float upwardDashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float VerticalDashClamp = 0.4f;

    private float cooldownTimer;
    private float dashTimer;

    public Vector2 dashDirection;

    [SerializeField] private Material cannotDashIndicatorMaterial;
    [SerializeField] private Material defaultMaterial;

    public float originalGravityScale;

    private Vector2 direction;

    // Input actions
    [Header("Input Actions")]
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
        if(controlsLocked)
        {
            float  automaticMovement = 0f;

            if(Mathf.Abs(rb.linearVelocity.x) > 0.01f)
            {
                automaticMovement = Mathf.Sign(rb.linearVelocity.x);
            }

            playerAnimationScript.updateAnimations(direction.x, isGrounded, false, false, UnityEngine.Random.Range(0, 10000));
            UpdateAutomaticFacing(automaticMovement);

            return;
        }

        direction = moveAction.action.ReadValue<Vector2>();

        if(!wasGrounded && isGrounded && rb.linearVelocity.y <= 0) {
            playerSoundFX.playLandingSound();
        }

        playWalkSound(direction);
        flipSprite();

        if(jumpAction.action.WasPressedThisFrame())
        {
            jumpQueued = true;
        }
        
        updateState();   
        handleMovementSpeed(); 

        if(wallJumpTimer > 0f) {
            wallJumpTimer -= Time.deltaTime;
        }

        HandleDashing();

        playerAnimationScript.updateAnimations(direction.x, isGrounded, isCrouching, isDashing, UnityEngine.Random.Range(0, 10000));
    }

    private void FixedUpdate()
    {
        if(controlsLocked)
        {
            return;
        }
        
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        OnWallLeft = Physics2D.OverlapCircle(wallLeftCheckPoint.position, wallCheckRadius, groundLayer);
        OnWallRight = Physics2D.OverlapCircle(wallRightCheckPoint.position, wallCheckRadius, groundLayer);
        canWallJump = OnWallLeft || OnWallRight;

        if(isDashing) {
            float currentDashForce;

            if(Mathf.Abs(dashDirection.y) > 0.1f) {
                currentDashForce = upwardDashForce;
            } else {
                currentDashForce = dashForce;
            }

            rb.linearVelocity = dashDirection * currentDashForce;
         
            dashTimer -= Time.fixedDeltaTime;

            if(dashTimer <= 0f) {
                isDashing = false;
                rb.gravityScale = originalGravityScale;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * VerticalDashClamp);
            }

            jumpQueued = false;
            return;
        }

        if(isLaunched) {
            jumpQueued = false;
            return;
        }

        if(canWallJump && !isGrounded) {
            handleWallJumping();
        } else {
            handleJumping(); 
        }

        jumpQueued = false;

        HandleMovement();
        wallSlide();
        
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

    private void UpdateAutomaticFacing(float HorizontalMovement)
    {
        if(HorizontalMovement > 0f)
        {
            spriteRenderer.flipX = false;
        } else if (HorizontalMovement < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }
       
    private void updateState()
    {
        if(!isGrounded) {
            isCrouching = false;
            return;
        }
        isCrouching = crouchAction.action.IsPressed();
    }

    private void HandleDashing() {
        if(!canDash) {
            setPlayerCannotDashMaterial();

            if(!dashedInAir && !isGrounded) {
                dashedInAir = true;
            }

            if(!dashedInAir) {
                cooldownTimer -= Time.deltaTime;

                if(cooldownTimer <= 0f) {
                    setPlayerDefaultMaterial();
                    canDash = true;
                }
            } else {
                if(!wasGrounded && isGrounded) {
                    setPlayerDefaultMaterial();
                    canDash = true;
                    dashedInAir = false;
                }
            }
        }

        if(dashAction.action.WasPressedThisFrame() && canDash && !isCrouching && !isDashing) {
            StartDash();
        }
    }

    private void HandleMovement()
    {
        if(wallJumpTimer > 0f || isLaunched) {
            return;
        }

        rb.linearVelocity = new Vector2(direction.x * currentMoveSpeed, rb.linearVelocity.y);
    }

    public void Sprung(float duration) {
        isLaunched = true;
        Invoke(nameof(endSprung), duration);
    }

    public void endSprung() {
        isLaunched = false;
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
        if (jumpQueued && isGrounded && !isCrouching && !isDashing)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerSoundFX.playJumpSound();
        }
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
        wallJumpTimer = wallJumpTimerDuration;
    }

    private void handleWallJumping() {
        if (jumpQueued && canWallJump && !isCrouching && !isGrounded)
        {
            if(OnWallLeft) {
                wallJump(new Vector2(1f, 0f));
            } else if(OnWallRight) {
                wallJump(new Vector2(-1f, 0f));
            }
        }
    }

    Vector2 GetDashDirection() {
        float dashX = direction.x;
        float dashY = 0f;

        if(lookingUpAction.action.IsPressed()) {
            dashY = 1f;
        } else if(crouchAction.action.IsPressed()) {
            dashY = -1f;
        }

        if(dashX == 0f && dashY == 0f) {
            dashX = spriteRenderer.flipX ? -1f : 1f;
        }

        if(dashX == 0f && dashY != 0f) {
            return new Vector2(0f, dashY);
        }

        return new Vector2(dashX, dashY).normalized;
    }

    void StartDash() {
        playerSoundFX.playDashSound();
        canDash = false;
        isDashing = true;
        
        dashedInAir = !isGrounded;

        dashDirection = GetDashDirection();
        rb.gravityScale = 0f;

        dashTimer = dashDuration;

        CameraVariance.instance.ShakeCamera(dashDirection, 0.1f);

        if(!dashedInAir) {
            cooldownTimer = dashCooldown;
        }
    }

    public void ReboundFromDash(Vector2 dashDirection, float reboundForce, float controlLockDuration = 0.15f) {
        isDashing = false;
        dashTimer = 0f;
        rb.gravityScale = originalGravityScale;

        isLaunched = true;
        CancelInvoke(nameof(endSprung));

        rb.linearVelocity = -dashDirection.normalized * reboundForce;

        Invoke(nameof(endSprung), controlLockDuration);
    }

    public bool getIsDashing() {
        return isDashing;
    }

    public void setPlayerDefaultMaterial() {
        spriteRenderer.material = defaultMaterial;
    }

    public void setPlayerCannotDashMaterial() {
        spriteRenderer.material = cannotDashIndicatorMaterial;
    }

    public void resetDash() {
        canDash = true;
        setPlayerDefaultMaterial();
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

    public void SetControlsLocked(bool locked) {
        controlsLocked = locked;

        if(locked) {
            rb.linearVelocity = Vector2.zero;
        }
    }
}

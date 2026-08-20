using UnityEngine;
using System.Collections;

public class EggBossBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Arena Positions")]
    [SerializeField] private Transform rightSide;
    [SerializeField] private Transform leftSide;

    [Header("Prefabs")]
    [SerializeField] private GameObject walnutPrefab;
    [SerializeField] private GameObject acornPrefab;
    [SerializeField] private GameObject slamAttackNutPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform walnutSpawnPointL;
    [SerializeField] private Transform acornSpawnPointL;
    [SerializeField] private Transform walnutSpawnPointR;
    [SerializeField] private Transform acornSpawnPointR;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldownDuration = 2f;
    [SerializeField] private float chargeSpeed = 8f;
    [SerializeField] private float stoppingDistance = 0.1f;

    [Header("Nut Throwing Attack Settings")]
    [SerializeField] private int maxNutThrow = 5;
    [SerializeField] private float nutThrowInterval = 0.5f;

    [Header("Slam Attack Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float slamJumpForce = 10f;
    [SerializeField] private float slamFallingSpeed = 15f;
    [SerializeField] private float slamImpactDuration = 0.2f;

    [Header("Slam Attack Nut Settings")]
    [SerializeField] private BoxCollider2D SlamAttackNutSpawner;
    [SerializeField] private int maxNutCount = 10;
    [SerializeField] private float nutSpawnInterval = 0.15f;
    [SerializeField] private float nutFallSpeed = 5f;

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private string EggLocation = "Right";
    private float attackCooldown;
    private bool isCharging = false;
    private bool isSlamming = false;
    private bool isThrowingNuts = false;
    private Vector2 chargeTargetPosition;

    //Pretty Stuff
    [SerializeField] private ParticleSystem SlamParticles;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        attackCooldown = attackCooldownDuration;
    }

    void FixedUpdate() {
        if(isCharging) {
            UpdateChargeMovement();
        }
    }

    void Update()
    {
        if(isCharging || isSlamming || isThrowingNuts) {
            return;
        }

        attackCooldown -= Time.deltaTime;

        if(attackCooldown <= 0) {
            int attackChoice = Random.Range(0, 3);
            //int attackChoice = 1;

            switch (attackChoice) {
                case 0:
                    StartSlamAttack();
                    break;
                case 1:
                    StartCoroutine(NutThrowingAttack());
                    break;
                case 2:
                    StartChargingAttack();
                    break;
            }

            attackCooldown = attackCooldownDuration;
        }
    }

    //Slam Attack Function; make nuts fall from ceiling
    private void StartSlamAttack() {
        if(!isSlamming) {
            StartCoroutine(SlamAttackRoutine());
        }
    }

    private IEnumerator SlamAttackRoutine() {
        isSlamming = true;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, slamJumpForce);

        yield return new WaitUntil(() => rb.linearVelocity.y <= 0);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -slamFallingSpeed);

        yield return new WaitUntil(() => IsGrounded());

        rb.linearVelocity = Vector2.zero;

        SlamImpact();

        yield return new WaitForSeconds(slamImpactDuration);

        yield return StartCoroutine(SpawnSlamAttackNuts());

        isSlamming = false;
        attackCooldown = attackCooldownDuration;
    }

    private void SlamImpact() {
        // Play slam impact animation and sound effect here
        SlamParticles.Play();
    }

    private IEnumerator SpawnSlamAttackNuts() {
        int nutCount = Random.Range(3, maxNutCount + 1);

        for(int i = 0; i < nutCount; i++) {
            Bounds spawnBounds = SlamAttackNutSpawner.bounds;

            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                spawnBounds.center.y
            );

            GameObject nut = Instantiate(slamAttackNutPrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D nutRb = nut.GetComponent<Rigidbody2D>();

            if(nutRb != null) {
                nutRb.linearVelocity = Vector2.down * nutFallSpeed;
            }

            yield return new WaitForSeconds(nutSpawnInterval);
        }
    }

    //Nut Throwing Attack; have egg throw nuts at player
    private IEnumerator NutThrowingAttack() {
        isThrowingNuts = true;
        int nutCount = Random.Range(3, maxNutThrow + 1);

        for(int i = 0; i < nutCount; i++) {
            int nutType = Random.Range(0, 2); 

            if(nutType == 0) {
                if(EggLocation == "Right") {
                    Instantiate(walnutPrefab, walnutSpawnPointL.position, Quaternion.identity);
                } else {
                    Instantiate(walnutPrefab, walnutSpawnPointR.position, Quaternion.identity);
                }
            } else {
                if(EggLocation == "Right") {
                    Instantiate(acornPrefab, acornSpawnPointL.position, Quaternion.identity);
                } else {
                    Instantiate(acornPrefab, acornSpawnPointR.position, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(nutThrowInterval);
        }

        isThrowingNuts = false;
        attackCooldown = attackCooldownDuration;
    }

    private void StartChargingAttack() {
        isCharging = true;
        
        if(EggLocation == "Right") {
            chargeTargetPosition = leftSide.position;
            rb.linearVelocity = new Vector2(-chargeSpeed, rb.linearVelocity.y);
        } else {
            chargeTargetPosition = rightSide.position;
            rb.linearVelocity = new Vector2(chargeSpeed, rb.linearVelocity.y);
        }
    }

    private void UpdateChargeMovement() {
        bool reachedTarget;

        if(EggLocation == "Right") {
            reachedTarget = transform.position.x <= chargeTargetPosition.x + stoppingDistance;
        } else {
            reachedTarget = transform.position.x >= chargeTargetPosition.x - stoppingDistance;
        }

        if(reachedTarget) {
            FinishChargingAttack();
        }
    }

    private void FinishChargingAttack() {
        rb.linearVelocity = Vector2.zero;

        transform.position = new Vector2(chargeTargetPosition.x, transform.position.y);

        EggLocation = (EggLocation == "Right") ? "Left" : "Right";
        isCharging = false;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        attackCooldown = attackCooldownDuration;

        Debug.Log("Egg Boss finished charging attack and is now at " + EggLocation + " side.");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHurt : MonoBehaviour
{
    private float iFrameDuration = 0.35f; 
    private bool isInvincible = false;

    private Rigidbody2D rb;

    private PlayerDied playerDiedScript;
    private HealthScript healthScript;
    private DamageFlash damageFlash;
    private PlayerUI playerUI;
    private PlayerMovement playerMovement;
    private PlayerSoundFX playerSounds;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        playerDiedScript = GetComponent<PlayerDied>();
        healthScript = GetComponent<HealthScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageFlash = GetComponent<DamageFlash>();
        playerUI = FindFirstObjectByType<PlayerUI>();
        rb = GetComponent<Rigidbody2D>();
        playerSounds = GetComponent<PlayerSoundFX>();
    }

    private void Update() {
        UpdateIFrameTimer();
    }

    public void TakeDamage(int damageAmount) {
        if(isInvincible) {
            return;
        }

        playerSounds.playHurtSound();
        BeginIFrames();
        damageFlash.PlayerFlash();

        healthScript.currentHealth -= damageAmount;
        playerUI.RefreshHearts();

        if(healthScript.currentHealth <= 0) {
            playerDiedScript.Died();
        }
    }

    private void BeginIFrames() {
        isInvincible = true;
    }

    private void UpdateIFrameTimer() {
        if(isInvincible) {
            if(iFrameDuration > 0) {
                iFrameDuration -= Time.deltaTime;
            } else {
                ResetIFrames();
            }
        }
    }

    private void ResetIFrames() {
        isInvincible = false;
        iFrameDuration = 2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")) {
            int damageDealt = collision.gameObject.GetComponent<EnemyStats>().damageToPlayer;

            //if (isInvincible || isDashing)
               // return;

            TakeDamage(damageDealt);
        }
    }
}

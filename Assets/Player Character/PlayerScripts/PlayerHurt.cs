using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHurt : MonoBehaviour
{
    private float iFrameDuration = 0.35f; 
    private bool isInvincible = false;

    private PlayerDied playerDiedScript;
    private HealthScript healthScript;
    private DamageFlash damageFlash;
    private PlayerUI playerUI;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        playerDiedScript = GetComponent<PlayerDied>();
        healthScript = GetComponent<HealthScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageFlash = GetComponent<DamageFlash>();
        playerUI = FindFirstObjectByType<PlayerUI>();
    }

    private void Update() {
        UpdateIFrameTimer();
    }

    public void TakeDamage(int damageAmount) {
        if(isInvincible) {
            return;
        }

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
}

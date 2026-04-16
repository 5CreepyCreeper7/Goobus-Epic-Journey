using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDied : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerSoundFX playerSoundFX;
    private HealthScript healthScript;

    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerSoundFX = GetComponent<PlayerSoundFX>();
        healthScript = GetComponent<HealthScript>();
    }

    public void Died() {
        if(isDead) return;

        isDead = true;

        playerMovement.enabled = false;
        anim.SetTrigger("Died");
        anim.SetBool("IsDead", true);
        this.playerSoundFX.playDeathSound();

        Rigidbody2D rb = playerMovement.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        Invoke("Respawn", 0.5f);
    }

    public void Respawn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isDead = false;
        healthScript.currentHealth = healthScript.maxHealth;
        anim.SetBool("IsDead", false);
        playerMovement.enabled = true;
        Rigidbody2D rb = playerMovement.GetComponent<Rigidbody2D>();
        rb.gravityScale = playerMovement.originalGravityScale;
    }
}

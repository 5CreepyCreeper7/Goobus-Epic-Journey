using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHurt : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerSoundFX playerSoundFX;

    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerSoundFX = GetComponent<PlayerSoundFX>();
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

    void Respawn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isDead = false;
        anim.SetBool("IsDead", false);
        playerMovement.enabled = true;
        Rigidbody2D rb = playerMovement.GetComponent<Rigidbody2D>();
        rb.gravityScale = playerMovement.originalGravityScale;
    }
}

using UnityEngine;

public class SpringScript : MonoBehaviour
{
    public float springForce = 20f;

    public float SpringDuration = 0.25f;

    public AudioSource bounceSFX;

    public Vector2 springDirection = Vector2.up;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();

            if(playerMovement == null) {
                return;
            }

            if(playerMovement.canDash == false) {
                playerMovement.canDash = true;
                playerMovement.setPlayerDefaultMaterial();
            }

            playerMovement.Sprung(SpringDuration);

            if(rb != null) {
                Vector2 direction = springDirection.normalized;
                
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(direction * springForce, ForceMode2D.Impulse);
                bounceSFX.Play();
            }
        }
    }
}

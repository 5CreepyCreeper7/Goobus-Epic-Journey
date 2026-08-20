using UnityEngine;

public class FallingNutBehavior : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float maximumLifetime = 6f;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        Destroy(gameObject, maximumLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerHurt>()?.TakeDamage(damage);

            //Play breaking animation and sound effect
            //Destroy nut after animation is finished

            GameObject.Destroy(this.gameObject);
        }
    }
}

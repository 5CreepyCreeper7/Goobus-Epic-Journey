using UnityEngine;

public class BottomNutBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float maximumLifetime = 6f;

    private Rigidbody2D rb;
    private float rollDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null) {
            Debug.LogError("Player not found in the scene.");
            return;
        }

        rollDirection = Mathf.Sign(player.transform.position.x - rb.position.x);

        if(rollDirection == 0) {
            rollDirection = Random.value < 0.5f ? -1f : 1f;
        }

        Destroy(gameObject, maximumLifetime);
    }

    private void FixedUpdate() {
        RollTowardsPlayer();
    }

    private void RollTowardsPlayer() {
        rb.linearVelocity = new Vector2(rollDirection * speed, rb.linearVelocity.y);
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

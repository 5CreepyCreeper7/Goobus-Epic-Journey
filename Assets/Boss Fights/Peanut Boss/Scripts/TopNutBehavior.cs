using UnityEngine;

public class TopNutBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float MinTravelTime = 0.4f;
    [SerializeField] private float maximumLifetime = 6f;
    [SerializeField] private float arcAmount = 0.5f;
    
    private Rigidbody2D rb;
    private Vector2 TargetPosition;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null) {
            Debug.LogError("Player not found in the scene.");
            return;
        }

        TargetPosition = player.transform.position;

        FlyTowardsPlayer();

        Destroy(gameObject, maximumLifetime);
    }

    private void FlyTowardsPlayer() {
        Vector2 startPosition = rb.position;
        Vector2 targetPosition = TargetPosition;
        Vector2 Displacement = targetPosition - startPosition;

        float travelTime = Mathf.Abs(Displacement.x / speed);
        travelTime = Mathf.Max(travelTime, MinTravelTime) * arcAmount;

        float gravity = Physics2D.gravity.y * rb.gravityScale;

        float horizontalVelocity = Displacement.x / travelTime;

        float verticalVelocity = (Displacement.y - 0.5f * gravity * travelTime * travelTime) / travelTime;

        rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
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

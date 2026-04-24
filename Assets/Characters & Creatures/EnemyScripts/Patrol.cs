using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform GroundCheck;
    public Transform WallCheck;

    public float GroundCheckDistance = 1f;
    public float WallCheckDistance = 0.2f;
    public float Speed = 2f;

    public LayerMask GroundLayer;

    //left = -1, right = 1
    private int direction = -1;

    void FixedUpdate() {
        rb.linearVelocity = new Vector2(direction * Speed, rb.linearVelocity.y);

        bool isGroundAhead = Physics2D.Raycast(GroundCheck.position, Vector2.down, GroundCheckDistance, GroundLayer);

        bool isWallAhead = Physics2D.Raycast(WallCheck.position, Vector2.right * direction, WallCheckDistance, GroundLayer);

        if(!isGroundAhead || isWallAhead) {
            Flip();
        }
    }

    void Flip() {
        direction *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

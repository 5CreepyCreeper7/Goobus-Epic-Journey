using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 targetPosition;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
        }
        else if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
        }

        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }
}

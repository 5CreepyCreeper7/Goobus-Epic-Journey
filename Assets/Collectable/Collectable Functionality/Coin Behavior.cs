using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    private Vector3 startPos;

    public float bobbingSpeed = 2f;
    public float bobbingAmount = 0.1f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos +
            new Vector3(
                0,
                Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount,
                0
            );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Here you can add code to increase the player's coin count or trigger any other effect you want when the coin is collected.
            Destroy(gameObject);
        }
    }
}


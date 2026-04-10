using UnityEngine;

public class DashGhost : MonoBehaviour
{
    public float lifetime = 0.5f;

    private float timer = 0f;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        float alpha = Mathf.Lerp(0.5f, 0f, 1 - timer/lifetime);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if(timer <= 0f) {
            Destroy(gameObject);
        }
    }
}

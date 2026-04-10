using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Flash() {
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine() {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }
}

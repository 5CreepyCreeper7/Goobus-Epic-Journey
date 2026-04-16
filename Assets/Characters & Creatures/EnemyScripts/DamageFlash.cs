using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public float flashInterval = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void EnemyFlash() {
        StartCoroutine(EnemyFlashCoroutine());
    }

    public void PlayerFlash() {
        StartCoroutine(PlayerIFramesFlash());
    }

    IEnumerator EnemyFlashCoroutine() {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }

    IEnumerator PlayerIFramesFlash() {
        float timer = 0f;

        while(timer < flashDuration) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
         
            yield return new WaitForSeconds(flashInterval);

            timer += flashInterval;
        }

        spriteRenderer.enabled = true;
    }
}

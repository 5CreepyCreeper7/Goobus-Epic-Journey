using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour
{
    public EnemyStats enemyStats;

    public float deathAnimationDuration = 1f;

    private bool dying;

    void Update() {
        if(enemyStats.enemyHealth <= 0 && !dying) {
            StartCoroutine(deathAnimation());
            dying = true;
        }
    }

    IEnumerator deathAnimation() {
        Vector2 originalScale = transform.localScale;
        float timer = 0f;

        while(timer < deathAnimationDuration) {
            float scale = Mathf.Lerp(1f, 0f, timer/deathAnimationDuration);
            transform.localScale = originalScale * scale;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

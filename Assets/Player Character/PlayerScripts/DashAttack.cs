using UnityEngine;
using System.Collections;

public class DashAttack : MonoBehaviour
{
    public float timer = 1f;
    public float TargetTimeScale = 0f;

    public PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Enemy") && playerMovement.getIsDashing()) {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if(enemyStats != null) {
                enemyStats.enemyHealth -= 1;
            }
            
            DamageFlash damageFlash = collision.GetComponent<DamageFlash>();

            if (damageFlash != null) {
                damageFlash.EnemyFlash();
            }

            //StartCoroutine(ImpactPause());

            Debug.Log("Enemy hit by dash attack!" + " Enemy health: " + enemyStats.enemyHealth);
        }
    }

    IEnumerator ImpactPause() {
        Time.timeScale = TargetTimeScale;
        yield return new WaitForSecondsRealtime(timer);
        Time.timeScale = 1f;
    }
}

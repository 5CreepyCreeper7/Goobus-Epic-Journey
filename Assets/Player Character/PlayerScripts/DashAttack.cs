using UnityEngine;

public class DashAttack : MonoBehaviour
{
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
            
            Debug.Log("Enemy hit by dash attack!" + " Enemy health: " + enemyStats.enemyHealth);
        }
    }
}

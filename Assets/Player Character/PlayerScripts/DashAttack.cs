using UnityEngine;
using System.Collections;

public class DashAttack : MonoBehaviour
{
    public float timer = 0.1f;
    public float TargetTimeScale = 0f;
    public int AttackDamage = 1;
    public float ReboundForce = 5f;

    public bool isRebounding = false;

    public PlayerMovement playerMovement;
    public Rigidbody2D rb;
    public PlayerSoundFX playerSounds;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerSounds = GetComponent<PlayerSoundFX>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        TryDamageEnemy(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        TryDamageEnemy(collision);
    }

    private void TryDamageEnemy(Collider2D collision) {
        if(!collision.CompareTag("EnemyVulnerableBox") || !playerMovement.getIsDashing() || isRebounding) 
            return;

        EnemyStats enemyStats = collision.GetComponentInParent<EnemyStats>();

        if(enemyStats == null) 
            return;

        enemyStats.enemyHealth -= AttackDamage;
        playerSounds.playDashAttackSound();

        DamageFlash damageFlash = collision.GetComponentInParent<DamageFlash>();

        if (damageFlash != null) 
            damageFlash.EnemyFlash();

        Vector2 dashDirection = playerMovement.dashDirection;

        if (dashDirection == Vector2.zero) {
            dashDirection = transform.right;
        }
        
        StartCoroutine(ImpactPauseThenRebound(dashDirection));

        Debug.Log("Enemy hit by dash attack!" + " Enemy health: " + enemyStats.enemyHealth);

        playerMovement.resetDash();
    }

    IEnumerator ImpactPauseThenRebound(Vector2 dashDirection) {
        isRebounding = true;

        Time.timeScale = TargetTimeScale;

        yield return new WaitForSecondsRealtime(timer);

        Time.timeScale = 1f;

        playerMovement.ReboundFromDash(dashDirection, ReboundForce);

        yield return new WaitForSecondsRealtime(0.05f);

        isRebounding = false;
    }

    private void OnDisable() {
        Time.timeScale = 1f;
        isRebounding = false;
    }
        
}

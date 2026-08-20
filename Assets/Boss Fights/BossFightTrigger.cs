using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] private BossFightManager bossFightManager;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!collision.CompareTag("Player"))
            return;
        
        bossFightManager.BeginBossFight();
        gameObject.SetActive(false);
    }
}

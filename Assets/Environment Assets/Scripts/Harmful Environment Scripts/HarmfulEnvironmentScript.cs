using UnityEngine;

public class HarmfulEnvironmentScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlaceCharacter>()?.PlacePlayer();
            collision.gameObject.GetComponent<PlayerHurt>()?.TakeDamage(1);
        }
    }
}

using UnityEngine;

public class HarmfulEnvironmentScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerHurt>()?.Died();
        }
    }
}

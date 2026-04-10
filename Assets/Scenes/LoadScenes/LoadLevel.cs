using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            SceneManager.LoadScene("SampleScene");
        }
    }
}

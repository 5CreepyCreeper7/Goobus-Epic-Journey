using UnityEngine;

public class makeNotAngry : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusicSwapper musicSwapper = FindObjectOfType<MusicSwapper>();
            if (musicSwapper != null)
            {
                Debug.Log("makeNotAngry: Player entered trigger, setting angry state to false.");
                musicSwapper.SetAngryState(false);
            } else {
                Debug.LogWarning("makeNotAngry: No MusicSwapper instance found in the scene.");
            }
        }
    }
}

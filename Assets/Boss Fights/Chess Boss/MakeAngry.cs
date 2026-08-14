using UnityEngine;

public class MakeAngry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusicSwapper musicSwapper = FindObjectOfType<MusicSwapper>();
            if (musicSwapper != null)
            {
                Debug.Log("MakeAngry: Player entered trigger, setting angry state to true.");
                musicSwapper.SetAngryState(true);
            } else {
                Debug.LogWarning("MakeAngry: No MusicSwapper instance found in the scene.");
            }
        }
    }
}

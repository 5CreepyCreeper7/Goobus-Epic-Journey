using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject playerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null && spawnPoint != null) {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

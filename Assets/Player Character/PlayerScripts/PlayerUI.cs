using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour
{
    private HealthScript healthScript;

    public GameObject heartPrefab;
    public Transform heartContainer;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private List<Image> hearts = new List<Image>();

    private void Awake() {
        healthScript = FindFirstObjectByType<HealthScript>();
    }

    private void Start() {
        RefreshHearts();
    }

    public void RefreshHearts() {
        int totalHearts = Mathf.CeilToInt(healthScript.maxHealth / 2f);

        while(hearts.Count < totalHearts) {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            Image heartImage = newHeart.GetComponent<Image>();
            hearts.Add(heartImage);
        }

        while(hearts.Count > totalHearts) {
            Destroy(hearts[hearts.Count -1].gameObject);
            hearts.RemoveAt(hearts.Count - 1);
        }

        for(int i = 0; i < hearts.Count; i++) {
            int heartHealth = healthScript.currentHealth - (i * 2);

            if(heartHealth >= 2) {
                hearts[i].sprite = fullHeart;
            } else if(heartHealth == 1) {
                hearts[i].sprite = halfHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}

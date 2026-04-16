using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public int maxHealth = 6;

    public int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void IncreaseMaxHealth() {
        maxHealth += 2;
        currentHealth = maxHealth;
    }
}

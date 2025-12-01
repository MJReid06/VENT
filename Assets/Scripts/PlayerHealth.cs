using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;          
    public int currentHealth;

    [Header("UI & Game Over")]
    public HealthBar healthBar;
    public GameObject gameOverPanel;

    [Header("Automatic Health Decay")]
    public float damageInterval = 15f;  

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameOverPanel.SetActive(false);

        StartCoroutine(HealthDecay());
    }

    void Update()
    {
        // Test damage
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void KillPlayer()
    {
        currentHealth = 0;
        healthBar.SetHealth(0);
        Die();
    }

    void Die()
    {
        Debug.Log("Player Died");
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    IEnumerator HealthDecay()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(damageInterval);
            TakeDamage(1);
        }
    }
}

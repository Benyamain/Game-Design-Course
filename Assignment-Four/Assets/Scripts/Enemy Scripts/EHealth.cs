using UnityEngine;

public class EHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public float respawnTime = 5f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            EliminateEnemy();
        }
    }

    private void EliminateEnemy()
    {
        gameObject.SetActive(false);
        Invoke(nameof(RespawnEnemy), respawnTime);
        GameManager.CurrentScore++;
    }

    private void RespawnEnemy()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
    }
}
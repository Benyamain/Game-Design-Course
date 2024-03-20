using UnityEngine;

public class PHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        // End the game and stop all movement
        GetComponent<PlayerController>().enabled = false;

        // Stop all AI zombie agents
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            AIEnemyController enemyController = enemy.GetComponent<AIEnemyController>();
            if (enemyController != null)
            {
                enemyController.enabled = false;
                /* Telling the AI zombie agent to stop its current action and make a new decision
                based on the current state of the game. Since the game is over, the AI zombie agent will
                likely decide to stop moving or performing any other actions. */
                // enemyController.RequestDecision();
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float health = 3f;
    private Enemy enemyScript;
    [SerializeField]
    private Slider enemyHealthSlider;

    private void Awake() {
        enemyScript = GetComponent<Enemy>();
    }

    public void TakeDamage(float damageAmount) {
        if (health <= 0) return;
        health -= damageAmount;

        // When enemy has died, change state vars, destroy the game object, and increment the score
        if (health <= 0f)
        {
            health = 0;
            enemyScript.EnemyDied();
            EnemySpawner.instance.EnemyDied(gameObject);
            GameManager.CurrentScore++;
        }

        // Health slider
        enemyHealthSlider.value = health;
    }
}
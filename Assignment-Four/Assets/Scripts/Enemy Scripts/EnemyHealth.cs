using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float health = 5f;
    private Enemy enemyScript;
    [SerializeField]
    private Slider enemyHealthSlider;

    private void Awake() {
        enemyScript = GetComponent<Enemy>();
    }

    public void TakeDamage(float damageAmount) {
        if (health <= 0) return;
        health -= damageAmount;

        if (health <= 0f)
        {
            health = 0;
            enemyScript.EnemyDied();
            EnemySpawner.instance.EnemyDied(gameObject);
            GameManager.CurrentScore++;
        }

        enemyHealthSlider.value = health;
    }
}
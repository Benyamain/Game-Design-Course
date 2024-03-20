using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;

    [SerializeField]
    private Slider enemyHealthSlider;

    public void TakeDamage(float damageAmount) {
        if (health <= 0) return;
        health -= damageAmount;

        if (health <= 0f)
        {
            health = 0;
            GameManager.EnemyDied();
        }

        enemyHealthSlider.value = health;
    }
}
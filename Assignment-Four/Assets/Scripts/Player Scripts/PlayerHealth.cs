using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    [SerializeField]
    private Slider playerHealthSlider;

    public void TakeDamage(float damageAmount) {
        if (health <= 0f) {
            return;
        }

        health -= damageAmount;
        if (health <= 0) {
            GameManager.DisablePlayerCharacterController();
            GameManager.RestartGame();
            GameManager.ResetInstances();
            GameManager.EnablePlayerCharacterController();
        }

        playerHealthSlider.value = health;
    }
}
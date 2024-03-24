using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private float healAmount = 100f;
    public float respawnTime = 10f;
    private float playerHealthCondition = 100f;
    
    private void OnTriggerEnter(Collider other) {
        // Check when the player collides with the health pickup
        if (other.tag == "Player" && GameManager.PlayerHealth < playerHealthCondition) {
            GameManager.AddHealthPickup(healAmount);
            gameObject.SetActive(false);
            // Respawn timer on the health pickup
            Invoke(nameof(RespawnPickup), respawnTime);
        }
    }

    private void RespawnPickup()
    {
        gameObject.SetActive(true);
    }
}
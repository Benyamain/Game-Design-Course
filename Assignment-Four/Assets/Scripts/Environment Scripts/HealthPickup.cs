using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private float healAmount = 100f;
    public float respawnTime = 10f;
    private float playerHealthCondition = 100f;
    
    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player" && GameManager.PlayerHealth < playerHealthCondition) {
            // Destroy(gameObject);
            GameManager.AddHealthPickup(healAmount);
            gameObject.SetActive(false);
            Invoke(nameof(RespawnPickup), respawnTime);
        }
    }

    private void RespawnPickup()
    {
        gameObject.SetActive(true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageArea : MonoBehaviour
{
    [SerializeField]
    private float deactivateWaitTime = 0.1f;
    private float deactivateTimer;
    [SerializeField]
    private bool canDealDamage;
    private float damageAmount = 10f;

    private void Awake() {
        gameObject.SetActive(false);
    }

    private void Update() {
        // Check when player is near and take damage to them
        if (Physics.CheckSphere(transform.position, 1f))
        {
            if (canDealDamage)
            {
                canDealDamage = false;
                GameManager.PlayerTakeDamage(damageAmount);
            }
        }

        DeactivateDamageArea();
    }

    void DeactivateDamageArea() {
        // Cooldown for the enemy to hit not the player every frame
        if (Time.time > deactivateTimer) gameObject.SetActive(false);
    }

    public void ResetDeactivateTimer() {
        canDealDamage = true;
        deactivateTimer = Time.time + deactivateWaitTime;
    }
}
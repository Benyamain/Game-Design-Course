using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthBonus = 50;
    public float respawnTime = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PHealth>().HealPlayer(healthBonus);
            gameObject.SetActive(false);
            Invoke(nameof(RespawnPickup), respawnTime);
        }
    }

    void RespawnPickup()
    {
        gameObject.SetActive(true);
    }
}
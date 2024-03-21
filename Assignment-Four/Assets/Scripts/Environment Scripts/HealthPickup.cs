using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private AudioSource _medSFX;
    private float healAmount = 100f;
    public float respawnTime = 10f;
    
    // Start is called before the first frame update
    private void Start()
    {
        _medSFX = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player") {
            // Hear coin pickup
            if (!_medSFX.isPlaying) {
                _medSFX.Stop();
            }

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
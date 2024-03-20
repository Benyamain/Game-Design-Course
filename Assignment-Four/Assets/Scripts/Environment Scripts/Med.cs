using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Med : MonoBehaviour
{
    private AudioSource _medSFX;
    private MeshRenderer _medMeshRenderer;
    private BoxCollider _medBoxCollider;
    private float healAmount = 20f;
    
    // Start is called before the first frame update
    private void Start()
    {
        _medSFX = GetComponent<AudioSource>();
        _medBoxCollider = GetComponentInChildren<BoxCollider>();
        _medMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player") {
            // Hear coin pickup
            if (!_medSFX.isPlaying) {
                _medSFX.Stop();
            }

            // Destroy the game object after the audio clip is doing playing, so we can hear its beauty
            Destroy(gameObject);
            // Display this on the UI thread
            GameManager.AddHealthPickup(healAmount);
        }
    }
}
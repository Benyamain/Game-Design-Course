using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource _skullSFX;
    private MeshRenderer _skullMeshRenderer;
    private CapsuleCollider _skullCapsuleCollider;
    
    // Start is called before the first frame update
    private void Start()
    {
        _skullSFX = GetComponent<AudioSource>();
        _skullCapsuleCollider = GetComponentInChildren<CapsuleCollider>();
        _skullMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player") {
            // Hear coin pickup
            if (_skullSFX.isPlaying) {
                _skullSFX.Stop();
            }

            // Destroy the game object after the audio clip is doing playing, so we can hear its beauty
            Destroy(gameObject);
            // Display this on the UI thread
            GameManager.AddSkull();
        }
    }
}
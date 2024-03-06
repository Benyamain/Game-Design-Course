using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Audio file for the coin")]
    private AudioSource _coinSFX;
    
    // Start is called before the first frame update
    private void Start()
    {
        _coinSFX = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player") {
            // Hear coin pickup
            if (!_coinSFX.isPlaying) {
                _coinSFX.Play();
            }

            // Destroy the game object after the audio clip is doing playing, so we can hear its beauty
            Destroy(this.gameObject, 0.05f);
            // Display this on the UI thread
            GameManager.AddCoin();
        }
    }
}
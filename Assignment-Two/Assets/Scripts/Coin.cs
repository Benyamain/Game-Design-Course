using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Coin rotation speed")]
    private float _speed = 60f;
    private int _coinCount;
    private int _maxCoins = 5;
    [SerializeField]
    [Tooltip("Audio file for the coin")]
    private AudioSource _coinSFX;
    
    // Start is called before the first frame update
    private void Start()
    {
        _coinSFX = GetComponent<AudioSource>();
    }

    // Called once every frame
    private void Update()
    {
        if (_coinCount == _maxCoins) {
            ResetCoinCount();
        }
        
        // Rotate on the y-axis but do it with respect to time
        // https://docs.unity3d.com/ScriptReference/Transform.Rotate.html
        transform.Rotate(0f, _speed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player") {
            // Hear coin pickup
            if (!_coinSFX.isPlaying) {
                _coinSFX.Play();
                Debug.Log("Playing coin sound");
            }
            Destroy(this.gameObject);
            // Display this on the UI thread
            ++_coinCount;
        }
    }

    private void ResetCoinCount() {
        _coinCount = 0;
    }
}
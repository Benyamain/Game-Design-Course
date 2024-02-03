using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
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
        // As the number of coins increases and eventually meets the limit, we update GM and reset the count to exit the condition
        if (_coinCount == _maxCoins) {
            GameManager.MaxCoins = _maxCoins;
            Debug.Log(" GameManager.MaxCoins: " +  GameManager.MaxCoins);
            _coinCount = 0;
            Debug.Log("RESET _coinCount: " + _coinCount);
        }
    }

    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player") {
            // Hear coin pickup
            if (!_coinSFX.isPlaying) {
                _coinSFX.Play();
                Debug.Log("Playing coin audio");
            }
            Destroy(this.gameObject);
            // Display this on the UI thread
            ++_coinCount;
            Debug.Log("_coinCount: " + _coinCount);
            GameManager.CoinCount = _coinCount;
            Debug.Log("GameManager.CoinCount: " + GameManager.CoinCount);
        }
    }
}
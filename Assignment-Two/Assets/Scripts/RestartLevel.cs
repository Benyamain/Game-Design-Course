using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Audio file for the endzone")]
    private AudioSource _endzoneSFX;
    
    // Start is called before the first frame update
    private void Start()
    {
        _endzoneSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        // As long as enters, the music will keep playing until it reaches the end of its length
        if (other.tag == "Player") {
            // Hear endzone audio
            if (!_endzoneSFX.isPlaying) {
                _endzoneSFX.Play();
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player" && GameManager.CoinCount == GameManager.MaxCoins) {
            // Add GUI element to hint for the keypress
            // https://forum.unity.com/threads/restart-scene-key.812355/
            if (Input.GetKey(KeyCode.Space)) {
                ResetGameManagerCoins();
                // Load the scene again
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        // Irrelevant of the collider, just stop the music once called
        if (_endzoneSFX.isPlaying) {
            _endzoneSFX.Stop();
        }
    }

    // Reset values
    private void ResetGameManagerCoins() {
        GameManager.CoinCount = 0;
        GameManager.MaxCoins = 0;
    }
}

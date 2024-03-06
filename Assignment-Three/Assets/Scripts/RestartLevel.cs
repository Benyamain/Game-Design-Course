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

    private void OnTriggerStay(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player" && GameManager.SkullCount == GameManager.MaxSkulls) {
            // Can dance now
            GameManager.ReachedEndzone = true;
            GameManager.CanDance = true;

            // Hear endzone audio once you start dancing
            if (!_endzoneSFX.isPlaying) {
                _endzoneSFX.Play();
            }

            // https://forum.unity.com/threads/restart-scene-key.812355/
            if (Input.GetKey(KeyCode.R)) {
                // Load the scene again
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                GameManager.ResetInstances();
            } 
        }
    }

    private void OnTriggerExit(Collider other) {
        // Irrelevant of the collider, just stop the music once called
        if (_endzoneSFX.isPlaying) {
            _endzoneSFX.Stop();
        }

        GameManager.ReachedEndzone = false;
        GameManager.CanDance = false;
    }
}
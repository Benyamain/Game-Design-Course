using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RestartLevel : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void Start()
    {
        // TODO
    }

    private void OnTriggerStay(Collider other) {
        // https://discussions.unity.com/t/character-detection-from-tag-on-trigger-enter/53838/2
        if (other.tag == "Player" && GameManager.SkullCount == GameManager.MaxSkulls) {
            // Can dance now
            GameManager.ReachedEndzone = true;
            GameManager.CanDance = true;

            // https://forum.unity.com/threads/restart-scene-key.812355/
            if (Keyboard.current.rKey.wasPressedThisFrame) {
                // Load the scene again
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                GameManager.ResetInstances();
            } 
        }
    }

    private void OnTriggerExit(Collider other) {
        GameManager.ReachedEndzone = false;
        GameManager.CanDance = false;
    }
}
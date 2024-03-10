using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            GameManager.ReachedEndzone = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        GameManager.ReachedEndzone = false;
    }
}
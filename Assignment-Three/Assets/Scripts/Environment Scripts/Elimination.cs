using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elimination : MonoBehaviour
{    
    private AudioSource _eliminationSFX;
    
    // Start is called before the first frame update
    private void Start()
    {
        _eliminationSFX = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // Destroy(other.gameObject);
            // other.gameObject.SetActive(false);

            if (!_eliminationSFX.isPlaying) {
                _eliminationSFX.Play();
            }

            // Disable to character controller so we can move the player via the transform.
            GameManager.DisablePlayerCharacterController();
            
            // Set the position
            other.gameObject.transform.localPosition = GameManager.CheckpointPosition;
            
            // Set the rotation
            other.gameObject.transform.localEulerAngles = GameManager.CheckpointRotation;

            // Set the scale
            other.gameObject.transform.localScale = GameManager.CheckpointScale;
            
            // Enable the character controller so it can move again
            GameManager.EnablePlayerCharacterController();
        }
    }
    
    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            //Destroy(other.gameObject);
            //other.gameObject.SetActive(false);

            if (!_eliminationSFX.isPlaying) {
                _eliminationSFX.Play();
            }

            // Disable to character controller so we can move the player via the transform.
            GameManager.DisablePlayerCharacterController();
            
            // Set the position
            other.gameObject.transform.localPosition = GameManager.CheckpointPosition;
            
            // Set the rotation
            other.gameObject.transform.localEulerAngles = GameManager.CheckpointRotation;

            // Set the scale
            other.gameObject.transform.localScale = GameManager.CheckpointScale;
            
            // Enable the character controller so it can move again
            GameManager.EnablePlayerCharacterController();
        }
    }
}

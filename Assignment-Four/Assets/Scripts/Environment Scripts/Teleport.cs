using UnityEngine;

public class Teleport : MonoBehaviour
{    
    
    private Vector3 _checkpointPositionTop = new(-0.7f, 37.3f, 1.3f);
    private Vector3 _checkpointRotationTop = new(0f, 0f, 0f);
    private Vector3 _checkpointPositionBottom = new(-61.32f, 2.61f, -13.7f);
    private Vector3 _checkpointRotationBottom = new(0f, 90f, 0f);

    private Vector3 _checkpointScale = new(2f, 2f, 2f);
    
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // Disable to character controller so we can move the player via the transform.
            GameManager.DisablePlayerCharacterController();

            if (!GameManager.IsTeleportedToTop) {
                other.gameObject.transform.localPosition = _checkpointPositionTop;
                other.gameObject.transform.localEulerAngles = _checkpointRotationTop;
            } else {
                other.gameObject.transform.localPosition = _checkpointPositionBottom;
                other.gameObject.transform.localEulerAngles = _checkpointRotationBottom;
            }

            other.gameObject.transform.localScale = _checkpointScale;
            GameManager.IsTeleportedToTop = !GameManager.IsTeleportedToTop;
            
            // Enable the character controller so it can move again
            GameManager.EnablePlayerCharacterController();
        }
    }
    
}
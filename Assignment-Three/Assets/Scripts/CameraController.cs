using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float minX = -35f;
    [SerializeField]
    private float maxX = -30f;
    [SerializeField]
    private float minY = 40f;
    [SerializeField]
    private float maxY = 45f;
    [SerializeField]
    private float minZ = -63f;
    [SerializeField]
    private float maxZ = -60f;

    private void Update()
    {
        Transform cameraTransform = transform;

        // Calculate the clamped position based on the player's position
        Vector3 newPosition = GameManager.Player.transform.TransformPoint(cameraTransform.localPosition);

        // Clamp the position values to the specified ranges
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        // Apply the clamped position back to the camera's local position
        cameraTransform.localPosition = GameManager.Player.transform.InverseTransformPoint(newPosition);
    }
}
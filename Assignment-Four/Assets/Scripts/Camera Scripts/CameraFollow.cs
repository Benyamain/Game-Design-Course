using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float xAxis = 0f;
    public float yAxis = 20f;
    public float zAxis = 0f;

    private void Update()
    {
        // Camera follows the movement of the player
        transform.position = new Vector3(GameManager.Player.transform.position.x + xAxis, GameManager.Player.transform.position.y + yAxis, GameManager.Player.transform.position.z + zAxis);
    }
}
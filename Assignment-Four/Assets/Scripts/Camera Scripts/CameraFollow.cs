using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float xAxis = 0f;
    public float yAxis = 17f;
    public float zAxis = 8f;

    private void Update()
    {
        transform.position = new Vector3(GameManager.Player.transform.position.x + xAxis, GameManager.Player.transform.position.y + yAxis, GameManager.Player.transform.position.z + zAxis);
    }
}
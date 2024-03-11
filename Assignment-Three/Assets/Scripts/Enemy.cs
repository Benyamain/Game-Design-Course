using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField]
    private float speed = 5f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (GameManager.Player == null)
        {
            return;
        }

        // Calculate the direction to the player
        Vector3 directionToPlayer = GameManager.Player.transform.position - transform.position;
        directionToPlayer.y = 0f; // Keep the movement on the XZ plane

        // Normalize the direction to get a unit vector
        Vector3 moveDirection = directionToPlayer.normalized;

        // Move towards the player
        _controller.Move(moveDirection * speed * Time.deltaTime);
    }
}

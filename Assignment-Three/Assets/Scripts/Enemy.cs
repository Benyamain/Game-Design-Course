using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    // Ground the enemy so he does not go flying
    
    private CharacterController _controller;
    private Animator _animator;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float rotationSpeed = 2f;

    [SerializeField]
    private float meleeRange = 2f;

    private Vector3 _moveDirection;
    private bool _isRunning;
    private bool _isRunningBackwards;
    private bool _isLeftStrafing;
    private bool _isRightStrafing;
    private bool _isMelee;
    // private bool _isKicking;

    private Vector2 _move;
    private float _look;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _look = value.Get<float>();
    }

    private void Update()
    {
        if (GameManager.Player == null)
        {
            return;
        }

        ResetMovementState();

        if (_move.y > 0f)
        {
            _isRunning = true;
        }

        if (_move.y < 0f)
        {
            _isRunningBackwards = true;
        }

        if (_move.x > 0f)
        {
            _isRightStrafing = true;
        }

        if (_move.x < 0f)
        {
            _isLeftStrafing = true;
        }

        CalculateMovement();

        // Calculate the rotation to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Move towards the player
        _controller.Move(_moveDirection * speed * Time.deltaTime);

        // Check if the player is within arms reach for melee
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Player.transform.position);
        if (distanceToPlayer <= meleeRange)
        {
            // Player is within arms reach, trigger the melee animation
            _isMelee = true;
        }

        // Update animator based on movement and attack states
        UpdateAnimator();
    }

    private void CalculateMovement()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = GameManager.Player.transform.position - transform.position;
        directionToPlayer.y = 0f; // Keep the movement on the XZ plane

        // Normalize the direction to get a unit vector
        _moveDirection = directionToPlayer.normalized;
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("_isRunning", _isRunning);
        _animator.SetBool("_isRunningBackwards", _isRunningBackwards);
        _animator.SetBool("_isLeftStrafing", _isLeftStrafing);
        _animator.SetBool("_isRightStrafing", _isRightStrafing);
        _animator.SetBool("_isMelee", _isMelee);
        // _animator.SetBool("_isKicking", _isKicking);
    }

    private void ResetMovementState()
    {
        _isRunning = false;
        _isRunningBackwards = false;
        _isLeftStrafing = false;
        _isRightStrafing = false;
        _isMelee = false;
        // _isKicking = false;
    }
}
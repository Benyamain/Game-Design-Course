using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    // Ground the enemy so he does not go flying
    
    private CharacterController _controller;
    private Animator _animator;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float rotationSpeed = 2f;

    [SerializeField]
    private float chaseRange = 25f;

    // Maybe change this when they collide with each other?
    [SerializeField]
    private float meleeRange = 3f;

    private Vector3 _moveDirection;
    private bool _isRunning;
    private bool _isRunningBackwards;
    private bool _isLeftStrafing;
    private bool _isRightStrafing;
    private bool _isMelee;
    private bool _canMelee = true;
    // private bool _isKicking;

    [SerializeField]
    private float gravity = -2f;

    [SerializeField]
    private float _velocity;

    private Vector2 _move;
    private float _look;
    private float damageAmount = 20f;
    private bool _isAttacking = false;

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

        // Check if the player is within arms reach for melee
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Player.transform.position);

        if (distanceToPlayer <= chaseRange)
        {
        
            OrientateToPlayer();

            // Calculate the rotation to look at the player
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);

            // Smoothly rotate towards the target rotation
            // https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            _velocity += gravity * Time.deltaTime;

            // Keep the enemy grounded by setting the y-component of the movement direction
            // to a negative value equal to the current downward velocity
            Vector3 movement = _moveDirection * moveSpeed * Time.deltaTime;
            movement.y = _velocity * Time.deltaTime;

            // Move towards the player
            _controller.Move(movement);

            // Check if the player is within arms reach for melee
            if (distanceToPlayer <= meleeRange && _canMelee)
            {
                // _isMelee = true;

                float time = 0f;
                _animator.Play("Melee", 0, 0f);
                time += Time.deltaTime;

                // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Melee") && (time <= _animator.GetCurrentAnimatorStateInfo(0).normalizedTime)) {
                //     GameManager.Player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
                //     time = 0f;
                // }

                // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Melee") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f) {
                //     _canMelee = true;
                //     _animator.Play("Melee", 0, 0f);
                //     _isAttacking = true;
                // }
            }
            else
            {
                // _isMelee = false;
                // _isAttacking = false;
            }

            // Update animator based on movement and attack states
            UpdateAnimator();
            // DamagePlayer();
        }
    }

    private void DamagePlayer() {
        if (_isAttacking) {
            GameManager.Player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        }
    }

    // TODO: Fix Player taking way too much damage from Enemy hit.
    // private void OnControllerColliderHit(ControllerColliderHit hit) {
    //     if (hit.collider.CompareTag("Player"))
    //     {
    //         GameManager.Player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
    //     }
    // }

    // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Melee") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f) {
    //     GameManager.Player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
    // }

    private void OrientateToPlayer()
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
        // _animator.SetBool("_isMelee", _isMelee);
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
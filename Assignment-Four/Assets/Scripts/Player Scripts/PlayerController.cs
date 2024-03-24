using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10f;

    [Tooltip("How fast to turn in degrees.")]
    [Min(float.Epsilon)]
    [SerializeField]
    private float lookSpeed = 8f;

    [Tooltip("How much velocity to add for jumping.")]
    [Min(float.Epsilon)]
    [SerializeField]
    private float jumpForce = 0.5f;

    [Tooltip("The gravity to apply to this character.")]
    [SerializeField]
    private float gravity = -2f;

    [Tooltip("The terminal velocity of the character.")]
    [SerializeField]
    private float terminalVelocity = -1f;

    [SerializeField]
    [Tooltip("What ties in the animation to the player")]
    private Animator _animator;

    [SerializeField]
    private Transform playerTransform;
    public bool isRunning;
    public bool isRunningBackwards;
    public bool _isLeftStrafing;
    private bool _isRightStrafing;
    private bool _isShooting;
    private bool _isJumping;
    private bool _canJump = true;
    private float _velocity;

    private void Start()
    {
        // Get the animator component
        _animator = playerTransform.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        // Reset movement animations every frame
        ResetMovementState();

        Vector3 movement = GetMovementDirection();
        ApplyGravity();
        GameManager.PlayerCharacterController.Move(movement + new Vector3(0f, _velocity, 0f));

        RotateTowardsMouse();
        UpdateAnimator();
        HandleInput();
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 movement = Vector3.zero;
        Ray ray = GameManager.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // Detect what direction that the player wants to go in based on where their current mouse position is.
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0f;
            movement = targetDirection.normalized * moveSpeed * Time.deltaTime;

            // https://docs.unity3d.com/ScriptReference/Vector3.SignedAngle.html
            float angle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
            if (angle > 0f)
            {
                _isRightStrafing = true;
            }
            else if (angle < 0f)
            {
                _isLeftStrafing = true;
            }
            else
            {
                /* Measure the difference between the current player position and where their current mouse position is.
                Then, find the magnitude. */
                if (targetDirection.magnitude > 0.1f)
                {
                    isRunning = true;
                }
                else
                {
                    isRunningBackwards = true;
                }
            }
        }

        return movement;
    }

    private void ApplyGravity()
    {
        _velocity += gravity * Time.deltaTime;

        if (_velocity < terminalVelocity)
        {
            _velocity = terminalVelocity;
        }
    }

    private void RotateTowardsMouse()
    {
        Ray ray = GameManager.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // We now rotate where the mouse is placed in the game
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0f;
            // Smooth rotation
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("_isRunning", isRunning);
        _animator.SetBool("_isRunningBackwards", isRunningBackwards);
        _animator.SetBool("_isLeftStrafing", _isLeftStrafing);
        _animator.SetBool("_isRightStrafing", _isRightStrafing);
        _animator.SetBool("_isShooting", _isShooting);
        _animator.SetBool("_isJumping", _isJumping);
    }

    private void HandleInput()
    {
        if (_canJump && Keyboard.current.spaceKey.isPressed)
        {
            StartCoroutine(JumpWithDelay());
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GameManager.RestartGame();
            GameManager.ResetInstances();
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(GameManager.LoadMenu);
        }
    }

    private void ResetMovementState()
    {
        isRunning = false;
        isRunningBackwards = false;
        _isLeftStrafing = false;
        _isRightStrafing = false;
        _isShooting = false;
        _isJumping = false;
    }

    private IEnumerator JumpWithDelay()
    {
        _canJump = false;
        _isJumping = false;

        if (GameManager.PlayerCharacterController.isGrounded)
        {
            _velocity = jumpForce;
            _isJumping = true;
        }

        yield return new WaitForSeconds(0.5f);

        _canJump = true;
    }
}
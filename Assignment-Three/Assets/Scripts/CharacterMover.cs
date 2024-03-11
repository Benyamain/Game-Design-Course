using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    // Animation documentation: https://docs.unity3d.com/Manual/class-AnimatorController.html
    // Animations with input system: https://discussions.unity.com/t/how-to-set-animators-controller-in-script/63474/3
    // Animations: https://youtu.be/C2jrQ8VWvUs?si=R_tDfmc8dvKqQKuJ
    // Movement: https://youtu.be/_QajrabyTJc?si=5i___U6P-PwVYXvG

    [Tooltip("How fast to move.")]
    [Min(float.Epsilon)]
    [SerializeField]
    private float moveSpeed = 10f;
    
    [Tooltip("How fast to turn in degrees.")]
    [Min(float.Epsilon)]
    [SerializeField]
    private float lookSpeed = 2f;

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

    [Tooltip("The position to teleport the player to.")]
    [SerializeField]
    private Vector3 teleportPosition = new(0, 0.5f, 0);
    
    [Tooltip("The rotation to teleport the player to.")]
    [SerializeField]
    private Vector3 teleportRotation = Vector3.zero;

    private CharacterController _controller;

    [SerializeField]
    [Tooltip("What ties in the animation to the player")]
    private Animator _animator;

    [SerializeField]
    private Transform _playerTransform;
    private bool _isRunning;
    private bool _isRunningBackwards;
    private bool _isLeftStrafing;
    private bool _isRightStrafing;
    private bool _isShooting;
    private bool _isJumping;
    private bool _isSprinting;
    private float _stopwatch;
    private AudioSource _weaponSFX;
    private bool _canJump = true;

    /// <summary>
    /// The vertical velocity to handle jumping and falling.
    /// </summary>
    private float _velocity;

    private  Vector2 _move;
    private  float _look;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        // Get animator attached on the player
        _animator = _playerTransform.gameObject.GetComponent<Animator>();
        _weaponSFX = GetComponent<AudioSource>();

        Cursor.lockState  = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMove(InputValue value) {
        _move = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _look = value.Get<float>();
    }

    private void Update()
    {
        // https://forum.unity.com/threads/trying-to-make-a-stopwatch-with-time-deltatime.1223490/
        _stopwatch += Time.deltaTime;
        // Display to UI
        GameManager.CurrentTime = _stopwatch;

        // Before moving every frame, reset animator states, which means player is idle
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

        if (Mouse.current.leftButton.isPressed) {
            _isShooting = true;

            // Shoot sound
            if (!_weaponSFX.isPlaying) {
                _weaponSFX.Play();
            }
        }
        
        // When player stops shooting
        if (Mouse.current.leftButton.wasReleasedThisFrame) {
            _isShooting = false;

            if (_weaponSFX.isPlaying)
            {
                _weaponSFX.Stop();
            }
        }

        if (!_isRunning && !_isShooting && Keyboard.current.leftShiftKey.isPressed) {
            _isSprinting = true;
        }

        // Cache the transform for performance
        Transform t = transform;

        // Rotation on y-axis
        t.eulerAngles = new(0f, t.eulerAngles.y + _look * lookSpeed, 0f);
        
        // Teleport the player when T is pressed.
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            // Disable to character controller so we can move the player via the transform.
            _controller.enabled = false;
            
            // Set the position
            t.position = teleportPosition;
            
            // Set the rotation
            t.eulerAngles = teleportRotation;
            
            // Enable the character controller so it can move again
            _controller.enabled = true;
        }

        if (Keyboard.current.fKey.wasPressedThisFrame) {
            GameManager.IsLocalLayer = !GameManager.IsLocalLayer;
            GameManager.ChangeLayer(GameManager.IsLocalLayer);
        }
        
        // Rotate on the y (green) axis for turning.
        t.Rotate(0f, _look * lookSpeed * Time.deltaTime, 0f);

        // Calculate the forwards and backwards movement relative to the direction the character is facing.
        Vector3 movement = t.forward * (moveSpeed * _move * Time.deltaTime);

        if (_canJump && Keyboard.current.spaceKey.wasPressedThisFrame) {
            StartCoroutine(JumpWithDelay());
        }

        // https://forum.unity.com/threads/restart-scene-key.812355/
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            // Load the scene again
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.ResetInstances();
        } 
        
        // Every frame apply gravity
        _velocity += gravity * Time.deltaTime;

        if (_velocity < terminalVelocity)
        {
            _velocity = terminalVelocity;
        }

        // Apply the walking movement and the vertical velocity to the character.
        _controller.Move(new(movement.x, _velocity, movement.z));

        // Ultimately, this will check if we have collected all the coins and are at the endzone so you earned this dance move!
        // Add GUI element to hint for the keypress
        if ((GameManager.SkullCount == GameManager.MaxSkulls) && GameManager.ReachedEndzone) {
            // TODO
        }

        if (Keyboard.current.mKey.wasPressedThisFrame) {
            Cursor.lockState  = CursorLockMode.None;
            Cursor.visible = true;

            // Load the level based on the index set in the inspector.
            SceneManager.LoadScene(GameManager.LoadMenu);
        }

        // Update animator based on movement states
        _animator.SetBool("_isRunning", _isRunning);
        _animator.SetBool("_isRunningBackwards", _isRunningBackwards);
        _animator.SetBool("_isLeftStrafing", _isLeftStrafing);
        _animator.SetBool("_isRightStrafing", _isRightStrafing);
        _animator.SetBool("_isShooting", _isShooting);
        _animator.SetBool("_isJumping", _isJumping);
        _animator.SetBool("_isSprinting", _isSprinting);
    }

    private void ResetMovementState() {
        _isRunning = false;
        _isRunningBackwards = false;
        _isLeftStrafing = false;
        _isRightStrafing = false;
        _isShooting = false;
        _isJumping = false;
        _isSprinting = false;
    }

    private IEnumerator JumpWithDelay() {
        _canJump = false;
        _isJumping = false;

        if (_controller.isGrounded)
        {
            _velocity = jumpForce;
            _isJumping = true;
        }

        yield return new WaitForSeconds(2f);

        _canJump = true;
    }
}
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
    private float lookSpeed = 180f;

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
    private Transform _playerTransform;
    private bool _isRunning;
    private bool _isRunningBackwards;
    private bool _isLeftStrafing;
    private bool _isRightStrafing;
    private bool _isShooting;
    private bool _isJumping;
    private bool _isSprinting;
    private bool _isCrouching;
    private float _stopwatch;
    private AudioSource _weaponSFX;
    private bool _canJump = true;

    private bool _playerDied;

    /// <summary>
    /// The vertical velocity to handle jumping and falling.
    /// </summary>
    private float _velocity;

    private void Start()
    {
        // Get animator attached on the player
        _animator = _playerTransform.gameObject.GetComponent<Animator>();
        _weaponSFX = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // https://forum.unity.com/threads/trying-to-make-a-stopwatch-with-time-deltatime.1223490/
        _stopwatch += Time.deltaTime;
        // Display to UI
        GameManager.CurrentTime = _stopwatch;

        // Before moving every frame, reset animator states, which means player is idle
        ResetMovementState();
        
        // Variables to hold our movement (forwards and back) and looking (right and left) data.
        float move = 0f;
        float look = 0f;
        
        if (Keyboard.current.wKey.isPressed)
        {
            move += 1f;
            _isRunning = true;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            move -= 1f;
            _isRunningBackwards = true;
        }
        
        if (Keyboard.current.dKey.isPressed)
        {
            look += 1f;
            _isRightStrafing = true;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            look -= 1f;
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

        if (Keyboard.current.cKey.isPressed)
        {
            _isCrouching = true;
        }

        if (!_isRunning && !_isShooting && Keyboard.current.leftShiftKey.isPressed) {
            move += 1.25f;
            _isSprinting = true;
        }

        // Cache the transform for performance
        Transform t = transform;

        if (Keyboard.current.fKey.wasPressedThisFrame) {
            GameManager.IsLocalLayer = !GameManager.IsLocalLayer;
            GameManager.ChangeLayer(GameManager.IsLocalLayer);
        }
        
        // Rotate on the y (green) axis for turning.
        t.Rotate(0, look * lookSpeed * Time.deltaTime, 0);

        // Calculate the forwards and backwards movement relative to the direction the character is facing.
        Vector3 movement = t.forward * (moveSpeed * move * Time.deltaTime);

        if (_canJump && Keyboard.current.spaceKey.wasPressedThisFrame) {
            StartCoroutine(JumpWithDelay());
        }

        // https://forum.unity.com/threads/restart-scene-key.812355/
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            // Load the scene again
            GameManager.RestartGame();
            GameManager.ResetInstances();
        } 
        
        // Every frame apply gravity
        _velocity += gravity * Time.deltaTime;

        if (_velocity < terminalVelocity)
        {
            _velocity = terminalVelocity;
        }

        // Apply the walking movement and the vertical velocity to the character.
        GameManager.PlayerCharacterController.Move(new(movement.x, _velocity, movement.z));

        // Ultimately, this will check if we have collected all the coins and are at the endzone so you earned this dance move!
        // Add GUI element to hint for the keypress
        if ((GameManager.SkullCount == GameManager.MaxSkulls) && GameManager.ReachedEndzone) {
            // TODO
        }

        // Update animator based on movement states
        _animator.SetBool("_isRunning", _isRunning);
        _animator.SetBool("_isRunningBackwards", _isRunningBackwards);
        _animator.SetBool("_isLeftStrafing", _isLeftStrafing);
        _animator.SetBool("_isRightStrafing", _isRightStrafing);
        _animator.SetBool("_isShooting", _isShooting);
        _animator.SetBool("_isJumping", _isJumping);
        _animator.SetBool("_isSprinting", _isSprinting);
        _animator.SetBool("_isCrouching", _isCrouching);
    }

    private void ResetMovementState() {
        _isRunning = false;
        _isRunningBackwards = false;
        _isLeftStrafing = false;
        _isRightStrafing = false;
        _isShooting = false;
        _isJumping = false;
        _isSprinting = false;
        _isCrouching = false;
    }

    private IEnumerator JumpWithDelay() {
        _canJump = false;
        _isJumping = false;

        if (GameManager.PlayerCharacterController.isGrounded)
        {
            _velocity = jumpForce;
            _isJumping = true;
        }

        yield return new WaitForSeconds(2f);

        _canJump = true;
    }
}
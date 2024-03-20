using System.Collections;
using System.Windows.Markup;
using UnityEngine;
using UnityEngine.AI;
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
    private bool _isSprinting;
    private bool _isCrouching;
    private int _stopwatch;
    private AudioSource _weaponSFX;
    public bool canShoot = true;
    private bool _canJump = true;
    private bool _canSprint = false;
    private bool _canCrouch = true;

    [Tooltip("How sensitive the mouse input should be.")]
    [Min(float.Epsilon)]
    [SerializeField]
    private float mouseSensitivity = 1f;

    /// <summary>
    /// The vertical velocity to handle jumping and falling.
    /// </summary>
    private float _velocity;

    private void Start()
    {
        // Get animator attached on the player
        _animator = playerTransform.gameObject.GetComponent<Animator>();
        _weaponSFX = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Display to UI
        GameManager.CurrentScore = _stopwatch;

        // Before moving every frame, reset animator states, which means player is idle
        ResetMovementState();

        // Calculate the forwards and backwards movement relative to the direction the character is facing.
        Vector3 movement = new Vector3(0f, 0f, 0f);
        
        if (Keyboard.current.wKey.isPressed)
        {
            if (_canSprint) {
                _canCrouch = canShoot = false;
                movement += GameManager.Player.transform.forward * moveSpeed * 1.25f * Time.deltaTime;
                _isSprinting = true;
            }
            else {
                canShoot = _canCrouch = _canSprint = false;
                movement += GameManager.Player.transform.forward * moveSpeed * Time.deltaTime;
                isRunning = true;
            }

            if (_weaponSFX.isPlaying) {
                _weaponSFX.Stop();
            }
        }

        if (Keyboard.current.wKey.wasReleasedThisFrame)
        {
            _canJump = canShoot = _canCrouch = true;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            canShoot = _canCrouch = _canSprint = false;
            movement -= GameManager.Player.transform.forward * moveSpeed * Time.deltaTime;
            isRunningBackwards = true;

            if (_weaponSFX.isPlaying) {
                _weaponSFX.Stop();
            }
        }

        if (Keyboard.current.sKey.wasReleasedThisFrame)
        {
            _canJump = canShoot = _canCrouch = true;
        }
        
        if (Keyboard.current.dKey.isPressed)
        {
            _canCrouch = _canSprint = false;
            movement += GameManager.Player.transform.right * moveSpeed * Time.deltaTime;
            _isRightStrafing = true;
        }

        if (Keyboard.current.dKey.wasReleasedThisFrame)
        {
            _canJump = canShoot = _canCrouch = true;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            _canCrouch = _canSprint = false;
            movement -= GameManager.Player.transform.right * moveSpeed * Time.deltaTime;
            _isLeftStrafing = true;
        }

        if (Keyboard.current.aKey.wasReleasedThisFrame)
        {
           _canJump = canShoot = _canCrouch = true;
        }

        if (canShoot && Mouse.current.leftButton.isPressed) {
            _canJump = _canCrouch = _canSprint = false;
            _isShooting = true;

            // Shoot sound
            if (!_weaponSFX.isPlaying && !isRunning && !isRunningBackwards) {
                _weaponSFX.Play();
            }

            RaycastHit hit;
            if (Physics.Raycast(GameManager.MainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                GameManager.Player.GetComponent<NavMeshAgent>().SetDestination(hit.point);
            }
        }
        
        // When player stops shooting
        if (Mouse.current.leftButton.wasReleasedThisFrame) {
            _canJump = _canCrouch = true;
            _isShooting = false;

            if (_weaponSFX.isPlaying)
            {
                _weaponSFX.Stop();
            }
        }

        if (_canCrouch && Keyboard.current.cKey.isPressed)
        {
            _canJump = canShoot = _canSprint = false;
            _isCrouching = true;
        }

        if (Keyboard.current.cKey.wasReleasedThisFrame)
        {
            _canJump = canShoot = true;
        }

        if (Keyboard.current.leftShiftKey.isPressed) {
            _canSprint = true;
        }

        if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
        {
            _canSprint = false;
        }
        
        if (_canJump && Keyboard.current.spaceKey.isPressed) {
            canShoot = _canCrouch = _canSprint = false;
            StartCoroutine(JumpWithDelay());
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            canShoot = _canCrouch = true;
        }

        // https://forum.unity.com/threads/restart-scene-key.812355/
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            // Load the scene again
            GameManager.RestartGame();
            GameManager.ResetInstances();

            // New additions
            GameManager.EnablePlayerCharacterController();
            GameManager.EnableEnemyCharacterController();
        }

        if (Keyboard.current.mKey.wasPressedThisFrame) {
            SceneManager.LoadScene(GameManager.LoadMenu);
        }
        
        // Every frame apply gravity
        _velocity += gravity * Time.deltaTime;

        if (_velocity < terminalVelocity)
        {
            _velocity = terminalVelocity;
        }

        // Apply the walking movement and the vertical velocity to the character.
        GameManager.PlayerCharacterController.Move(movement + new Vector3(0f, _velocity, 0f));

        // Get mouse input for rotation
        Vector2 mouseInput = Mouse.current.delta.ReadValue();

        transform.Rotate(Vector3.up * mouseInput.x * lookSpeed * mouseSensitivity * Time.deltaTime, Space.Self);

        // Update animator based on movement states
        UpdateAnimator();
    }

    private void UpdateAnimator() {
        _animator.SetBool("_isRunning", isRunning);
        _animator.SetBool("_isRunningBackwards", isRunningBackwards);
        _animator.SetBool("_isLeftStrafing", _isLeftStrafing);
        _animator.SetBool("_isRightStrafing", _isRightStrafing);
        _animator.SetBool("_isShooting", _isShooting);
        _animator.SetBool("_isJumping", _isJumping);
        _animator.SetBool("_isSprinting", _isSprinting);
        _animator.SetBool("_isCrouching", _isCrouching);
    }

    private void AllowExtraMovements() {
        _canJump = canShoot = _canCrouch = _canSprint = true;
    }

    private void DisableExtraMovements() {
        _canJump = canShoot = _canCrouch = _canSprint = false;
    }

    private void ResetMovementState() {
        isRunning = false;
        isRunningBackwards = false;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    // Public getter private setter
    public bool CanMove {get; private set; } = true;

    // Lambda4
    private bool IsSprinting => _canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKey(jumpKey) && _characterController.isGrounded;

    private bool _canSprint = true;
    private bool _canJump = true;
    private KeyCode sprintKey = KeyCode.LeftShift;
    private KeyCode jumpKey = KeyCode.Space;
    
    [SerializeField]
    private float jumpForce = 8f;

    [SerializeField]
    private float walkSpeed = 3f;

    [SerializeField]
    private float sprintSpeed = 6f;

    [SerializeField]
    private float gravity = 30f;

    [SerializeField, Range(1, 10)]
    private float lookSpeedX = 2f;

    [SerializeField, Range(1, 10)]
    private float lookSpeedY = 2f;

    [SerializeField, Range(1, 180)]
    private float upperLookLimit = 60f;

    [SerializeField, Range(1, 180)]
    private float lowerLookLimit = 60f;

    private Camera _playerCamera;
    private CharacterController _characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0f;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();

        Cursor.lockState  = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (CanMove) {
            HandleMovementInput();
            HandleMouseLook();

            if (_canJump) {
                HandleJump();
            }

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput() {
        currentInput = new Vector2((IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        // Reset the y position to original position
        float moveDirectionY = moveDirection.y;
        // Orientation of current player moves on the current axis they are facing
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);

        // Cached
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook() {
        // Mouse Y value controls the X rotation of the camera
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Rotate the parent game object (player) itself
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeedX, 0f);
    }

    private void HandleJump() {
        if (ShouldJump) {
            moveDirection.y = jumpForce;
        }
    }

    private void ApplyFinalMovements() {
        if (!_characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        _characterController.Move(moveDirection * Time.deltaTime);
    }
}
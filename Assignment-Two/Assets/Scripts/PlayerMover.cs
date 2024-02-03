using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    // Animation documentation: https://docs.unity3d.com/Manual/class-AnimatorController.html
    // Animations with input system: https://discussions.unity.com/t/how-to-set-animators-controller-in-script/63474/3
    // Animations: https://youtu.be/C2jrQ8VWvUs?si=R_tDfmc8dvKqQKuJ
    
    [SerializeField]
    [Tooltip("What adds physics complexity to our player")]
    private Rigidbody _rb;
    [SerializeField]
    [Tooltip("What ties in the animation to the player")]
    private Animator _animator;
    [SerializeField]
    [Tooltip("Movement speed relative to delta time")]
    [Range(1f, 10f)]
    private float _movementSpeed = 1f;
    private  Vector2 _move;
    [SerializeField]
    private Transform _playerTransform;
    private bool _isRunning;
    private bool _isRunningBackwards;
    private bool _isDancing;
    private bool _isLeftStrafing;
    private bool _isRightStrafing;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Player is idle when game starts
        _rb = GetComponent<Rigidbody>();
        // Get animator attached on the player
        _animator = _playerTransform.gameObject.GetComponent<Animator>();
    }

    private void OnMove(InputValue value) {
        _move = value.Get<Vector2>();
    }

    // Update is called upon a fixed time
    private void Update()
    {
        // Prevent caching
        Transform t = transform;

        float speed = _movementSpeed * Time.deltaTime;

        // Before moving every frame, reset animator states
        ResetMovementState();

        // Animations based on direction
        if (_move.y > 0f) {
            _isRunning = true;
        }
        else if (_move.y < 0f) {
            _isRunningBackwards = true;
        }
        else if (_move.x > 0f) {
            _isRightStrafing = true;
        }
        else if (_move.x < 0f) {
            _isLeftStrafing = true;
        }
        else if (false) {
            // Replace this when condition of reaching end zone and player wins, start dancing
            _isDancing = true;
        }

        // Update animator based on movement states
        _animator.SetBool("_isRunning", _isRunning);
        _animator.SetBool("_isRunningBackwards", _isRunningBackwards);
        _animator.SetBool("_isDancing", _isDancing);
        _animator.SetBool("_isLeftStrafing", _isLeftStrafing);
        _animator.SetBool("_isRightStrafing", _isRightStrafing);

        // Move player forward, backwards, right, left
        t.position += t.forward * (_move.y * speed) + t.right * (_move.x * speed);
    }

    private void ResetMovementState() {
        _isRunning = false;
        _isRunningBackwards = false;
        _isDancing = false;
        _isLeftStrafing = false;
        _isRightStrafing = false;
    }
}
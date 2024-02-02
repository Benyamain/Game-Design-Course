using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    // https://docs.unity3d.com/Manual/class-AnimatorController.html
    // https://youtu.be/9H0aJhKSlEQ?si=Fb35VJ7kvBwtH724
    // https://discussions.unity.com/t/how-to-set-animators-controller-in-script/63474/3
    // https://youtu.be/C2jrQ8VWvUs?si=R_tDfmc8dvKqQKuJ
    
    [SerializeField]
    [Tooltip("What adds physics complexity to our player")]
    private Rigidbody _rb;
    [SerializeField]
    [Tooltip("How much force is applied to the player")]
    [Range(1f, 10f)]
    private float _force = 1f; 
    [SerializeField]
    [Tooltip("What ties in the animation to the player")]
    private Animator _animator;
    [SerializeField]
    private Transform _playerTransform;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = _playerTransform.gameObject.GetComponent<Animator>();
    }

    // Update is called upon a fixed time
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) {
            _rb.AddForce(0f, _force, 0f);
            _animator.runtimeAnimatorController = Resources.Load("Assets/Animations/AC_Running.controller") as RuntimeAnimatorController;
            Debug.Log("The animation running is ", _animator.runtimeAnimatorController);
        }
        else if (Input.GetKey(KeyCode.A)) {
            _rb.AddForce(-_force, 0f, 0f);
            _animator.runtimeAnimatorController = Resources.Load("Assets/Animations/AC_LeftStrafe.controller") as RuntimeAnimatorController;
            Debug.Log("The animation running is ", _animator.runtimeAnimatorController);
        }
        else if (Input.GetKey(KeyCode.S)) {
            _rb.AddForce(0f, -_force, 0f);
            _animator.runtimeAnimatorController = Resources.Load("Assets/Animations/AC_RunningBackwards.controller") as RuntimeAnimatorController;
            Debug.Log("The animation running is ", _animator.runtimeAnimatorController);
        }
        else if (Input.GetKey(KeyCode.D)) {
            _rb.AddForce(_force, 0f, 0f);
            _animator.runtimeAnimatorController = Resources.Load("Assets/Animations/AC_RightStrafe.controller") as RuntimeAnimatorController;
            Debug.Log("The animation running is ", _animator.runtimeAnimatorController);
        }
        else {
            _rb.AddForce(0f, 0f, 0f);
            _animator.runtimeAnimatorController = Resources.Load("Assets/Animations/AC_StandingIdle.controller") as RuntimeAnimatorController;
            Debug.Log("The animation running is ", _animator.runtimeAnimatorController);
        }
    }
}

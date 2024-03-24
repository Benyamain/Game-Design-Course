using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Ground the enemy so he does not go flying
    
    private Animator _animator;

    private float moveSpeed = 2f;

    [SerializeField]
    private float rotationSpeed = 2f;

    private float chaseRange = 250f;

    // Maybe change this when they collide with each other?
    [SerializeField]
    private float meleeRange = 3f;

    private Vector3 _moveDirection;
    private bool _isRunning;
    private bool _isRunningBackwards;
    private bool _isLeftStrafing;
    private bool _isRightStrafing;
    private bool _isMelee;
    // private bool _isKicking;

    [SerializeField]
    private float gravity = -2f;

    [SerializeField]
    private float _velocity;

    [SerializeField]
    private float attackWaitTime = 2.5f;
    private float _attackTimer;
    [SerializeField]
    private float attackFinishedWaitTime = 0.5f;
    private float _attackFinishedTimer;
    private CharacterController _enemyCharacterController;

    [SerializeField]
    private EnemyDamageArea enemyDamageArea;
    private bool enemyDied;
    private NavMeshAgent enemyNavMeshAgent;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyCharacterController = GetComponent<CharacterController>();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // enemyNavMeshAgent.destination = GameManager.Player.position;
        
        if (GameManager.IsPlayerDead) {
            GameManager.GameOver();
            _enemyCharacterController.enabled = false;
            // Disable the script
            this.enabled = false;
        }
        
        if (enemyDied) return;
        
        if (GameManager.Player == null)
        {
            return;
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
            _enemyCharacterController.Move(movement);

            if (_moveDirection.z > 0f) {
                _isRunning = true;
                _isRunningBackwards = _isRightStrafing = _isLeftStrafing = false;
            }

            if (_moveDirection.z < 0f) {
                _isRunningBackwards = true;
                _isRunning = _isRightStrafing = _isLeftStrafing = false;
            }

            if (_moveDirection.x < 0f) {
                _isLeftStrafing = true;
                _isRunningBackwards = _isRightStrafing = _isRunning = false;
            }

            if (_moveDirection.x > 0f) {
                _isRightStrafing = true;
                _isRunningBackwards = _isRunning = _isLeftStrafing = false;
            }

            if (distanceToPlayer <= meleeRange)
            {
                _isMelee = true;
                CheckIfAttackFinished();
                Attack();
            }

            // if (enemyNavMeshAgent.destination.z > 0f)
            // {
            //     _isRunning = true;
            //     _isRunningBackwards = _isRightStrafing = _isLeftStrafing = false;
            // }

            // if (enemyNavMeshAgent.destination.z < 0f)
            // {
            //     _isRunningBackwards = true;
            //     _isRunning = _isRightStrafing = _isLeftStrafing = false;
            // }

            // if (enemyNavMeshAgent.destination.x < 0f)
            // {
            //     _isLeftStrafing = true;
            //     _isRunningBackwards = _isRightStrafing = _isRunning = false;
            // }

            // if (enemyNavMeshAgent.destination.x > 0f)
            // {
            //     _isRightStrafing = true;
            //     _isRunningBackwards = _isRunning = _isLeftStrafing = false;
            // }

            // if (enemyNavMeshAgent.destination.z <= meleeRange)
            // {
            //     _isMelee = true;
            //     CheckIfAttackFinished();
            //     Attack();
            // }
        }
        // Update animator based on movement and attack states
        UpdateAnimator();
    }

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
        _animator.SetBool("_isMelee", _isMelee);
        // _animator.SetBool("_isKicking", _isKicking);
    }

    private void CheckIfAttackFinished() {
        if (Time.time > _attackFinishedTimer) _animator.Play("Standing Idle", 0, 0f);
    }

    private void Attack() {
        if (Time.time > _attackTimer)
        {
            _attackFinishedTimer = Time.time + attackFinishedWaitTime;
            _attackTimer = Time.time + attackWaitTime;

            _animator.Play("Melee", 0, 0.25f);

            // Activate the damage area when attacking
            enemyDamageArea.gameObject.SetActive(true);
            enemyDamageArea.ResetDeactivateTimer();
        }
    }

    public void EnemyDied() {
        enemyDied = true;
        Destroy(gameObject);
    }
}
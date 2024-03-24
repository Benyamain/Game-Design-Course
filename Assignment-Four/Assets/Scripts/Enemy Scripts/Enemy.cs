using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private float meleeRange = 3f;
    private bool _isRunning;
    private bool _isRunningBackwards;
    private bool _isLeftStrafing;
    private bool _isRightStrafing;
    private bool _isMelee;
    // private bool _isKicking;

    [SerializeField]
    private float attackWaitTime = 2.5f;
    private float _attackTimer;
    [SerializeField]
    private float attackFinishedWaitTime = 0.5f;
    private float _attackFinishedTimer;

    [SerializeField]
    private EnemyDamageArea enemyDamageArea;
    private bool enemyDied;
    private NavMeshAgent enemyNavMeshAgent;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        enemyNavMeshAgent.destination = GameManager.Player.transform.position;

        if (GameManager.IsPlayerDead)
        {
            GameManager.GameOver();
            // Stop the agent from moving once player is dead
            enemyNavMeshAgent.isStopped = true;
            // Disable the script
            this.enabled = false;
        }

        if (enemyDied) return;

        if (GameManager.Player == null)
        {
            return;
        }

        // Check if the player is within arms reach for melee
        float distanceToPlayer = Vector3.Distance(enemyNavMeshAgent.transform.position, GameManager.Player.transform.position);

        if (transform.InverseTransformDirection(enemyNavMeshAgent.velocity).z > 0f)
        {
            _isRunning = true;
            _isRunningBackwards = _isRightStrafing = _isLeftStrafing = false;
        }

        if (transform.InverseTransformDirection(enemyNavMeshAgent.velocity).z < 0f)
        {
            _isRunningBackwards = true;
            _isRunning = _isRightStrafing = _isLeftStrafing = false;
        }

        if (transform.InverseTransformDirection(enemyNavMeshAgent.velocity).x < 0f)
        {
            _isLeftStrafing = true;
            _isRunningBackwards = _isRightStrafing = _isRunning = false;
        }

        if (transform.InverseTransformDirection(enemyNavMeshAgent.velocity).x > 0f)
        {
            _isRightStrafing = true;
            _isRunningBackwards = _isRunning = _isLeftStrafing = false;
        }

        if (distanceToPlayer <= meleeRange)
        {
            _isMelee = true;
            CheckIfAttackFinished();
            Attack();
        }
        // Update animator based on movement and attack states
        UpdateAnimator();
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

    private void CheckIfAttackFinished()
    {
        if (Time.time > _attackFinishedTimer) _animator.Play("Standing Idle", 0, 0f);
    }

    private void Attack()
    {
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

    public void EnemyDied()
    {
        if (enemyNavMeshAgent != null)
        {
            enemyNavMeshAgent.isStopped = true;
        }

        enemyDied = true;
        Destroy(gameObject);
    }
}
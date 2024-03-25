using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class PlayerShooting : MonoBehaviour
{
    [Tooltip("Where to start the ray from.")]
    [SerializeField]
    private Transform rayStart;

    [SerializeField]
    private float fadeOutSpeed = 10000f;

    [SerializeField]
    private float lineWidth = 0.045f;

    private LineRenderer _lr;
    private float _alpha = 1f;

    public float shootingRange = 100f;

    [SerializeField]
    public int damagePerShot = 1;

    public float shootingCooldown = 1f;

    private float lastShotTime = 0f;
    private GameObject nearestEnemy;
    private float minDistance;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        FindNearestEnemy();

        /* Shoot when the distance to be between the player and enemy is less than the shooting range, along with
        a shooting cooldown as well. */
        if (minDistance < shootingRange && Time.time - lastShotTime > shootingCooldown)
        {
            ShootAtNearestEnemy();
            lastShotTime = Time.time;
        }
    }

    private void FindNearestEnemy()
    {
        // Shoot the enemy even when the player is not directly facing them
        Vector3[] castDirections = { GameManager.PlayerNavMeshAgent.transform.forward, -GameManager.PlayerNavMeshAgent.transform.forward, GameManager.PlayerNavMeshAgent.transform.right, -GameManager.PlayerNavMeshAgent.transform.right };

        foreach (Vector3 direction in castDirections)
        {
            RaycastHit[] hits = Physics.SphereCastAll(GameManager.PlayerNavMeshAgent.transform.position, 1f, direction, shootingRange);
            nearestEnemy = null;
            minDistance = Mathf.Infinity;

            // Handle hits if needed
            foreach (RaycastHit hit in hits)
            {
                /* Find a hit with an enemy, but only register the collider that gets the closest enemy hit. */
                if (hit.collider.CompareTag("Enemy"))
                {
                    float distance = Vector3.Distance(GameManager.PlayerNavMeshAgent.transform.position, hit.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = hit.collider.gameObject;
                    }
                }
            }
        }
    }

    private void ShootAtNearestEnemy()
    {
        // Shoot into random place if enemy is not hit (most likely this will bias the former)
        Vector3 endPoint = nearestEnemy ? nearestEnemy.transform.position : rayStart.position + transform.forward * shootingRange;

        // Logic for line renderer
        SetLineRendererPositions(rayStart.position, endPoint);
        SetLineRendererWidth();
        FadeOutLineRenderer();

        if (nearestEnemy != null)
        {
            nearestEnemy.GetComponent<EnemyHealth>().TakeDamage(damagePerShot);
        }
    }

    private void SetLineRendererPositions(Vector3 start, Vector3 end)
    {
        _lr.positionCount = 2;
        _lr.SetPositions(new[] { start, end });
    }

    private void SetLineRendererWidth()
    {
        _lr.startWidth = lineWidth;
        _lr.endWidth = lineWidth;
    }

    private void FadeOutLineRenderer()
    {
        if (_alpha > 0f)
        {
            _alpha -= Time.deltaTime * fadeOutSpeed;
            Color lineColor = _lr.startColor;
            lineColor.a = _alpha;
            _lr.startColor = lineColor;
            _lr.endColor = lineColor;
        }
    }
}
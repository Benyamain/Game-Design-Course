using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private GameObject nearestEnemy;
    private float minDistance;
    private float lastShotTime = 0f;
    private float shootingCooldown = 2f;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        FindNearestEnemy();

         // Check if enough time has passed since the last shot
        if (Time.time - lastShotTime >= shootingCooldown)
        {
            /* Shoot when the distance to be between the player and enemy is less than the shooting range */
            if (minDistance < shootingRange)
            {
                ShootAtNearestEnemy();
                lastShotTime = Time.time;
            }
        }
    }

    private void FindNearestEnemy()
    {
        // Get all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Sort the enemies by distance to the player
        List<GameObject> sortedEnemies = new List<GameObject>(enemies);
        sortedEnemies.Sort((a, b) => Vector3.Distance(GameManager.PlayerNavMeshAgent.transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(GameManager.PlayerNavMeshAgent.transform.position, b.transform.position)));

        nearestEnemy = null;
        minDistance = Mathf.Infinity;

        // Line check from player to each enemy
        foreach (GameObject enemy in sortedEnemies)
        {
            Vector3 direction = enemy.transform.position - GameManager.PlayerNavMeshAgent.transform.position;
            RaycastHit hit;

            // Perform line check
            if (Physics.Raycast(GameManager.PlayerNavMeshAgent.transform.position, direction, out hit, shootingRange))
            {
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
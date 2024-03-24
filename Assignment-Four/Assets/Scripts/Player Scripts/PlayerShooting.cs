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

        if (minDistance < shootingRange && Time.time - lastShotTime > shootingCooldown)
        {
            ShootAtNearestEnemy();
            lastShotTime = Time.time;
        }
    }

    private void FindNearestEnemy()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, shootingRange);
        nearestEnemy = null;
        minDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hit.collider.gameObject;
                }
            }
        }
    }

    private void ShootAtNearestEnemy()
    {
        Vector3 endPoint = nearestEnemy ? nearestEnemy.transform.position : rayStart.position + transform.forward * shootingRange;

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
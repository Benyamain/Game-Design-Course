using UnityEngine;
using UnityEngine.InputSystem;
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
    private PlayerController playerController;

    private LineRenderer _lr;
    private float _alpha = 1f;

    [SerializeField]
    public float shootingRange = 15f;

    [SerializeField]
    public int damagePerShot = 1;

    [SerializeField]
    public float shootingCooldown = 1f;

    private bool canShoot = true;
    private float lastShotTime = 0f;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            ClearLineRenderer();
            return;
        }

        if (Time.time - lastShotTime > shootingCooldown && playerController.canShoot && !playerController.isRunning && !playerController.isRunningBackwards)
        {
            Ray ray = GameManager.MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ShootAtNearestEnemy(ray);
            lastShotTime = Time.time;
        }
    }

    private void ShootAtNearestEnemy(Ray ray)
    {
        playerController.canShoot = false;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, shootingRange);
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 endPoint = rayStart.position + ray.direction * 1000;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hit.collider.gameObject;
                    endPoint = hit.point;
                }
            }
        }

        SetLineRendererPositions(rayStart.position, endPoint);
        SetLineRendererWidth();
        FadeOutLineRenderer();

        if (nearestEnemy != null)
        {
            GameManager.EnemyTakeDamage(damagePerShot);
        }

        Invoke(nameof(ResetShootingCooldown), shootingCooldown);
    }

    private void ResetShootingCooldown()
    {
        playerController.canShoot = true;
    }

    private void ClearLineRenderer()
    {
        _lr.positionCount = 0;
        _alpha = 1f;
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
        else
        {
            ClearLineRenderer();
        }
    }
}
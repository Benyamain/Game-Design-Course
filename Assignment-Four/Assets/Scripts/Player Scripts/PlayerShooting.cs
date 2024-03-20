using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float shootingRange = 15f;
    public int damagePerShot = 1;
    public float shootingCooldown = 1f;

    bool canShoot = true;

    private void Update()
    {
        if (canShoot)
        {
            ShootAtNearestEnemy();
        }
    }

    private void ShootAtNearestEnemy()
    {
        canShoot = false;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, shootingRange);
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

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

        if (nearestEnemy != null)
        {
            nearestEnemy.GetComponent<EHealth>().TakeDamage(damagePerShot);
        }

        Invoke(nameof(ResetShootingCooldown), shootingCooldown);
    }

    private void ResetShootingCooldown()
    {
        canShoot = true;
    }
}
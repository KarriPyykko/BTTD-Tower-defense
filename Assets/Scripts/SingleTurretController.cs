using UnityEngine;

public class SingleTurretController : AreaTurretController
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float step = 200f;

    GameObject closestTarget;
    float distanceToTarget;
    float minDistance;
    Vector2 shootDirection;

    void Start()
    {
        InvokeRepeating("DealDamage", 0f, attackInterval);
        closestTarget = null;
        minDistance = Mathf.Infinity;
        sprite = GetComponent<SpriteRenderer>();
        originalDamage = damage;

        BuffCheck();
    }

    private void Update()
    {
        // Follow target and flip sprite based on enemy position
        if (closestTarget != null)
        {
            if (closestTarget.transform.position.x < transform.position.x)
            {
               sprite.flipX = true;

                float angle = Vector2.SignedAngle(transform.right * -1, closestTarget.transform.position - transform.position);

                transform.eulerAngles += new Vector3(0f, 0f, step * (angle < 0f ? -1f : 1f) * Time.deltaTime);
            }

            else
            {
                sprite.flipX = false;

                float angle = Vector2.SignedAngle(transform.right, closestTarget.transform.position - transform.position);

                transform.eulerAngles += new Vector3(0f, 0f, step * (angle < 0f ? -1f : 1f) * Time.deltaTime);
            }
        }
    }

    void DealDamage()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (var enemy in enemies)
        {
            if ((distanceToTarget = Vector2.Distance(enemy.transform.position, transform.position)) <= attackRange)
            {
                if (distanceToTarget < minDistance)
                {
                    closestTarget = enemy;
                }
            }
        }

        if (closestTarget != null)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        ProjectileController controller = projectile.GetComponent<ProjectileController>();

        // Calculate direction for projectile
        shootDirection = closestTarget.transform.position - transform.position;
        shootDirection.Normalize();

        controller.Launch(shootDirection, projectileSpeed, damage);
    }
}

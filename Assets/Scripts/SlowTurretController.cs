using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurretController : AreaTurretController
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float step = 200f;

    GameObject closestTarget;
    float distanceToTarget;
    float minDistance;

    void Start()
    {
        InvokeRepeating("DealDamage", 0f, attackInterval);
        closestTarget = null;
        minDistance = Mathf.Infinity;
        sprite = GetComponent<SpriteRenderer>();
        originalDamage = damage;

        BuffCheck();
    }

    // Follow target and flip sprite based on enemy position
    void Update()
    {
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
            ShootHoming();
        }
    }

    void ShootHoming()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        HomingProjectileController controller = projectile.GetComponent<HomingProjectileController>();

        controller.SetTarget(closestTarget, damage);
    }
}

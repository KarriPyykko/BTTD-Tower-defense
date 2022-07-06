using UnityEngine;

public class HomingProjectileController : ProjectileController
{
    public float stepSize = 10f;
    public float slowPercentage = 0.5f;
    public float slowDuration = 3f;
    public float radius = 5f;

    GameObject enemy = null;
    Vector3 direction;

    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }

        FollowTarget();
    }

    private void FollowTarget()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate direction vector and move in the vector's direction
        direction = enemy.transform.position - transform.position;

        // smoother but less accurate
        //transform.Translate(direction.normalized * stepSize * Time.deltaTime);

        transform.position = transform.position + direction.normalized * stepSize * Time.deltaTime;
    }

    public void SetTarget(GameObject target, float _damage)
    {
        enemy = target;
        damage = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("enemy"))
        {
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        // Splash damage and slow
        foreach (GameObject enemy in enemies)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) <= radius)
            {
                EnemyController controller = enemy.GetComponent<EnemyController>();
                controller.DamageEnemy(damage);
                controller.SlowEnemy(slowPercentage, slowDuration);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

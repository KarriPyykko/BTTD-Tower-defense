using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    public float timeToLive = 10f;
    public GameObject singleTower;

    protected float damage;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Add force to projectile rigidbody
    public void Launch(Vector2 direction, float force, float _damage)
    {
        rb2d.AddForce(direction * force);
        damage = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController controller = collision.gameObject.GetComponent<EnemyController>();

        if (controller != null)
        {
            controller.DamageEnemy(damage);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaTurretController : MonoBehaviour
{
    public float attackInterval = 0.5f;
    public float damage = 10f;
    public float attackRange = 5f;
    public int cost = 50;
    public int upgradeCost = 100;
    public Text description;
    public GameObject buffTower;
    public int upgradeLevel = 1;
    protected int bufferCount = 0;
    protected float originalDamage;
    public float damageMultiplier = 1;

    protected float previousDamage = 0;
    float animationTimer = 1f;
    Animator anim;
    public SpriteRenderer sprite;

    void Start()
    {
        InvokeRepeating("DealDamage", 0f, attackInterval);
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        originalDamage = damage;
        previousDamage = damageMultiplier;

        BuffCheck();
    }

    void Update()
    {
        if (anim == null)
        {
            return;
        }

        animationTimer -= Time.deltaTime;

        if (animationTimer <= 0)
        {
            anim.SetTrigger("AnimationOver");
        }
    }

    void DealDamage()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (var enemy in enemies)
        {
            EnemyController controller = enemy.GetComponent<EnemyController>();

            if (Vector2.Distance(enemy.transform.position, transform.position) <= attackRange)
            {
                controller.DamageEnemy(damage * damageMultiplier);

                if (anim != null)
                {
                    anim.SetTrigger("Attack");
                    animationTimer = 1f;
                }
            }
        }
    }

    public void Upgrade()
    {
        if (upgradeLevel < 5)
        {
            upgradeLevel++;
            upgradeCost += Mathf.RoundToInt(upgradeCost * 0.3f * upgradeLevel);
            damage = CalculateDamage(damage, upgradeLevel);
            sprite.color = new Color(Mathf.Clamp(upgradeLevel * 0.2f, 0, 1), Mathf.Clamp(upgradeLevel * 0.2f, 0, 1), 0, 1);
        }
    }

    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }

    public void BoostDamage(float boostPercentage)
    {
        damageMultiplier *= boostPercentage;
    }

    public void ReduceDamage(float boostPercentage)
    {
        damageMultiplier /= boostPercentage;
    }

    public void BuffCheck()
    {
        GameObject[] buffers = GameObject.FindGameObjectsWithTag("Buffer");

        foreach (var buffer in buffers)
        {
            if (Vector2.Distance(buffer.transform.position, transform.position) <= buffer.GetComponent<AreaTurretController>().attackRange)
            {
                damageMultiplier *= buffer.GetComponent<AreaTurretController>().damage;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public float CalculateDamage(float _damage, int _upgradeLevel)
    {
        return _damage += _damage * 0.2f * _upgradeLevel;
    }

    public void SetDescriptionCost()
    {
        description.text += "\n\nCost: <b>" + cost + "</b>";
    }
}

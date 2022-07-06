using System.Collections.Generic;
using UnityEngine;

public class BuffTurretController : AreaTurretController
{
    List<float> damages = new List<float>();

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        ApplyBuff();
        damages.Add(damage);
    }

    void Update()
    {
        
    }

    // Apply buffs to every tower within range
    void ApplyBuff()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject tower in towers)
        {
            AreaTurretController controller = tower.GetComponent<AreaTurretController>();

            if (Vector2.Distance(tower.transform.position, transform.position) <= attackRange)
            {
                controller.BoostDamage(damage);
            }
        }
    }

    // Adding new buff percentage to a list and applying buffs on upgrade
    public new void Upgrade()
    {
        if (upgradeLevel < 5)
        {
            upgradeLevel++;
            upgradeCost += Mathf.RoundToInt(upgradeCost * 0.3f * upgradeLevel);
            previousDamage = damage;
            damage = CalculateDamage(damage, upgradeLevel);
            damages.Add(damage);
            sprite.color = new Color(Mathf.Clamp(upgradeLevel * 0.2f, 0, 1), Mathf.Clamp(upgradeLevel * 0.2f, 0, 1), 0, 1);

            ApplyBuff();
        }
    }

    public new float CalculateDamage(float _damage, int _upgradeLevel)
    {
        return _damage += Mathf.Log10(_damage * _upgradeLevel) * 0.3f;
    }

    // Removing buffs when tower is sold
    public void RemoveBuff()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject tower in towers)
        {
            AreaTurretController controller = tower.GetComponent<AreaTurretController>();

            if (Vector2.Distance(tower.transform.position, transform.position) <= attackRange)
            {
                for (int i = damages.Count - 1; i >= 0; i--)
                {
                    controller.ReduceDamage(damages[i]);
                }
            }
        }
    }
}

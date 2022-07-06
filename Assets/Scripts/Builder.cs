using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Builder : MonoBehaviour
{
    public static Builder instance;
    public GameObject defaultTower;
    public Text moneyText;
    GameObject tower;
    public GameObject towerToOperate = null;
    public Tile tileController;
    public Text description;
    public int money = 50;
    public AudioClip sellSound;
    public AudioClip errorSound;

    AudioSource sound;

    void Awake()
    {
        instance = this;
        tower = defaultTower;

        sound = GetComponent<AudioSource>();
    }

    public GameObject GetTower()
    {
        return tower;
    }

    public void SetMoney(int amount)
    {
        money += amount;

        moneyText.text = money.ToString();
    }

    public void SetTower(GameObject selectedTower)
    {
        tower = selectedTower;
    }

    public void SetTowerToOperate(GameObject _tower)
    {
        towerToOperate = _tower;
    }
    public void SellTower()
    {
        if (towerToOperate == null)
        {
            return;
        }

        // Remove buffs if the tower is a buff tower
        if (towerToOperate.CompareTag("Buffer"))
        {
            towerToOperate.GetComponent<BuffTurretController>().RemoveBuff();
        }
        SetMoney(towerToOperate.GetComponent<AreaTurretController>().cost);
        Destroy(towerToOperate);
        tileController.tower = null;

        sound.PlayOneShot(sellSound);
    }

    public void UpgradeTower()
    {
        // Upgrade buffer
        if (towerToOperate.CompareTag("Buffer"))
        {
            BuffTurretController towerController = towerToOperate.GetComponent<BuffTurretController>();

            if ((money >= towerController.upgradeCost) && (towerController.upgradeLevel < 5))
            {

                SetMoney(-towerController.upgradeCost);
                towerController.Upgrade();

                float damage = towerController.damage;
                float multiplier = towerController.damageMultiplier;
                int upgradeLevel = towerController.GetUpgradeLevel();

                description.text = "Tower level: <b>" + upgradeLevel +
                    "</b>\n\nDamage: <b>" + damage + "</b>\n\nBuffed damage: <b>" + damage * multiplier + "</b>\n\nUpgrade cost: <b>" +
                    towerController.upgradeCost + "</b>\n\n\n\nDamage next level: <b>" + towerController.CalculateDamage(damage, upgradeLevel + 1) + "</b>";
            }
            else
            {
                sound.PlayOneShot(errorSound);
            }
        }
        // Upgrade other
        else
        {
            AreaTurretController towerController = towerToOperate.GetComponent<AreaTurretController>();

            if ((money >= towerController.upgradeCost) && (towerController.upgradeLevel < 5))
            {

                SetMoney(-towerController.upgradeCost);
                towerController.Upgrade();

                float damage = towerController.damage;
                float multiplier = towerController.damageMultiplier;
                int upgradeLevel = towerController.GetUpgradeLevel();

                description.text = "Tower level: <b>" + upgradeLevel +
                    "</b>\n\nDamage: <b>" + damage + "</b>\n\nBuffed damage: <b>" + damage * multiplier + "</b>\n\nUpgrade cost: <b>" +
                    towerController.upgradeCost + "</b>\n\n\n\nDamage next level: <b>" + towerController.CalculateDamage(damage, upgradeLevel + 1) + "</b>";
            }
            else
            {
                sound.PlayOneShot(errorSound);
            }
        }
    }
}

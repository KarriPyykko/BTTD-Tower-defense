using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public GameObject tower;
    public GameObject shopButtons;
    public Text description;
    public Button button;
    public AudioClip towerPlacement;
    public AudioClip errorMessage;
    public AudioClip sellSound;

    AudioSource sound;
    SpriteRenderer sprite;
    Color originalColor;
    GameObject instantiatedTower;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        Builder.instance.SetMoney(0);
    }

    void Update()
    {
        // Disable script if game is over
        if (SceneHandler.gameOver)
        {
            enabled = false;
            return;
        }
    }

    private void OnMouseEnter()
    {
        Color tmpColor = originalColor;
        tmpColor.a = 0.2f;
        sprite.color = tmpColor;
    }

    private void OnMouseExit()
    {
        sprite.color = originalColor;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // check if tile is occupied
        if (tower != null)
        {
            AreaTurretController towerController = instantiatedTower.GetComponent<AreaTurretController>();
            float damage = towerController.damage;
            float multiplier = towerController.damageMultiplier;
            int upgradeLevel = towerController.GetUpgradeLevel();

            description.text = "Tower level: <b>" + upgradeLevel +
                "</b>\n\nDamage: <b>" + damage + "</b>\n\nBuffed damage: <b>" + damage * multiplier + "</b>\n\nUpgrade cost: <b>" + 
                towerController.upgradeCost + "</b>\n\n\n\nDamage next level: <b>" + towerController.CalculateDamage(damage, upgradeLevel+1) + "</b>";
            shopButtons.SetActive(true);

            if (instantiatedTower != null)
            {
                Builder.instance.towerToOperate = instantiatedTower;
                Builder.instance.tileController = this;
            }

            return;
        }

        shopButtons.SetActive(false);

        int availableMoney = Builder.instance.money;
        int towerCost = Builder.instance.GetTower().GetComponent<AreaTurretController>().cost;

        // Buy tower
        if (availableMoney >= towerCost)
        {
            tower = Builder.instance.GetTower();

            instantiatedTower = Instantiate(tower, transform.position, transform.rotation);
            Builder.instance.SetMoney(-towerCost);
            sound.PlayOneShot(towerPlacement);
        }
        else
        {
            sound.PlayOneShot(errorMessage);
        }
    }

    public void SellTower()
    {
        if (tower == null)
        {
            return;
        }

        AreaTurretController controller = tower.GetComponent<AreaTurretController>();

        int towerCost = controller.cost;
        Builder.instance.SetMoney(towerCost);

        // Remove buffs when needed
        if (tower.CompareTag("Buffer"))
        {
            BuffTurretController buffController = instantiatedTower.GetComponent<BuffTurretController>();

            buffController.RemoveBuff();
        }

        tower = null;
        Destroy(instantiatedTower);
    }

    private void OnMouseOver()
    {
        if (tower != null && Input.GetKeyDown(KeyCode.S))
        {
            SellTower();
            sound.PlayOneShot(sellSound);
        }
    }
}

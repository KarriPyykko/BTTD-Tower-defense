using UnityEngine;
using UnityEngine.UI;

public class TowerSelect : MonoBehaviour
{
    public GameObject singleTower;
    public GameObject areaTower;
    public GameObject buffTower;
    public GameObject slowTower;
    public GameObject shopButtons;

    public Text description;

    // Tower menu functionality
    public void SelectSingleTower()
    {
        Builder.instance.SetTower(singleTower);
        shopButtons.SetActive(false);

        description.text = "<b>Staff</b>\n\n" + description.text + "\n\nCost: <b>" + singleTower.GetComponent<AreaTurretController>().cost +"</b>";
    }

    public void SelectAreaTower()
    {
        Builder.instance.SetTower(areaTower);
        shopButtons.SetActive(false);

        description.text = "<b>Mod</b>\n\n" + description.text + "\n\nCost: <b>" + areaTower.GetComponent<AreaTurretController>().cost + "</b>";
    }

    public void SelectBuffTower()
    {
        Builder.instance.SetTower(buffTower);
        shopButtons.SetActive(false);

        description.text = "<b>Admin</b>\n\n" + description.text + "\n\nCost: <b>" + buffTower.GetComponent<AreaTurretController>().cost + "</b>";
    }

    public void SelectSlowTower()
    {
        Builder.instance.SetTower(slowTower);
        shopButtons.SetActive(false);

        description.text = "<b>Slow Mode</b>\n\n" + description.text + "\n\nCost: <b>" + slowTower.GetComponent<AreaTurretController>().cost + "</b>";
    }
}

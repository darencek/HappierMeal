using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradesPanel : MonoBehaviour
{
    public GameObject[] upgradeWindow_Panels;

    public GameObject UI_gardeningButton;

    public UI_UpgradeButton upgradeButton_dreamMachine;
    public UI_UpgradeButton upgradeButton_factory;
    public UI_UpgradeButton upgradeButton_bakery;
    public UI_UpgradeButton upgradeButton_refinery;
    public UI_UpgradeButton upgradeButton_foundry;

    public UI_UpgradeButton upgradeButton_dreamEngine;
    public UI_UpgradeButton upgradeButton_fishery;
    public UI_UpgradeButton upgradeButton_crystarium;

    public UI_UpgradeButton upgradeButton_grow;
    public UI_UpgradeButton upgradeButton_yield;
    public UI_UpgradeButton upgradeButton_crossbreed;

    // Start is called before the first frame update
    void Start()
    {
        upgradeButton_dreamMachine.upgrade = UpgradeManager.dreamMachine_efficiency;
        upgradeButton_factory.upgrade = UpgradeManager.factory_efficiency;
        upgradeButton_bakery.upgrade = UpgradeManager.bakery_efficiency;
        upgradeButton_refinery.upgrade = UpgradeManager.refinery_efficiency;
        upgradeButton_foundry.upgrade = UpgradeManager.foundry_efficiency;

        upgradeButton_dreamEngine.upgrade = UpgradeManager.dreamEngine_upgrade;
        upgradeButton_fishery.upgrade = UpgradeManager.fishery_upgrade;
        upgradeButton_crystarium.upgrade = UpgradeManager.crystarium_upgrade;

        upgradeButton_grow.upgrade = UpgradeManager.farm_grow_upgrade;
        upgradeButton_yield.upgrade = UpgradeManager.farm_yield_upgrade;
        upgradeButton_crossbreed.upgrade = UpgradeManager.farm_crossbreed_upgrade;
    }

    private void OnEnable()
    {
        UI_gardeningButton.SetActive(MainManager.instance.CountBuildingsOfType(Building.BuildingType.GARDEN_SHED) > 0);
        if (!UI_gardeningButton.activeInHierarchy && upgradeWindow_Panels[2].activeInHierarchy) UI_UpgradeSwitchPanel(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UI_Close()
    {
        gameObject.SetActive(false);
    }

    public void UI_UpgradeSwitchPanel(int panel)
    {
        for (int i = 0; i < upgradeWindow_Panels.Length; i++)
        {
            upgradeWindow_Panels[i].SetActive(i == panel);
        }
    }
}

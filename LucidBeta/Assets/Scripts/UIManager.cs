using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI zeeText;
    public TextMeshProUGUI restText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI zeeRateText;

    public TextMeshProUGUI energyCostsText;

    public TextMeshProUGUI sleepHoursText;

    public GameObject statsMiniPopup;
    public GameObject statsMiniPopup_rest;
    public GameObject statsMiniPopup_energy;

    public GameObject buildPanel;
    public GameObject sleepPanel;
    public GameObject wakePanel;
    public GameObject buildingInfoPanel;
    public GameObject upgradePanel;
    public GameObject farmPanel;
    public GameObject ascendPanel;
    public GameObject dialogBox;
    public GameObject newCreaturePopup;
    public GameObject newAscensionPopup;
    public GameObject tutorialMain;

    public GameObject button_upgrade;

    public GameObject UIBlocker;

    public bool blockUI = false;

    Building selectedBuilding;

    int statsMiniPopupCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.uiManager = this;

        if (MainManager.instance.playTutorial)
        {
            tutorialMain.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UIBlocker.SetActive(blockUI);

        zeeText.text = "$" + ((int)System.Math.Floor(MainManager.instance.zees)).ToString("N0");
        zeeRateText.text = "$" + MainManager.FormatMoney(MainManager.instance.zee_earningPerMinute * 60f) + "/" + MainManager.FormatMoney(MainManager.instance.zees_earnLimit) + " per hour";

        restText.text = MainManager.FormatNumber(MainManager.instance.rest_resource) + "/" + MainManager.FormatNumber(MainManager.instance.rest_limit);
        energyText.text = MainManager.FormatNumber(MainManager.instance.energy_resource) + "/" + MainManager.FormatNumber(MainManager.instance.energy_limit);

        sleepHoursText.text = "Slept for " + Mathf.FloorToInt(MainManager.instance.sleepTime / (60f * 60f)) + " hours...";

        energyCostsText.text = "Current costs: " + MainManager.instance.energy_upkeep + " / min";

        button_upgrade.SetActive(MainManager.instance.buildings_haveWorkshop);
    }

    public void UI_ShowStatsMiniPopup(int i)
    {
        statsMiniPopup.SetActive(!statsMiniPopup.activeInHierarchy || statsMiniPopupCurrent != i);
        statsMiniPopup_rest.SetActive(i == 1);
        statsMiniPopup_energy.SetActive(i == 2);

        statsMiniPopupCurrent = statsMiniPopup.activeInHierarchy ? i : 0;
    }

    public void UI_SleepButton()
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        dialogBox.SetActive(false);
        upgradePanel.SetActive(false);
        ascendPanel.SetActive(false);
        sleepPanel.SetActive(true);

        MainManager.instance.StartSleeping();
    }

    public void UI_WakeUpButton()
    {
        sleepPanel.SetActive(false);
        MainManager.instance.StopSleeping();
        wakePanel.SetActive(true);
        wakePanel.GetComponent<UI_WakePanel>().StartWakeUp(MainManager.instance.sleepTime);
    }

    public void UI_ShowBuildingPanel(Building selected)
    {
        ascendPanel.SetActive(false);
        farmPanel.SetActive(false);
        buildPanel.SetActive(true);
        dialogBox.SetActive(false);
        upgradePanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        selectedBuilding = (selected);
    }

    public void UI_ShowFarmPanel(FarmPlot f)
    {
        ascendPanel.SetActive(false);
        farmPanel.SetActive(true);
        buildPanel.SetActive(false);
        dialogBox.SetActive(false);
        upgradePanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        farmPanel.GetComponent<UI_FarmPanel>().OpenPanel(f);
    }

    public void UI_ShowAscendPanel()
    {
        ascendPanel.SetActive(true);
        farmPanel.SetActive(false);
        buildPanel.SetActive(false);
        dialogBox.SetActive(false);
        upgradePanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
    }
    public void UI_BuildButton()
    {
        buildPanel.SetActive(!buildPanel.activeInHierarchy);
        buildingInfoPanel.SetActive(false);
    }

    public void UI_BuildBuilding(Building.BuildingType buildType)
    {
        MainManager.instance.BuildBuilding(selectedBuilding, buildType);
    }

    public void UI_ShowBuildingInfoPanel(Building b)
    {
        selectedBuilding = b;
        buildPanel.SetActive(false);
        dialogBox.SetActive(false);
        upgradePanel.SetActive(false);
        farmPanel.SetActive(false);
        ascendPanel.SetActive(false);
        buildingInfoPanel.SetActive(true);
        buildingInfoPanel.GetComponent<UI_BuildingInfoPanel>().SelectBuilding(selectedBuilding);
    }

    public void UI_ShowUpgradePanel()
    {
        buildPanel.SetActive(false);
        dialogBox.SetActive(false);
        buildingInfoPanel.SetActive(false);
        farmPanel.SetActive(false);
        ascendPanel.SetActive(false);
        upgradePanel.SetActive(true);
    }

    public void UI_ShowDialogBox(CreatureController c)
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        upgradePanel.SetActive(false);
        farmPanel.SetActive(false);
        ascendPanel.SetActive(false);
        dialogBox.SetActive(true);
        dialogBox.GetComponent<UI_DialogBox>().OpenDialog(c);
    }

    public void UI_ShowNewCreaturePopup(CreatureController c)
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        upgradePanel.SetActive(false);
        dialogBox.SetActive(false);
        farmPanel.SetActive(false);
        ascendPanel.SetActive(false);
        newCreaturePopup.SetActive(true);
        newCreaturePopup.GetComponent<UI_NewCreaturePopup>().OpenPopup(c);
    }
}

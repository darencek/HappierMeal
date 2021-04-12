using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject settingsMenu;
    public GameObject credits;

    public GameObject button_upgrade;

    public UI_FarmPanel farmUI;

    public GameObject UIBlocker;
    public GameObject FadeIn;

    public bool blockUI = false;

    Building selectedBuilding;

    int statsMiniPopupCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        FadeIn.SetActive(true);

        MainManager.uiManager = this;

        if (MainManager.instance.playTutorial)
        {
            tutorialMain.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeIn.activeInHierarchy)
        {
            Color c = FadeIn.GetComponent<Image>().color;
            c.a = Mathf.MoveTowards(c.a, 0, Time.deltaTime);
            FadeIn.GetComponent<Image>().color = c;
            if (c.a <= 0)
                FadeIn.SetActive(false);
        }

        UIBlocker.SetActive(blockUI);

        zeeText.text = "$" + ((int)System.Math.Floor(MainManager.instance.zees)).ToString("N0");
        zeeRateText.text = "$" + MainManager.FormatMoney(MainManager.instance.zee_earningPerMinute * 60f) + "/" + MainManager.FormatMoney(MainManager.instance.zees_earnLimit) + " per hour";

        restText.text = MainManager.FormatNumber(MainManager.instance.rest_resource) + "/" + MainManager.FormatNumber(MainManager.instance.rest_limit);
        energyText.text = MainManager.FormatNumber(MainManager.instance.energy_resource) + "/" + MainManager.FormatNumber(MainManager.instance.energy_limit);

        sleepHoursText.text = "Slept for " + Mathf.FloorToInt(MainManager.instance.sleepTime / (60f * 60f)) + " hours...";

        energyCostsText.text = "Current costs: " + (MainManager.instance.energy_upkeep * 60) + " / hour";

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
        MainManager.musicManager.PlayClick();

        settingsMenu.SetActive(false);
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        dialogBox.SetActive(false);
        upgradePanel.SetActive(false);
        ascendPanel.SetActive(false);
        sleepPanel.SetActive(true);

        sleepPanel.GetComponent<Animator>().SetBool("Sleep", true);
        MainManager.instance.StartSleeping();
    }

    public void UI_WakeUpButton()
    {
        MainManager.musicManager.PlayClick();

        StartCoroutine("WakeupCoroutine");
    }

    IEnumerator WakeupCoroutine()
    {
        sleepPanel.GetComponent<Animator>().SetBool("Sleep", false);
        yield return new WaitForSecondsRealtime(0.5f);
        sleepPanel.SetActive(false);
        MainManager.instance.StopSleeping();
        MainManager.instance.CompleteWakeUp();
    }

    public void UI_ShowBuildingPanel(Building selected)
    {
        MainManager.musicManager.PlayClick();

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
        MainManager.musicManager.PlayClick();

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
        MainManager.musicManager.PlayClick();

        ascendPanel.SetActive(true);
        farmPanel.SetActive(false);
        buildPanel.SetActive(false);
        dialogBox.SetActive(false);
        upgradePanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
    }
    public void UI_BuildButton()
    {
        MainManager.musicManager.PlayClick();

        buildPanel.SetActive(!buildPanel.activeInHierarchy);
        buildingInfoPanel.SetActive(false);
    }

    public void UI_BuildBuilding(Building.BuildingType buildType)
    {
        MainManager.instance.BuildBuilding(selectedBuilding, buildType);
    }

    public void UI_ShowBuildingInfoPanel(Building b)
    {
        MainManager.musicManager.PlayClick();

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
        MainManager.musicManager.PlayClick();

        buildPanel.SetActive(false);
        dialogBox.SetActive(false);
        buildingInfoPanel.SetActive(false);
        farmPanel.SetActive(false);
        ascendPanel.SetActive(false);
        upgradePanel.SetActive(true);
    }

    public void UI_ShowDialogBox(CreatureController c)
    {
        MainManager.musicManager.PlayClick();

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
        MainManager.musicManager.PlayJingle();

        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        upgradePanel.SetActive(false);
        dialogBox.SetActive(false);
        farmPanel.SetActive(false);
        ascendPanel.SetActive(false);
        newCreaturePopup.SetActive(true);
        newCreaturePopup.GetComponent<UI_NewCreaturePopup>().OpenPopup(c);
    }

    public void UI_CrossbreedPopup(Crop.CropInfo info)
    {
        farmUI.UI_ShowCrossbreedPopup(info);
    }

    public void UI_SettingsButton()
    {
        MainManager.musicManager.PlayClick();

        settingsMenu.SetActive(!settingsMenu.activeInHierarchy);
    }

    public void UI_Settings_Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UI_Settings_Credits()
    {
        MainManager.musicManager.PlayClick();
        credits.SetActive(true);
    }

    public void UI_Settings_LoadDemoSave()
    {
        MainManager.musicManager.PlayClick();

        MainManager.instance.ascensionLevel = 3;
        GameObject[] bs = GameObject.FindGameObjectsWithTag("Building");

        bs[0].GetComponent<Building>().type = Building.BuildingType.DREAM_ENGINE;
        bs[1].GetComponent<Building>().type = Building.BuildingType.DREAM_MACHINE;
        bs[2].GetComponent<Building>().type = Building.BuildingType.WORKSHOP;
        bs[3].GetComponent<Building>().type = Building.BuildingType.LARGE_FARM;
        bs[4].GetComponent<Building>().type = Building.BuildingType.BAKERY;
        bs[5].GetComponent<Building>().type = Building.BuildingType.CRYSTARIUM;
        bs[6].GetComponent<Building>().type = Building.BuildingType.GARDEN_SHED;
        bs[7].GetComponent<Building>().type = Building.BuildingType.REFINERY;
        bs[8].GetComponent<Building>().type = Building.BuildingType.FOUNDRY;
        bs[9].GetComponent<Building>().type = Building.BuildingType.FACTORY;
        bs[10].GetComponent<Building>().type = Building.BuildingType.FISHERY;
        bs[11].GetComponent<Building>().type = Building.BuildingType.FRUIT_TREE;

        UpgradeManager.bakery_efficiency.level = 12;
        UpgradeManager.fishery_upgrade.level = 9;
        UpgradeManager.dreamMachine_efficiency.level = 15;
        UpgradeManager.dreamEngine_upgrade.level = 8;
        UpgradeManager.crystarium_upgrade.level = 5;
        UpgradeManager.refinery_efficiency.level = 10;

        MainManager.instance.rest_resource = 10000000;
        MainManager.instance.energy_resource = 10000000;

        MainManager.instance.zees = 10000000;
    }

}

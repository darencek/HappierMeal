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

    public TextMeshProUGUI sleepHoursText;

    public GameObject buildPanel;
    public GameObject sleepPanel;
    public GameObject wakePanel;
    public GameObject buildingInfoPanel;
    public GameObject dialogBox;
    public GameObject newCreaturePopup;

    Building selectedBuilding;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.uiManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        zeeText.text = "$" + (int)System.Math.Floor(MainManager.instance.zees);
        zeeRateText.text = "$" + (int)System.Math.Floor(MainManager.instance.zee_earningPerMinute * 60f) + "/" + MainManager.instance.zees_earnLimit + " per hour of sleep";

        restText.text = Mathf.FloorToInt(MainManager.instance.rest_resource) + "/" + Mathf.FloorToInt(MainManager.instance.rest_limit);
        energyText.text = Mathf.FloorToInt(MainManager.instance.energy_resource) + "/" + Mathf.FloorToInt(MainManager.instance.energy_limit);
        sleepHoursText.text = "Slept for " + Mathf.FloorToInt(MainManager.instance.sleepTime / (60f * 60f)) + " hours...";
    }

    public void UI_SleepButton()
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        dialogBox.SetActive(false);
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
        buildPanel.SetActive(true);
        dialogBox.SetActive(false);
        buildingInfoPanel.SetActive(false);
        selectedBuilding = (selected);
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
        buildingInfoPanel.SetActive(true);
        buildingInfoPanel.GetComponent<UI_BuildingInfoPanel>().SelectBuilding(selectedBuilding);
    }

    public void UI_ShowDialogBox(CreatureController c)
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        dialogBox.SetActive(true);
        dialogBox.GetComponent<UI_DialogBox>().OpenDialog(c);
    }

    public void UI_ShowNewCreaturePopup(CreatureController c)
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
        dialogBox.SetActive(false);
        newCreaturePopup.SetActive(true);
        newCreaturePopup.GetComponent<UI_NewCreaturePopup>().OpenPopup(c);
    }
}

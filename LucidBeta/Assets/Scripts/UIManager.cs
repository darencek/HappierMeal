using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI zeeText;
    public TextMeshProUGUI sleepHoursText;

    public GameObject buildPanel;
    public GameObject sleepPanel;
    public GameObject wakePanel;
    public GameObject buildingInfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.uiManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        zeeText.text = "$" + (int)System.Math.Floor(MainManager.instance.zees) +
            "\n($" + (int)System.Math.Floor(MainManager.instance.zee_earningPerMinute * 60f) + " per hour / " + MainManager.instance.zees_earnLimit + " Limit)" +
              "\nRest: " + Mathf.FloorToInt(MainManager.instance.rest_resource) + "/" + Mathf.FloorToInt(MainManager.instance.rest_limit) +
              "\nEnergy:" + Mathf.FloorToInt(MainManager.instance.energy_resource) + "/" + Mathf.FloorToInt(MainManager.instance.energy_limit);
        sleepHoursText.text = "Slept for " + Mathf.FloorToInt(MainManager.instance.sleepTime / (60f * 60f)) + " hours...";
    }

    public void UI_SleepButton()
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(false);
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

    public void UI_BuildButton()
    {
        buildPanel.SetActive(!buildPanel.activeInHierarchy);
        buildingInfoPanel.SetActive(false);
    }

    public void UI_BuildBuilding(Building.BuildingType buildType)
    {
        float price = Building.GetPrice(buildType);

        if (MainManager.instance.zees >= price)
        {
            MainManager.instance.zees -= price;

            buildPanel.SetActive(false);
            MainManager.instance.BuildBuilding(buildType);
        }
    }

    public void UI_ShowBuildingInfoPanel(Building b)
    {
        buildPanel.SetActive(false);
        buildingInfoPanel.SetActive(true);
        buildingInfoPanel.GetComponent<UI_BuildingInfoPanel>().SelectBuilding(b);
    }
}

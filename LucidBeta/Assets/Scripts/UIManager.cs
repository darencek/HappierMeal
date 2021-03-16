using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI zeeText;

    public GameObject buildPanel;
    public GameObject sleepPanel;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.uiManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        zeeText.text = "$" + (int)System.Math.Floor(MainManager.instance.zees) + "\nRest: " + Mathf.FloorToInt(MainManager.instance.rest_resource) + "\nEnergy:" + Mathf.FloorToInt(MainManager.instance.energy_resource) + "/" + Mathf.FloorToInt(MainManager.instance.energy_max);
    }

    public void UI_SleepButton()
    {
        buildPanel.SetActive(false);
        sleepPanel.SetActive(true);
        MainManager.instance.startSleeping();
    }

    public void UI_WakeUpButton()
    {
        sleepPanel.SetActive(false);
        MainManager.instance.stopSleeping();
    }

    public void UI_BuildButton()
    {
        buildPanel.SetActive(!buildPanel.activeInHierarchy);
    }

    public void UI_BuildBuilding(int type)
    {
        Building.BuildingType buildType = (Building.BuildingType)type;

        float price = Building.GetPrice(buildType);

        if (MainManager.instance.zees >= price)
        {
            MainManager.instance.zees -= price;

            buildPanel.SetActive(false);
            MainManager.instance.BuildBuilding(buildType);
        }
    }
}

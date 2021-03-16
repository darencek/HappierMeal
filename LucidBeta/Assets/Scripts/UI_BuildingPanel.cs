using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_BuildingPanel : MonoBehaviour
{
    public TextMeshProUGUI price_dreamMachine;
    public TextMeshProUGUI price_dreamEngine;
    public TextMeshProUGUI price_factory;
    public TextMeshProUGUI price_foundry;
    public TextMeshProUGUI price_refinery;
    public TextMeshProUGUI price_crystarium;
    public TextMeshProUGUI price_workshop;
    public TextMeshProUGUI price_gardenShed;
    public TextMeshProUGUI price_incubator;
    public TextMeshProUGUI price_bakery;
    public TextMeshProUGUI price_fishery;

    public GameObject[] buildWindow_Panels;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePrices();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void UI_BuildSwitchPanel(int panel)
    {
        for (int i = 0; i < buildWindow_Panels.Length; i++)
        {
            buildWindow_Panels[i].SetActive(i == panel);
        }
    }
    void UpdatePrices()
    {
        price_dreamMachine.text = "$" + Building.GetPrice(Building.BuildingType.DREAM_MACHINE);
        price_factory.text = "$" + Building.GetPrice(Building.BuildingType.FACTORY);
    }
}

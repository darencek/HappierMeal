using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UI_BuildingInfoPanel : MonoBehaviour
{
    public Building selected;

    public Building.BuildingStats stats;
    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI buildingInfo;
    public Image sprite;
    public GameObject energizeButton;
    public GameObject energizeDoneButton;
    public TextMeshProUGUI energizeButtonText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SelectBuilding(Building b)
    {
        selected = b;
        stats = Building.GetStat(b.type);
        sprite.sprite = MainManager.buildingSpriteManager.GetBuildingSprite(b.type);
    }

    // Update is called once per frame
    void Update()
    {
        if (!selected) return;

        buildingName.text = stats.buildingName;
        buildingInfo.text = stats.buildingInfo;

        energizeButton.SetActive((!selected.energized && stats.energizeCost > 0));
        energizeButtonText.text = "Energize\n" + stats.energizeCost + " Energy";

        energizeButton.SetActive(!selected.energized);
        energizeDoneButton.SetActive(selected.energized);
    }

    public void UI_Close()
    {
        gameObject.SetActive(false);
    }

    public void UI_Demolish()
    {
        selected.Demolish();
        UI_Close();
    }
    public void UI_Energize()
    {
        if (MainManager.instance.energy_resource >= stats.energizeCost)
        {
            MainManager.instance.energy_resource -= stats.energizeCost;
            selected.energized = true;
        }
    }
}

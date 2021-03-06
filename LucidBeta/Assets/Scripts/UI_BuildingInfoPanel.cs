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
    public GameObject energizeInfo;
    public GameObject demolishConfirm;


    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        demolishConfirm.SetActive(false);
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


        energizeButtonText.text = "Energize\n" + stats.energizeCost + " Energy";

        energizeInfo.SetActive(stats.energizeCost > 0);
        energizeButton.SetActive((!selected.energized && stats.energizeCost > 0));
        energizeDoneButton.SetActive(selected.energized && stats.energizeCost > 0);
    }

    public void UI_Close()
    {
        gameObject.SetActive(false);
    }
    public void UI_Demolish()
    {
        MainManager.musicManager.PlayClick();
        demolishConfirm.SetActive(true);
    }

    public void UI_DemolishCancel()
    {
        MainManager.musicManager.PlayClick();
        demolishConfirm.SetActive(false);
    }
    public void UI_DemolishConfirm()
    {
        MainManager.musicManager.PlayDemolishSound();
        selected.Demolish();
        demolishConfirm.SetActive(false);
        UI_Close();
    }

    public void UI_Energize()
    {
        if (MainManager.instance.energy_resource >= stats.energizeCost)
        {
            MainManager.instance.energy_resource -= stats.energizeCost;
            selected.Energize();
        }
    }
}

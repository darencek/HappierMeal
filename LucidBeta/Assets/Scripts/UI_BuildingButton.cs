using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_BuildingButton : MonoBehaviour
{
    public Building.BuildingStats stats;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI buildTimeText;

    public Image priceLabelSprite;

    public Color col_green;
    public Color col_red;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        priceLabelSprite.color = (MainManager.instance.zees >= stats.price) ? col_green : col_red;
    }

    public void ButtonClicked()
    {
        MainManager.uiManager.UI_BuildBuilding(stats.type);
    }
    public void UpdateText(Building.BuildingStats stats)
    {
        this.stats = stats;
        nameText.text = stats.buildingName;
        infoText.text = stats.buildingInfo;
        priceText.text = "$" + stats.price;
        buildTimeText.text = "Build time:\n" + stats.buildHours + " sleep hours";
    }
}

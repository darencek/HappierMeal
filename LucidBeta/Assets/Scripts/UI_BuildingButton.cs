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

    public Image sprite;
    public Image priceLabelSprite;
    public Image mainSprite;

    public Color col_green;
    public Color col_red;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.instance.zees >= stats.price)
        {
            priceLabelSprite.color = col_green;
            mainSprite.color = Color.white;
        }
        else
        {
            priceLabelSprite.color = col_red;
            mainSprite.color = Color.gray;
        }

        sprite.sprite = MainManager.buildingSpriteManager.GetBuildingSprite(stats.type);
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
        buildTimeText.text = stats.buildHours + " hours of sleep";
    }
}

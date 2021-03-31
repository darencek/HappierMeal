using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI levelText;

    public Image sprite;
    public Image priceLabelSprite;
    public Image mainSprite;

    public Color col_green;
    public Color col_red;

    public UpgradeManager.Upgrade upgrade;

    public bool researchable = false;

    void OnEnable()
    {

    }
    void Update()
    {
        if (upgrade != null)
            researchable = (MainManager.instance.CountBuildingsOfType(upgrade.buildingType) > 0);

        UpdateText();

        if (upgrade.level >= upgrade.maxLevel || !researchable)
        {
            priceLabelSprite.color = Color.gray;
            mainSprite.color = Color.gray;
        }
        else if (MainManager.instance.zees >= upgrade.price)
        {
            priceLabelSprite.color = col_green;
            mainSprite.color = Color.white;
        }
        else
        {
            priceLabelSprite.color = col_red;
            mainSprite.color = Color.gray;
        }

        sprite.sprite = MainManager.buildingSpriteManager.GetBuildingSprite(upgrade.buildingType);
    }

    public void UpdateText()
    {
        nameText.text = upgrade.upgradeName;
        infoText.text = upgrade.upgradeInfo;
        priceText.text = (upgrade.level >= upgrade.maxLevel) ? "MAXED" : "$" + MainManager.FormatMoney(upgrade.GetAdjustedPrice());
        levelText.text = upgrade.level + "";
    }

    public void ButtonClicked()
    {
        if (!researchable) return;

        float p = upgrade.GetAdjustedPrice();
        if (MainManager.instance.zees >= p && upgrade.level < upgrade.maxLevel)
        {
            MainManager.instance.zees -= p;
            upgrade.level++;
        }
    }
}

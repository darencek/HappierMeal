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
    public Button energizeButton;
    public TextMeshProUGUI energizeButtonText;

    float exitCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SelectBuilding(Building b)
    {
        selected = b;
        stats = Building.GetStat(b.type);
        exitCooldown = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (exitCooldown <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !MainManager.MouseOnUI)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            exitCooldown -= Time.deltaTime;
        }

        if (!selected) return;

        buildingName.text = stats.buildingName;
        buildingInfo.text = stats.buildingInfo;

        energizeButton.gameObject.SetActive((!selected.energized && stats.energizeCost > 0));
        energizeButtonText.text = "Energize\n" + stats.energizeCost + " Energy";
    }
    public void Energize()
    {
        if (MainManager.instance.zees >= stats.energizeCost)
        {
            MainManager.instance.zees -= stats.energizeCost;
            selected.energized = true;
        }
    }
}

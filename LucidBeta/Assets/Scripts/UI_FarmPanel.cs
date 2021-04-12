using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_FarmPanel : MonoBehaviour
{
    public FarmPlot selectedFarm;
    public GameObject seedPanel;
    public GameObject[] seedPanelPanels;

    public GameObject crossbreedPopup;
    public TextMeshProUGUI crossbreedPopup_Text;

    public UI_FarmSlotButton[] slotButtons;
    public Sprite noCropSprite;

    int selectedSlot = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (i < selectedFarm.activeSlots)
            {
                slotButtons[i].gameObject.SetActive(true);
                if (selectedFarm.crops[i] != null)
                {
                    Crop C = selectedFarm.crops[i];
                    Crop.CropInfo info = new Crop.CropInfo(C.type);
                    slotButtons[i].cropName.text = info.name;
                    slotButtons[i].progress.text = Mathf.RoundToInt(C.growTime / C.maxGrowTime * 100) + "% Grown";
                    slotButtons[i].cropSprite.sprite = MainManager.farmManager.cropSprites[(int)C.type - 1];
                }
                else
                {
                    slotButtons[i].cropName.text = "Empty";
                    slotButtons[i].progress.text = "Tap to Plant";
                    slotButtons[i].cropSprite.sprite = noCropSprite;
                }
            }
            else
            {
                slotButtons[i].gameObject.SetActive(false);
            }
        }
    }
    public void OpenPanel(FarmPlot p)
    {
        selectedFarm = p;
        seedPanel.SetActive(false);
    }

    public void UI_SelectSeed(Crop.CropType type)
    {
        float q = MainManager.instance.SeedInventory[type];
        if (q > 0 || q == -1)
        {
            if (q > 0)
                MainManager.instance.SeedInventory[type]--;
            seedPanel.SetActive(false);
            selectedFarm.Plant(selectedSlot, type, Crop.SoilType.DREAM_SOIL);
        }
    }

    public void UI_OpenSeedWindow(int slot)
    {
        if (selectedFarm.crops[slot] == null)
        {
            seedPanel.SetActive(true);
            selectedSlot = slot;
        }
        else
        {
            Crop C = selectedFarm.crops[slot];
            if (C.growTime >= C.maxGrowTime)
            {
                selectedFarm.Harvest(slot);
            }
        }
    }

    public void UI_ShowCrossbreedPopup(Crop.CropInfo info)
    {
        crossbreedPopup.SetActive(true);
        crossbreedPopup_Text.text = "Obtained " + info.name + " seed.";
    }

    public void UI_ShowBuildingInfoButton()
    {
        MainManager.uiManager.UI_ShowBuildingInfoPanel(selectedFarm.gameObject.GetComponent<Building>());
    }

    public void UI_SeedSwitchPanel(int panel)
    {
        for (int i = 0; i < seedPanelPanels.Length; i++)
        {
            seedPanelPanels[i].SetActive(i == panel);
        }
    }

    public void UI_Close()
    {
        gameObject.SetActive(false);
    }
    public void UI_CloseSeedPanel()
    {
        seedPanel.SetActive(false);
    }
}


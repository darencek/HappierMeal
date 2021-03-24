using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FarmPanel : MonoBehaviour
{
    public FarmPlot selectedFarm;
    public GameObject seedPanel;

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

    public void UI_ShowBuildingInfoButton()
    {
        MainManager.uiManager.UI_ShowBuildingInfoPanel(selectedFarm.gameObject.GetComponent<Building>());
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    Building building;

    public SpriteRenderer[] cropSprites_large;
    public SpriteRenderer[] cropSprites_small;

    public Crop[] crops = new Crop[4];

    public int activeSlots = 0;

    void Start()
    {
        building = GetComponent<Building>();
    }
    public void ClearSlots()
    {
        crops = new Crop[4];
    }

    void Update()
    {

        if (building.type == Building.BuildingType.LARGE_FARM || building.type == Building.BuildingType.SMALL_FARM)
        {
            activeSlots = building.type == Building.BuildingType.SMALL_FARM ? 1 : 4;

            for (int i = 0; i < activeSlots; i++)
            {
                Crop c = crops[i];

                SpriteRenderer sp = building.type == Building.BuildingType.SMALL_FARM ? cropSprites_small[i] : cropSprites_large[i];

                if (c == null)
                {
                    sp.sprite = MainManager.farmManager.growingSprites[0]; ;
                    continue;
                }

                if (!building.underConstruction)
                {
                    if (c.growTime < c.maxGrowTime)
                    {
                        c.growTime += MainManager.dreamTimeScale * Time.deltaTime * MainManager.instance.farm_grow_multiplier;

                        float f = (c.growTime / c.maxGrowTime) * 3f;
                        sp.sprite = MainManager.farmManager.growingSprites[Mathf.FloorToInt(f) + 1];
                    }
                    else
                    {
                        sp.sprite = MainManager.farmManager.cropSprites[(int)c.type - 1];
                    }
                }
            }

        }
    }

    public void Fertilize(float hours)
    {
        activeSlots = building.type == Building.BuildingType.SMALL_FARM ? 1 : 4;

        for (int i = 0; i < activeSlots; i++)
        {
            Crop c = crops[i];
            if (c != null && c.type != Crop.CropType.NONE)
            {
                c.growTime += MainManager.dreamTimeScale * 60 * 60 * hours;
            }
        }
    }

    public void Plant(int slot, Crop.CropType seed, Crop.SoilType soil)
    {
        crops[slot] = new Crop(seed, soil);
    }

    public void Harvest(int slot)
    {
        Crop c = crops[slot];
        //Crossbreed Check
        foreach (Crop.CropInfo ci in MainManager.farmManager.crossbreeds)
        {
            bool x1 = false;
            bool x2 = false;
            foreach (Crop cr in crops)
            {
                if (cr != null)
                {
                    if (cr.type == ci.x1)
                        x1 = true;
                    if (cr.type == ci.x2)
                        x2 = true;
                }
            }

            if (x1 && x2)
            {
                float chanceTresh = 50 * Mathf.Pow(0.99f, UpgradeManager.farm_crossbreed_upgrade.level);
                if (Random.Range(0, 100) <= chanceTresh)
                {
                    if (!MainManager.instance.SeedInventory.ContainsKey(ci.type))
                    {
                        MainManager.instance.SeedInventory[ci.type] = 0;
                    }

                    MainManager.uiManager.UI_CrossbreedPopup(ci);
                    MainManager.instance.SeedInventory[ci.type]++;
                }
            }
        }

        c.Harvest();

        crops[slot] = null;
    }
}

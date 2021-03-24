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
                        c.growTime += MainManager.dreamTimeScale * Time.unscaledDeltaTime;

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

    public void Plant(int slot, Crop.CropType seed, Crop.SoilType soil)
    {
        crops[slot] = new Crop(seed, soil);
    }

    public void Harvest(int slot)
    {
        crops[slot].Harvest();
        crops[slot] = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Sprite[] cropSprites;
    public Sprite[] growingSprites;

    private void Start()
    {
        MainManager.farmManager = this;
    }
}

public class Crop
{
    public enum SoilType { DREAM_SOIL, BOSOIL, MULSOIL };
    public enum CropType { NONE, DREAMLITE, ENERGITE, RANDITE, SPECITE, FERRITE };

    public SoilType soil;
    public CropType type;
    public float growTime;
    public float maxGrowTime;

    public Crop(CropType type, SoilType soil)
    {
        this.soil = soil;
        this.type = type;
        growTime = 0;
        maxGrowTime = 12 * (60 * 60);

        switch (type)
        {
        }
    }

    public void Harvest()
    {
        float multiplier = soil == SoilType.BOSOIL ? 1.1f : 1;

        switch (type)
        {
            case CropType.DREAMLITE:
                MainManager.instance.rest_resource += 1500 * multiplier;
                break;
            case CropType.ENERGITE:
                MainManager.instance.energy_resource += 2000 * multiplier;
                break;
            case CropType.RANDITE:
                MainManager.instance.zees += Random.Range(5000, 20000) * multiplier;
                break;
            case CropType.SPECITE:
                break;
            case CropType.FERRITE:
                break;
        }
    }

    public struct CropInfo
    {
        public string name;
        public string info;
        public string xbreed;

        public CropInfo(CropType type)
        {
            name = "SEED";
            info = "-";
            xbreed = "";

            switch (type)
            {
                case CropType.DREAMLITE:
                    name = "Dreamlite";
                    info = "Gives 1500 rest on harvest.";
                    break;
                case CropType.ENERGITE:
                    name = "Energite";
                    info = "Gives 2000 energy on harvest.";
                    break;
                case CropType.RANDITE:
                    name = "Randite";
                    info = "Gives $5000 to $20000 randomly on harvest.";
                    break;
                case CropType.SPECITE:
                    name = "Specite";
                    info = "";
                    break;
                case CropType.FERRITE:
                    name = "Ferrite";
                    info = "";
                    break;
            }
        }
    }
}
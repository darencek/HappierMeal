using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Sprite[] cropSprites;
    public Sprite[] growingSprites;

    public List<Crop.CropInfo> crossbreeds = new List<Crop.CropInfo>();

    private void Start()
    {
        MainManager.farmManager = this;

        crossbreeds.Add(new Crop.CropInfo(Crop.CropType.FERRITE));
        crossbreeds.Add(new Crop.CropInfo(Crop.CropType.SPECITE));
        crossbreeds.Add(new Crop.CropInfo(Crop.CropType.BATTRON));
        crossbreeds.Add(new Crop.CropInfo(Crop.CropType.GROWON));
    }
}

public class Crop
{
    public enum SoilType { DREAM_SOIL, BOSOIL, MULSOIL };
    public enum CropType { NONE, DREAMLITE, ENERGITE, RANDITE, SPECITE, FERRITE, BATTRON, GROWON, SLOWON, CRYSTAVER, GENERVER, QUICKSILVER };

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
    }

    public void Harvest()
    {
        float multiplier = soil == SoilType.BOSOIL ? 1.1f : 1;

        multiplier *= Mathf.Pow(1.01f, UpgradeManager.farm_yield_upgrade.level);

        switch (type)
        {
            //T1
            case CropType.DREAMLITE:
                MainManager.instance.rest_resource += 500 * multiplier;
                break;
            case CropType.ENERGITE:
                MainManager.instance.energy_resource += 180 * multiplier;
                break;
            case CropType.RANDITE:
                MainManager.instance.zees += Random.Range(1000, 2000) * multiplier;
                break;
            case CropType.SPECITE:
                MainManager.instance.cropBuffs.Add(new CropBonusBuff(CropBonusBuff.buffType.ZLIMIT1));
                break;
            case CropType.FERRITE:
                MainManager.instance.cropBuffs.Add(new CropBonusBuff(CropBonusBuff.buffType.CONSTRUCTION));
                break;
            //T2
            case CropType.BATTRON:
                MainManager.instance.energy_resource += 300 * multiplier;
                break;
            case CropType.GROWON:
                MainManager.instance.cropBuffs.Add(new CropBonusBuff(CropBonusBuff.buffType.ENERGYCOST1));
                break;
            case CropType.SLOWON:
                MainManager.instance.cropBuffs.Add(new CropBonusBuff(CropBonusBuff.buffType.REST1));
                break;
            //T3
            case CropType.CRYSTAVER:
                MainManager.instance.rest_resource += 1000 * multiplier;
                break;
            case CropType.GENERVER:
                MainManager.instance.cropBuffs.Add(new CropBonusBuff(CropBonusBuff.buffType.ENERGY1));
                break;
            case CropType.QUICKSILVER:
                MainManager.instance.cropBuffs.Add(new CropBonusBuff(CropBonusBuff.buffType.EARNING1));
                break;
        }
    }

    public struct CropInfo
    {
        public string name;
        public string info;
        public string xbreed;

        public CropType x1;
        public CropType x2;

        public CropType type;

        public CropInfo(CropType type)
        {
            this.type = type;

            name = "SEED";
            info = "-";
            xbreed = "";

            x1 = CropType.NONE;
            x2 = CropType.NONE;

            switch (type)
            {
                //T1
                case CropType.DREAMLITE:
                    name = "Dreamlite";
                    info = "Gives 500 rest on harvest.";
                    break;
                case CropType.ENERGITE:
                    name = "Energite";
                    info = "Gives 180 energy on harvest.";
                    break;
                case CropType.RANDITE:
                    name = "Randite";
                    info = "Gives $1000 to $2000 randomly on harvest.";
                    break;
                case CropType.SPECITE:
                    name = "Specite";
                    info = "Harvest to increase $ earning limit by 10% for 24 hours.";
                    x1 = CropType.ENERGITE;
                    x2 = CropType.RANDITE;
                    break;
                case CropType.FERRITE:
                    name = "Ferrite";
                    info = "Harvest to double construction speed for 24 hours (not stackable).";
                    x1 = CropType.DREAMLITE;
                    x2 = CropType.RANDITE;
                    break;
                //T2
                case CropType.BATTRON:
                    name = "Battron";
                    info = "Gives 300 energy on harvest.";
                    x1 = CropType.ENERGITE;
                    x2 = CropType.FERRITE;
                    break;
                case CropType.GROWON:
                    name = "Growon";
                    info = "Harvest to reduce energy running costs by 5% for 24 hours.";
                    x1 = CropType.RANDITE;
                    x2 = CropType.SPECITE;
                    break;
                case CropType.SLOWON:
                    name = "Slowon";
                    info = "Harvest to boost rest generation by 5% for 24 hours.";
                    x1 = CropType.DREAMLITE;
                    x2 = CropType.SPECITE;
                    break;
                //T3
                case CropType.CRYSTAVER:
                    name = "Crystaver";
                    info = "Gives 1000 rest on harvest.";
                    x1 = CropType.DREAMLITE;
                    x2 = CropType.SLOWON;
                    break;
                case CropType.GENERVER:
                    name = "Generver";
                    info = "Harvest to boost energy generation by 5% for 24 hours.";
                    x1 = CropType.BATTRON;
                    x2 = CropType.SLOWON;
                    break;
                case CropType.QUICKSILVER:
                    name = "Quicksilver";
                    info = "Harvest to boost $ earned by 3% for 24 hours.";
                    x1 = CropType.SLOWON;
                    x2 = CropType.GROWON;
                    break;
            }
        }
    }
}

public class CropBonusBuff
{
    public enum buffType { ZLIMIT1, CONSTRUCTION, ENERGYCOST1, REST1, ENERGY1, EARNING1 };
    public float timeLeft = 24 * 60 * 60;
    public buffType type = buffType.ZLIMIT1;

    public CropBonusBuff(buffType t)
    {
        type = t;
    }
    public void Update()
    {
        switch (type)
        {
            case buffType.ZLIMIT1:
                MainManager.instance.zees_earnLimit_bonusMultiplier *= 1.1f;
                break;
            case buffType.CONSTRUCTION:
                MainManager.instance.constructionSpeed_multiplier = 2f;
                break;
            case buffType.ENERGYCOST1:
                MainManager.instance.energy_upkeep_multiplier *= 0.95f;
                break;
            case buffType.REST1:
                MainManager.instance.rest_EarnMultiplier *= 1.05f;
                break;
            case buffType.ENERGY1:
                MainManager.instance.energy_EarnMultiplier *= 1.05f;
                break;
            case buffType.EARNING1:
                MainManager.instance.zee_FromRestEarnMultiplier *= 1.03f;
                break;
        }
    }
}

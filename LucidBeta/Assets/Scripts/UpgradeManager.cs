using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    public static Upgrade dreamMachine_efficiency;
    public static Upgrade factory_efficiency;
    public static Upgrade bakery_efficiency;
    public static Upgrade refinery_efficiency;
    public static Upgrade foundry_efficiency;

    public static Upgrade dreamEngine_upgrade;
    public static Upgrade fishery_upgrade;
    public static Upgrade crystarium_upgrade;

    public static Upgrade farm_grow_upgrade;
    public static Upgrade farm_crossbreed_upgrade;
    public static Upgrade farm_yield_upgrade;

    private void Start()
    {
        MainManager.upgradeManager = this;
    }

    private void Awake()
    {
        ResetUpgrades();
    }
    public void ResetUpgrades()
    {
        Upgrade up;

        up = new Upgrade();
        up.upgradeName = "Electric Sheep";
        up.upgradeInfo = "Improves each Dream Machine output by 10%.";
        up.buildingType = Building.BuildingType.DREAM_MACHINE;
        dreamMachine_efficiency = up;

        up = new Upgrade();
        up.upgradeName = "Snooze Generators";
        up.upgradeInfo = "Improves each Factory output by 3%.";

        up.buildingType = Building.BuildingType.FACTORY;
        factory_efficiency = up;
        up = new Upgrade();
        up.upgradeName = "Fluffier Bread";
        up.upgradeInfo = "Each Bakeries produce 5% more energy.";
        up.priceMultiplier = 3.2f;
        up.buildingType = Building.BuildingType.BAKERY;
        bakery_efficiency = up;

        up = new Upgrade();
        up.upgradeName = "Premium Sleepsand";
        up.upgradeInfo = "Improves $ gained by a further 1% for each Refineries.";
        up.buildingType = Building.BuildingType.REFINERY;
        refinery_efficiency = up;

        up = new Upgrade();
        up.upgradeName = "Sleep Capacitors";
        up.upgradeInfo = "Grants a bonus of 100 energy for every 400 energy gained.";
        up.price = 12000;
        up.priceMultiplier = 3f;
        up.buildingType = Building.BuildingType.FOUNDRY;
        foundry_efficiency = up;

        up = new Upgrade();
        up.upgradeName = "Daydream Battery";
        up.upgradeInfo = "Improves Rest limit increase by 50% for each Dream Engine.";
        up.buildingType = Building.BuildingType.DREAM_ENGINE;
        dreamEngine_upgrade = up;

        up = new Upgrade();
        up.upgradeName = "Cloud Boats";
        up.upgradeInfo = "Improves Energy limit increase by 80% for each Fishery.";
        up.price = 6000;
        up.priceMultiplier = 3f;
        up.buildingType = Building.BuildingType.FISHERY;
        fishery_upgrade = up;

        up = new Upgrade();
        up.upgradeName = "Crystal Bank";
        up.upgradeInfo = "Further improves $ earning limit by 50% for each Crystarium.";
        up.buildingType = Building.BuildingType.CRYSTARIUM;
        crystarium_upgrade = up;

        up = new Upgrade();
        up.upgradeName = "Fertilizer";
        up.upgradeInfo = "Improves plants growing speed by 1%.";
        up.buildingType = Building.BuildingType.GARDEN_SHED;
        up.maxLevel = 50;
        farm_grow_upgrade = up;

        up = new Upgrade();
        up.upgradeName = "Plant Genes";
        up.upgradeInfo = "Improves crossbreeding chances by 1%.";
        up.buildingType = Building.BuildingType.GARDEN_SHED;
        up.maxLevel = 50;
        farm_crossbreed_upgrade = up;

        up = new Upgrade();
        up.upgradeName = "Seed Spreader";
        up.upgradeInfo = "Increases harvesting yield by 1%.";
        up.buildingType = Building.BuildingType.GARDEN_SHED;
        up.maxLevel = 50;
        farm_yield_upgrade = up;
    }

    public class Upgrade
    {
        public Building.BuildingType buildingType = Building.BuildingType.FACTORY;
        public float price = 5000;
        public string upgradeName = "SomeUpgrade";
        public string upgradeInfo = "This does some things";
        public int maxLevel = 30;
        public float priceMultiplier = 2.3f;

        public int level = 0;

        public float GetAdjustedPrice()
        {
            return price + price * (priceMultiplier * level);
        }
    }
}

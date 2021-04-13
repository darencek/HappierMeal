using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public enum BuildingType { NONE, DREAM_MACHINE, DREAM_ENGINE, FACTORY, REFINERY, FOUNDRY, CRYSTARIUM, GARDEN_SHED, INCUBATOR, WORKSHOP, BAKERY, FISHERY, SMALL_FARM, LARGE_FARM, FRUIT_TREE };

    public BuildingType type;

    public bool underConstruction = false;
    public bool placing = false;

    public bool energized = false;

    public float buildHours = 0;
    float totalBuildHours = 0;

    public GameObject[] buildingObjects;

    public GameObject buildingSpriteContainer;
    public SpriteRenderer buildingSprite;
    public SpriteRenderer tileSprite;

    public Sprite[] tileSprites;

    static float gridX = 1f;
    static float gridY = 0.5f;

    public Color buildRed;
    public Color buildColor;

    public GameObject progressBar;
    public GameObject progressBarFill;

    public BoxCollider2D mainClickbox;

    public GameObject[] fruitTree_fruits;

    public GameObject EnergizedEffect;
    public GameObject BuildCompleteEffect;

    public bool SlotUnlocked = true;

    public int tierUnlock = 0;

    // Start is called before the first frame update
    void Start()
    {
        type = BuildingType.NONE;
        _swaySpeed = Random.Range(0.5f, 1f);
        EnergizedEffect.SetActive(false);

        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        SlotUnlocked = MainManager.instance.ascensionLevel >= tierUnlock;

        if (SlotUnlocked)
        {
            BouncyAnimation();

            if (energized)
            {
                if (!EnergizedEffect.activeInHierarchy)
                    EnergizedEffect.SetActive(EnergizedEffect);
            }
            else
            {
                EnergizedEffect.SetActive(false);
            }

            tileSprite.sprite = tileSprites[0];

            if (underConstruction || placing)
                buildingSprite.color = buildColor;
            else
                buildingSprite.color = Color.white;

            if (underConstruction)
            {
                placing = true;
                _growthTimer = 0;

                if (MainManager.instance.sleepState >= 1)
                    buildHours -= Time.unscaledDeltaTime * MainManager.dreamTimeScale * MainManager.instance.constructionSpeed_multiplier;

                if (buildHours <= 0)
                {
                    if (!MainManager.instance.revealEvents.Contains(gameObject))
                        MainManager.instance.revealEvents.Add(gameObject);
                    underConstruction = false;
                }

                UpdateProgressBar();
            }
            else
            {
                FruitTree_Update();
            }
        }
        else
        {
            tileSprite.sprite = tileSprites[1];
            tileSprite.transform.localScale = Vector3.one;
        }
    }

    public void RevealSequence()
    {
        StartCoroutine("RevealCoroutine");
    }

    IEnumerator RevealCoroutine()
    {
        MainManager.uiManager.blockUI = true;
        Time.timeScale = 0f;
        MainManager.instance.CamPan_SmoothToTarget(transform.position);
        yield return new WaitForSecondsRealtime(2f);
        BuildCompleteEffect.SetActive(true);
        placing = false;
        MainManager.musicManager.PlayBoom();
        yield return new WaitForSecondsRealtime(2f);
        BuildCompleteEffect.SetActive(false);
        Time.timeScale = 1f;
        MainManager.uiManager.blockUI = false;
        MainManager.instance.revealRunning = false;
    }

    bool clicked = false;
    public bool CancelClick = false;
    private void OnMouseDown()
    {
        clicked = true;
    }

    private void OnMouseUp()
    {
        if (clicked && SlotUnlocked)
        {
            StartCoroutine("ConfirmClick");
        }

        clicked = false;
    }

    IEnumerator ConfirmClick()
    {
        yield return new WaitForEndOfFrame();
        if (!CancelClick)
            if (!underConstruction && !MainManager.CamPan_JustReleased && !MainManager.MouseOnUI)
            {
                if (type != BuildingType.NONE)
                {
                    if (type == BuildingType.LARGE_FARM || type == BuildingType.SMALL_FARM)
                        MainManager.uiManager.UI_ShowFarmPanel(gameObject.GetComponent<FarmPlot>());
                    else
                        MainManager.uiManager.UI_ShowBuildingInfoPanel(this);
                }
                else
                {
                    MainManager.uiManager.UI_ShowBuildingPanel(this);
                }
            }
        CancelClick = false;
    }

    public void BuildAs(BuildingType buildInto)
    {
        type = buildInto;
        underConstruction = true;
        totalBuildHours = GetBuildHours(type) * 60f * 60f;
        buildHours = totalBuildHours;
        UpdateSprite();
        MainManager.musicManager.PlayDink();
    }

    public void Energize()
    {
        energized = true;
        MainManager.musicManager.PlayDoubleDink();
    }

    public void Demolish()
    {
        if (type == BuildingType.LARGE_FARM || type == BuildingType.SMALL_FARM)
            gameObject.GetComponent<FarmPlot>().ClearSlots();
        type = BuildingType.NONE;
        energized = false;
        UpdateSprite();
        UpdateProgressBar();
    }

    void snapToGrid()
    {
        Vector3 pos = transform.position;

        pos.y = Mathf.Round(pos.y / gridY) * gridY;
        pos.x = Mathf.Round(pos.x / gridX) * gridX;

        transform.position = pos;
    }

    void UpdateProgressBar()
    {
        if (buildHours == 0 || totalBuildHours == 0 || !underConstruction)
        {
            progressBar.SetActive(false);
            return;
        }

        float progress = 1f - buildHours / (totalBuildHours == 0 ? buildHours : totalBuildHours);

        progressBar.SetActive(underConstruction);
        progressBarFill.transform.localScale = new Vector3(progress, 1f);
        progressBarFill.transform.localPosition = new Vector3((1f - progress) * -0.5f, 0f);
    }
    public void UpdateSprite()
    {
        if (type != BuildingType.NONE)
        {
            buildingSprite = buildingObjects[(int)type - 1].GetComponent<SpriteRenderer>();
            buildingSprite.gameObject.SetActive(true);
        }

        foreach (GameObject o in buildingObjects)
            if (o != buildingSprite.gameObject || type == BuildingType.NONE)
                o.SetActive(false);

        //buildingSprite.sprite = MainManager.buildingSpriteManager.GetBuildingSprite(type);

        mainClickbox.enabled = (type != BuildingType.NONE);

    }

    float _growthTimer = 0;
    void FruitTree_Update()
    {
        if (type == BuildingType.FRUIT_TREE)
        {
            _growthTimer += Time.unscaledDeltaTime * MainManager.dreamTimeScale;

            if (_growthTimer >= (60f * 60f * 3f))
            {
                _growthTimer = 0;

                foreach (GameObject g in fruitTree_fruits)
                    if (!g.activeInHierarchy)
                    {
                        g.SetActive(true);
                        break;
                    }
            }

        }
    }

    float _sway = 0;
    float _swayTime = 0;
    float _swaySpeed = 1f;
    void BouncyAnimation()
    {
        _swayTime += Time.deltaTime * _swaySpeed;
        _sway = Mathf.PingPong(_swayTime, 1);
        float fSway = _sway - 0.5f;
        float cSway = Mathf.PingPong(_swayTime * 0.5f, 1) - 0.5f;
        buildingSpriteContainer.transform.rotation = Quaternion.Euler(0, 0, fSway * 0.5f);
        buildingSpriteContainer.transform.localScale = new Vector3(1 + fSway * 0.05f, 1 - fSway * 0.03f, 1);
        tileSprite.transform.localScale = new Vector3(1 + cSway * 0.1f, 1 - cSway * 0.03f, 1);
    }

    public static float GetPrice(BuildingType type)
    {
        return GetStat(type).price;
    }
    public static float GetBuildHours(BuildingType type)
    {
        return GetStat(type).buildHours;
    }

    public static BuildingStats GetStat(BuildingType type)
    {
        BuildingStats stats = new BuildingStats(type);
        return stats;
    }

    public struct BuildingStats
    {
        public BuildingType type;
        public float price;
        public float buildHours;
        public string buildingName;
        public string buildingInfo;
        public float energizeCost;

        public BuildingStats(Building.BuildingType type)
        {
            this.type = type;
            price = 0;
            buildHours = 1;
            buildingName = "Building";
            buildingInfo = "Building does things.";
            energizeCost = 0;

            switch (type)
            {
                case BuildingType.DREAM_MACHINE:
                    price = 1000;
                    buildHours = 3;
                    buildingName = "Dream Machine";
                    buildingInfo = "Generates 1 Rest every minute while sleeping.";
                    energizeCost = 200;
                    break;
                case BuildingType.FACTORY:
                    price = 75000;
                    buildHours = 7;
                    buildingName = "Factory";
                    buildingInfo = "Generates 10 Rest every minute while sleeping.";
                    energizeCost = 1000;
                    break;
                case BuildingType.BAKERY:
                    price = 5000;
                    buildHours = 4;
                    buildingName = "Bakery";
                    buildingInfo = "Produces 1 Energy every minute while sleeping.";
                    energizeCost = 300;
                    break;
                case BuildingType.REFINERY:
                    price = 100000;
                    buildHours = 7;
                    buildingName = "Refinery";
                    buildingInfo = "Increases $ earned by 5% while sleeping.";
                    energizeCost = 1000;
                    break;
                case BuildingType.FOUNDRY:
                    price = 45000;
                    buildHours = 7;
                    buildingName = "Foundry";
                    buildingInfo = "Increases Energy gained by 10% while sleeping.";
                    energizeCost = 1000;
                    break;

                case BuildingType.WORKSHOP:
                    price = 10000;
                    buildHours = 7;
                    buildingName = "Workshop";
                    buildingInfo = "Enables researching upgrades for buildings.";
                    break;
                case BuildingType.DREAM_ENGINE:
                    price = 25000;
                    buildHours = 5;
                    buildingName = "Dream Engine";
                    buildingInfo = "Increases Rest limit by 1000.";
                    break;
                case BuildingType.FISHERY:
                    price = 50000;
                    buildHours = 5;
                    buildingName = "Fishery";
                    buildingInfo = "Increases Energy limit by 100.";
                    break;
                case BuildingType.CRYSTARIUM:
                    price = 50000;
                    buildHours = 14;
                    buildingName = "Crystarium";
                    buildingInfo = "Increases $ earning limit by 2000.";
                    break;
                case BuildingType.GARDEN_SHED:
                    price = 10000;
                    buildHours = 7;
                    buildingName = "Garden Shed";
                    buildingInfo = "Adds gardening upgrades to the workshop.";
                    break;
                case BuildingType.INCUBATOR:
                    price = 30000;
                    buildHours = 7;
                    buildingName = "Incubator";
                    buildingInfo = "Enables purchasing of special seeds and soil for gardening.";
                    break;
                case BuildingType.SMALL_FARM:
                    price = 12000;
                    buildHours = 1;
                    buildingName = "Small Garden";
                    buildingInfo = "Allows you to plant 1 seed to grow a plant.";
                    break;
                case BuildingType.LARGE_FARM:
                    price = 40000;
                    buildHours = 4;
                    buildingName = "Large Garden";
                    buildingInfo = "Allows you to plant up to 4 seeds to grow plants and crossbreed seeds.";
                    break;
                case BuildingType.FRUIT_TREE:
                    price = 45000;
                    buildHours = 4;
                    buildingName = "Fruit Tree";
                    buildingInfo = "Grows up to 3 fruits every 3 hours that can be harvested for $7000.";
                    break;
            }
        }
    }
}


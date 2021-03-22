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

    public GameObject fruitTree_fruitGroup;
    public GameObject[] fruitTree_fruits;

    public bool SlotUnlocked = true;

    // Start is called before the first frame update
    void Start()
    {
        type = BuildingType.NONE;
        _swaySpeed = Random.Range(0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (SlotUnlocked)
        {
            BouncyAnimation();

            tileSprite.sprite = tileSprites[0];
            buildingSprite.gameObject.SetActive(true);

            UpdateSprite();

            if (underConstruction)
            {
                _growthTimer = 0;

                if (MainManager.instance.sleepState >= 1)
                    buildHours -= Time.deltaTime * MainManager.dreamTimeScale;

                if (buildHours <= 0)
                    underConstruction = false;

                UpdateProgressBar();
            }
            else
            {
                FruitTree_Update();
            }
        }
        else
        {
            buildingSprite.gameObject.SetActive(false);
            tileSprite.sprite = tileSprites[1];
            tileSprite.transform.localScale = Vector3.one;
        }
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
        Debug.Log("Building into: " + buildInto);
        type = buildInto;
        underConstruction = true;
        totalBuildHours = GetBuildHours(type) * 60f * 60f;
        buildHours = totalBuildHours;
    }

    public void Demolish()
    {
        type = BuildingType.NONE;
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
        //foreach (GameObject o in buildingSprites)
        //    o.SetActive(false);

        mainClickbox.enabled = (type != BuildingType.NONE);

        buildingSprite.sprite = MainManager.buildingSpriteManager.GetBuildingSprite(type);

        if (underConstruction)
            buildingSprite.color = buildColor;
        else
            buildingSprite.color = Color.white;

    }

    float _growthTimer = 0;
    void FruitTree_Update()
    {
        if (type == BuildingType.FRUIT_TREE)
        {
            fruitTree_fruitGroup.SetActive(true);

            _growthTimer += Time.unscaledDeltaTime * MainManager.dreamTimeScale * Random.Range(0.7f,1f);

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
        else
        {
            fruitTree_fruitGroup.SetActive(false);
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
        buildingSprite.transform.rotation = Quaternion.Euler(0, 0, fSway * 0.5f);
        buildingSprite.transform.localScale = new Vector3(1 + fSway * 0.05f, 1 - fSway * 0.03f, 1);
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
                    energizeCost = 300;
                    break;
                case BuildingType.FACTORY:
                    price = 25000;
                    buildHours = 5;
                    buildingName = "Factory";
                    buildingInfo = "Generates 10 Rest every minute while sleeping.";
                    energizeCost = 1000;
                    break;
                case BuildingType.BAKERY:
                    price = 1500;
                    buildHours = 5;
                    buildingName = "Bakery";
                    buildingInfo = "Produces 1 Energy every minute while sleeping.";
                    energizeCost = 800;
                    break;
                case BuildingType.REFINERY:
                    price = 50000;
                    buildHours = 10;
                    buildingName = "Refinery";
                    buildingInfo = "Increases $ earned by 10% while sleeping.";
                    energizeCost = 2000;
                    break;
                case BuildingType.FOUNDRY:
                    price = 45000;
                    buildHours = 8;
                    buildingName = "Foundry";
                    buildingInfo = "Increases Energy gained by 10% while sleeping.";
                    energizeCost = 4200;
                    break;

                case BuildingType.WORKSHOP:
                    price = 12000;
                    buildHours = 3;
                    buildingName = "Workshop";
                    buildingInfo = "Enables researching upgrades for buildings.";
                    break;
                case BuildingType.DREAM_ENGINE:
                    price = 5000;
                    buildHours = 5;
                    buildingName = "Dream Engine";
                    buildingInfo = "Increases Rest limit by 500.";
                    break;
                case BuildingType.FISHERY:
                    price = 7000;
                    buildHours = 5;
                    buildingName = "Fishery";
                    buildingInfo = "Increases Energy limit by 50.";
                    break;
                case BuildingType.CRYSTARIUM:
                    price = 30000;
                    buildHours = 10;
                    buildingName = "Crystarium";
                    buildingInfo = "Increases $ earning limit by 5000.";
                    break;
                case BuildingType.GARDEN_SHED:
                    price = 15000;
                    buildHours = 3;
                    buildingName = "Garden Shed";
                    buildingInfo = "Enables researching upgrades for gardening.";
                    break;
                case BuildingType.INCUBATOR:
                    price = 25000;
                    buildHours = 3;
                    buildingName = "Incubator";
                    buildingInfo = "Enables purchasing of special seeds and soil for gardening.";
                    break;
                case BuildingType.SMALL_FARM:
                    price = 5000;
                    buildHours = 1;
                    buildingName = "Small Garden";
                    buildingInfo = "Allows you to plant 1 seed to grow a plant.";
                    break;
                case BuildingType.LARGE_FARM:
                    price = 15000;
                    buildHours = 4;
                    buildingName = "Large Garden";
                    buildingInfo = "Allows you to plant up to 4 seeds to grow plants and crossbreed seeds.";
                    break;
                case BuildingType.FRUIT_TREE:
                    price = 15000;
                    buildHours = 4;
                    buildingName = "Fruit Tree";
                    buildingInfo = "Grows up to 3 fruits every 3 hours that can be harvested for $7000.";
                    break;
            }
        }
    }
}


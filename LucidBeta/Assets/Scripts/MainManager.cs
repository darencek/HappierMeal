using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public static UIManager uiManager;
    public static BuildingSpriteManager buildingSpriteManager;
    public static CreatureManager creatureManager;
    public static FarmManager farmManager;

    public Dictionary<Crop.CropType, int> SeedInventory;

    public EventSystem eventSystem;

    public static bool MouseOnUI;

    public static float dreamTimeScale = 3600f;

    public bool playTutorial;

    public int sleepState = 0;
    public float sleepTime = 0;

    public int ascensionLevel = 1;
    public double nextAscension = 100;

    public double zees = 0;
    public double zees_earnLimit = 0;
    public double zees_earnLimit_base = 1000;

    public float rest_resource = 0;
    public float rest_limit = 0;
    public float rest_limit_base = 1000;

    public float energy_resource = 0;
    public float energy_limit = 0;
    public float energy_limit_base = 500;
    public float energy_upkeep = 0;

    float zee_FromRestEarnMultiplier = 1f;
    float rest_EarnMultiplier = 1f;
    float energy_EarnMultiplier = 1f;

    public double zee_earningPerMinute = 0f;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetResources();
        StartCoroutine("DreamTick");

        Camera.main.orthographicSize = 3;

        ascensionLevel = 1;
        nextAscension = 500000;
    }

    public void Ascend()
    {
        if (zees >= nextAscension)
        {
            ascensionLevel++;
            ResetResources();

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
                g.GetComponent<Building>().Demolish();

            nextAscension *= 3.5f;
        }
    }

    void ResetResources()
    {
        zees = 1000;
        zees_earnLimit_base = 10000;

        rest_resource = 0;
        rest_limit_base = 5000;

        energy_resource = 0;
        energy_limit_base = 500;

        SeedInventory = new Dictionary<Crop.CropType, int>();
        SeedInventory.Add(Crop.CropType.DREAMLITE, -1);
        SeedInventory.Add(Crop.CropType.ENERGITE, -1);
        SeedInventory.Add(Crop.CropType.FERRITE, 0);
        SeedInventory.Add(Crop.CropType.RANDITE, -1);
        SeedInventory.Add(Crop.CropType.SPECITE, 0);
    }

    IEnumerator DreamTick()
    {
        while (true)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
                g.GetComponent<Building>().UpdateSprite();

            CountBuildings();
            ResourceTick();

            if (sleepState == 1)
            {
                sleepTime += dreamTimeScale;
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }
    public void ResourceTick()
    {
        //Limits
        rest_limit = rest_limit_base + (buildings_dreamEngines * 1000f * Mathf.Pow(1.5f, UpgradeManager.dreamEngine_upgrade.level));
        energy_limit = energy_limit_base + (buildings_fisheries * 100f * Mathf.Pow(1.8f, UpgradeManager.fishery_upgrade.level));
        zees_earnLimit = zees_earnLimit_base + (buildings_crystariums * 2000f * Mathf.Pow(1.5f, UpgradeManager.crystarium_upgrade.level));

        //Calculate Required Energy Upkeep 
        energy_upkeep = (buildings_dreamMachines + buildings_factories);
        float energyUpkeepThisTick = energy_upkeep / 60f * dreamTimeScale;

        if (sleepState == 1)
        {
            //WHEN SLEEPING ONLY

            //Add Energy
            if (energy_resource < energy_limit)
            {
                float rest_perMin = (1 / 10f) / 60f;

                float energy_perBakery = (1f) / 60f; //1 per Minute

                energy_EarnMultiplier = Mathf.Pow(1.1f, buildings_foundries) * Mathf.Pow(1.25f, UpgradeManager.foundry_efficiency.level);

                float energy_gained = (buildings_bakeries * energy_perBakery * Mathf.Pow(1.05f, UpgradeManager.bakery_efficiency.level));
                energy_gained *= energy_EarnMultiplier;

                energy_resource += energy_gained * dreamTimeScale;
            }

        }

        //Add Rests
        if (rest_resource < rest_limit)
        {
            float rest_perMin = (1 / 10f) / 60f;

            float rest_perDreamMachine = (1f) / 60f;
            float rest_perFactory = (10f) / 60f;

            float rest_gained = (buildings_dreamMachines * rest_perDreamMachine * Mathf.Pow(1.1f, UpgradeManager.dreamMachine_efficiency.level))
                + (buildings_factories * rest_perFactory * Mathf.Pow(1.03f, UpgradeManager.factory_efficiency.level)) + rest_perMin;

            rest_gained *= rest_EarnMultiplier;

            if (sleepState == 1 || energy_resource > 0)
            {
                //ADD REST WHEN SLEEPING OR WHEN THERE IS SUFFICIENT ENERGY
                rest_resource += rest_gained * dreamTimeScale;
            }
        }

        //Cap Rest
        if (rest_resource > rest_limit)
            rest_resource = rest_limit;
        if (rest_resource < 0)
            rest_resource = 0;

        //Cap Energy
        if (energy_resource > energy_limit)
            energy_resource = energy_limit;

        //Main Zees Gained
        zee_FromRestEarnMultiplier = Mathf.Pow((1.2f) + (0.01f * UpgradeManager.refinery_efficiency.level), buildings_refineries);

        double zees_perRest = (1f) / (60f * 60f); //1 per Rest per 1 hour
        double zees_gained = (rest_resource * zees_perRest);
        zees_gained *= zee_FromRestEarnMultiplier;

        double zees_gained_capped = System.Math.Min(zees_gained, zees_earnLimit / (60f * 60f));
        zee_earningPerMinute = (zees_gained_capped * 60f);

        if (sleepState == 0)
        {
            //SUBTRACT REST (Lose 2% every hour so 24 hours = 48% lost)
            if (rest_resource > 0)
                rest_resource -= (rest_resource * (0.01f / (60f * 60f))) * dreamTimeScale;
            if (rest_resource < 0)
                rest_resource = 0;

            //SUBTRACT ENERGY UPKEEP
            if (energy_resource > 0)
            {
                energy_resource -= energyUpkeepThisTick;
                if (energy_resource < 0) energy_resource = 0;
            }
            //GAIN ZEE DURING WAKING TIME
            zees += zees_gained_capped * dreamTimeScale;
        }
    }

    void Update()
    {
        MouseOnUI = false;
        if (Input.touchSupported && Input.touchCount > 0)
            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    MouseOnUI = true;
                }
            }
        else
            MouseOnUI = eventSystem.IsPointerOverGameObject();

        if (!MouseOnUI)
            CameraPanner();
        else
            dragging = false;

        if (Input.GetKeyDown(KeyCode.P))
            Application.Quit();
    }
    public void StartSleeping()
    {
        sleepTime = 0;
        sleepState = 1;
    }
    public void StopSleeping()
    {
        //Add Zees
        zees += zee_earningPerMinute * (sleepTime / 60f);

        //Clean Up Creatures
        GameObject[] aliveCreatures = GameObject.FindGameObjectsWithTag("Creature");
        if (aliveCreatures.Length >= 5)
            for (int i = aliveCreatures.Length; i > 5; i--)
                Destroy(aliveCreatures[i]);

        //Unenergize Buildings
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
        {
            Building b = g.GetComponent<Building>();
            b.energized = false;
        }

        sleepState = 0;
    }
    public void CompleteWakeUp()
    {
        if (Random.Range(0, 100) <= 50)
            creatureManager.SpawnNewCreature();
    }

    public void BuildBuilding(Building building, Building.BuildingType type)
    {
        float price = Building.GetPrice(type);

        if (zees >= price)
        {
            zees -= price;

            uiManager.buildPanel.SetActive(false);
            building.BuildAs(type);
        }
    }

    public int buildings_dreamMachines = 0;
    public int buildings_factories = 0;
    public int buildings_bakeries = 0;
    public int buildings_refineries = 0;
    public int buildings_foundries = 0;

    public int buildings_dreamEngines = 0;
    public int buildings_fisheries = 0;
    public int buildings_crystariums = 0;

    public bool buildings_haveWorkshop = false;
    public bool buildings_haveGardenShed = false;

    public void CountBuildings()
    {
        buildings_dreamMachines = CountBuildingsOfType(Building.BuildingType.DREAM_MACHINE);
        buildings_factories = CountBuildingsOfType(Building.BuildingType.FACTORY);
        buildings_bakeries = CountBuildingsOfType(Building.BuildingType.BAKERY);
        buildings_refineries = CountBuildingsOfType(Building.BuildingType.REFINERY);
        buildings_foundries = CountBuildingsOfType(Building.BuildingType.FOUNDRY);

        buildings_dreamEngines = CountBuildingsOfType(Building.BuildingType.DREAM_ENGINE);
        buildings_fisheries = CountBuildingsOfType(Building.BuildingType.FISHERY);
        buildings_crystariums = CountBuildingsOfType(Building.BuildingType.CRYSTARIUM);

        buildings_haveWorkshop = CountBuildingsOfType(Building.BuildingType.WORKSHOP) > 0;
        buildings_haveGardenShed = CountBuildingsOfType(Building.BuildingType.GARDEN_SHED) > 0;
    }
    public int CountBuildingsOfType(Building.BuildingType type)
    {
        int c = 0;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
        {
            Building b = g.GetComponent<Building>();
            if (b.type == type && !b.underConstruction)
            {
                c++;
                if (b.energized)
                    c++;
            }
        }
        return c;
    }

    bool dragging = false;
    Vector3 lastPos = Vector3.zero;
    int lastTouchCount = 0;
    public static bool CamPan_JustReleased = false;
    float moveDist = 0;
    float touchTime = 0;
    float releaseResetTime = 0;
    void CameraPanner()
    {
        if (!dragging)
        {
            touchTime = 0;
            moveDist = 0;
            if (Input.touchSupported && Input.touchCount > 0)
            {
                Vector3 sP = Input.GetTouch(0).position;
                lastPos = Camera.main.ScreenToWorldPoint(sP);
                CamPan_JustReleased = false;
                dragging = true;
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 sP = Input.mousePosition;
                lastPos = Camera.main.ScreenToWorldPoint(sP);
                CamPan_JustReleased = false;
                dragging = true;
            }
            if (releaseResetTime > 0)
            {
                if ((releaseResetTime -= Time.unscaledTime) <= 0)
                    CamPan_JustReleased = false;
            }
        }
        else
        {
            touchTime += Time.deltaTime;

            if (touchTime > 1f || moveDist > 0.001f)
            {
                CamPan_JustReleased = true;
                releaseResetTime = 0.1f;
            }

            if (!Input.GetKey(KeyCode.Mouse0) && (!Input.touchSupported || Input.touchCount <= 0))
            {
                dragging = false;
            }
            else
            {
                Vector3 sP = Vector3.zero;
                Vector3 nowPos = Vector3.zero;

                if (Input.touchSupported && Input.touchCount == 2)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    Vector2 t1d = touch1.position - touch1.deltaPosition;
                    Vector2 t2d = touch2.position - touch2.deltaPosition;

                    float nD = Vector2.Distance(touch1.position, touch2.position);
                    float oD = Vector2.Distance(t1d, t2d);

                    Camera.main.orthographicSize += (oD - nD) * 0.01f;
                }

                if (Input.touchSupported && Input.touchCount > 0)
                {
                    sP = Input.GetTouch(0).position;
                    nowPos = Camera.main.ScreenToWorldPoint(sP);
                    if (lastTouchCount != Input.touchCount)
                    {
                        lastPos = nowPos;
                    }
                }
                else
                {
                    sP = Input.mousePosition;
                    nowPos = Camera.main.ScreenToWorldPoint(sP);
                }

                Camera.main.transform.position = Camera.main.transform.position - (nowPos - lastPos);
                moveDist += (nowPos - lastPos).magnitude;

                lastPos = Camera.main.ScreenToWorldPoint(sP);

            }
        }

        float scroll = -Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += scroll * 10f;

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 7);

        float camLimit = 7;
        Vector3 cV = Camera.main.transform.position;
        cV.z = -10;
        cV.x = Mathf.Clamp(cV.x, -camLimit, camLimit);
        cV.y = Mathf.Clamp(cV.y, -camLimit, camLimit);
        Camera.main.transform.position = cV;

        lastTouchCount = Input.touchCount;
    }

    Vector3 _panToTarget;
    float _panTime = 0.5f;
    public void CamPan_SmoothToTarget(Vector3 tar, float tt)
    {
        _panToTarget = tar;
        _panTime = tt;
        StartCoroutine("CamSmoothPanToTarget", tt);
    }

    public void CamPan_SmoothToTarget(Vector3 tar)
    {
        CamPan_SmoothToTarget(tar, 0.5f);
    }

    IEnumerator CamSmoothPanToTarget(float dur)
    {
        float t = 0;
        Vector3 ve = Vector3.zero;
        while (true)
        {
            t += Time.unscaledDeltaTime;

            _panToTarget.z = Camera.main.transform.position.z;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, _panToTarget, ref ve, dur, 10000f, Time.unscaledDeltaTime);

            if (t > dur + 1f)
                break;
            yield return null;
        }
    }
    IEnumerator CamPan_ReleaseCooldown()
    {
        CamPan_JustReleased = true;
        yield return new WaitForSecondsRealtime(0.1f);
        CamPan_JustReleased = false;
    }

    public static string FormatMoney(double money)
    {
        if (money >= 100000000)
            return (money / 1000000D).ToString("0.#M");

        if (money >= 1000000)
            return (money / 1000000D).ToString("0.##M");

        if (money >= 100000)
            return (money / 1000D).ToString("0.#k");

        if (money >= 10000)
            return (money / 1000D).ToString("0.##k");


        return System.Math.Floor(money).ToString("#,0");
    }

    public static string FormatNumber(float num)
    {
        if (num >= 100000000)
            return (num / 1000000D).ToString("0.#M");

        if (num >= 1000000)
            return (num / 1000000D).ToString("0.##M");

        if (num >= 100000)
            return (num / 1000D).ToString("0.#k");

        if (num >= 10000)
            return (num / 1000D).ToString("0.##k");


        return Mathf.FloorToInt(num).ToString();
    }
}

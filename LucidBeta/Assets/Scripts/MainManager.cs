using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public static UIManager uiManager;

    public EventSystem eventSystem;
    public GameObject buildingPrefab;
    public GameObject creaturePrefab;

    public static bool MouseOnUI;

    public static float dreamTimeScale = 3600f;

    public int sleepState = 0;
    public float sleepTime = 0;

    public double zees = 0;
    public double zees_earnLimit = 0;
    public double zees_earnLimit_base = 1000;

    public float rest_resource = 0;
    public float rest_limit = 0;
    public float rest_limit_base = 1000;

    public float energy_resource = 0;
    public float energy_limit = 0;
    public float energy_limit_base = 500;

    float zee_FromRestEarnMultiplier = 1f;
    float rest_EarnMultiplier = 1f;
    float energy_EarnMultiplier = 1f;

    void Start()
    {
        instance = this;
        ResetResources();
        StartCoroutine("DreamTick");
    }
    void ResetResources()
    {
        zees = 10000000;
        zees_earnLimit_base = 5000;

        rest_resource = 0;
        rest_limit_base = 1000;

        energy_resource = 0;
        energy_limit_base = 500;
    }

    IEnumerator DreamTick()
    {
        while (true)
        {
            CountBuildings();
            ResourceTick();

            if (sleepState == 1)
            {
                sleepTime += dreamTimeScale;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    public void ResourceTick()
    {
        //Limits
        rest_limit = rest_limit_base + (buildings_dreamEngines * 500f);
        energy_limit = energy_limit_base + (buildings_fisheries * 50f);
        zees_earnLimit = zees_earnLimit_base + (buildings_crystariums * 5000f);

        if (sleepState == 1)
        {
            //Add Energy
            if (energy_resource < energy_limit)
            {
                float energy_perBakery = (1f) / 60f; //1 per Minute

                energy_EarnMultiplier = Mathf.Pow(1.1f, buildings_foundries);

                float energy_gained = (buildings_bakeries * energy_perBakery);
                energy_gained *= energy_EarnMultiplier;

                energy_resource += energy_gained * dreamTimeScale;
            }

            //Add Rests
            if (rest_resource < rest_limit)
            {
                float rest_perDreamMachine = (1f) / 60f;
                float rest_perFactory = (10f) / 60f;

                float rest_gained = (buildings_dreamMachines * rest_perDreamMachine)
                    + (buildings_factories * rest_perFactory);

                rest_gained *= rest_EarnMultiplier;

                rest_resource += rest_gained * dreamTimeScale;
            }
        }

        //Cap Rest
        if (rest_resource > rest_limit)
            rest_resource = rest_limit;

        //Cap Energy
        if (energy_resource > energy_limit)
            energy_resource = energy_limit;

        //Main Zees Gained
        zee_FromRestEarnMultiplier = Mathf.Pow(1.1f, buildings_refineries);

        if (sleepState == 1)
        {
            double zees_perRest = (1f) / (60f * 60f); //1 per Rest per 1 hour
            double zees_gained = (rest_resource * zees_perRest);
            zees_gained *= zee_FromRestEarnMultiplier;
            zees += System.Math.Min(zees_gained, zees_earnLimit) * dreamTimeScale;
        }
    }

    void Update()
    {
        MouseOnUI = eventSystem.IsPointerOverGameObject();
        CameraPanner();
    }
    public void StartSleeping()
    {
        sleepTime = 0;
        sleepState = 1;
    }
    public void StopSleeping()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Creature"))
        {
            Destroy(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
        {
            Building b = g.GetComponent<Building>();
            b.energized = false;
        }

        sleepState = 0;
    }
    public void CompleteWakeUp()
    {
        SpawnCreature();
        if (Random.Range(0, 100) < 50) SpawnCreature();
        if (Random.Range(0, 100) < 50) SpawnCreature();
    }
    void SpawnCreature()
    {
        float rg = 4;
        Instantiate(creaturePrefab, new Vector3(Random.Range(-rg, rg), Random.Range(-rg, rg), 0), Quaternion.identity);
    }

    public void BuildBuilding(Building.BuildingType type)
    {
        GameObject b = Instantiate(buildingPrefab);
        Building bS = b.GetComponent<Building>();
        bS.type = type;
        bS.placing = true;
    }

    public int buildings_dreamMachines = 0;
    public int buildings_factories = 0;
    public int buildings_bakeries = 0;
    public int buildings_refineries = 0;
    public int buildings_foundries = 0;

    public int buildings_dreamEngines = 0;
    public int buildings_fisheries = 0;
    public int buildings_crystariums = 0;

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
    }
    int CountBuildingsOfType(Building.BuildingType type)
    {
        int c = 0;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
        {
            Building b = g.GetComponent<Building>();
            if (b.type == type && !b.underConstruction)
                c++;
            if (b.energized)
                c++;
        }
        return c;
    }

    Vector3 CamPan_startPos;
    float CamPan_totalMove = 0;
    bool CamPan_Dragging = false;
    public static bool CamPan_JustReleased = false;
    void CameraPanner()
    {
        if (!MouseOnUI)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                CamPan_startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CamPan_Dragging = true;
                CamPan_totalMove = 0;
            }
        }
        if (CamPan_Dragging)
        {
            Vector3 CamPan_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 PanDelta = (CamPan_pos - CamPan_startPos);
            CamPan_totalMove += PanDelta.sqrMagnitude;
            Camera.main.transform.position -= PanDelta;
            CamPan_JustReleased = true;
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                CamPan_Dragging = false;
                if (CamPan_totalMove > 0.01f)
                    StartCoroutine("CamPan_ReleaseCooldown");
                else
                    CamPan_JustReleased = false;
            }
        }
    }
    IEnumerator CamPan_ReleaseCooldown()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        CamPan_JustReleased = false;
    }
}

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

    public static float dreamTimeScale = 3600f;

    public int sleepState = 0;

    public double zees = 0;
    public double zees_dailyLimit = 0;

    public float rest_resource = 0;

    public float energy_resource = 0;
    public float energy_max = 0;

    float zee_FromRestEarnMultiplier = 1f;
    float rest_EarnMultiplier = 1f;

    public int buildings_factories = 0;

    void Start()
    {
        instance = this;
        ResetResources();
        StartCoroutine("DreamTick");
    }
    void ResetResources()
    {
        zees = 10000000;
        zees_dailyLimit = 5000;

        rest_resource = 0;
        energy_resource = 0;
        energy_max = 1000;

    }

    IEnumerator DreamTick()
    {
        while (true)
        {
            CountBuildings();

            if (sleepState == 1)
                ResourceTick();

            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        CameraPanner();
    }
    public void startSleeping()
    {
        sleepState = 1;
    }
    public void stopSleeping()
    {
        sleepState = 0;
    }

    public void BuildBuilding(Building.BuildingType type)
    {
        GameObject b = Instantiate(buildingPrefab);
        Building bS = b.GetComponent<Building>();
        bS.type = type;
        bS.placing = true;
    }

    public void CountBuildings()
    {
        buildings_factories = CountBuildingsOfType(Building.BuildingType.FACTORY);
    }
    int CountBuildingsOfType(Building.BuildingType type)
    {
        int c = 0;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
        {
            Building b = g.GetComponent<Building>();
            if (b.type == type && !b.underConstruction)
                c++;
        }
        return c;
    }

    public void ResourceTick()
    {
        //Calculate Multipliers

        //Add Rests
        float rest_perFactories = (buildings_factories * 0.017f);
        float rest_gained = rest_perFactories * rest_EarnMultiplier;

        rest_resource += rest_gained * dreamTimeScale;

        //Main Zees Gained
        float zees_gained = rest_resource * zee_FromRestEarnMultiplier;

        zees += zees_gained * dreamTimeScale;
    }


    Vector3 CamPan_startPos;
    float CamPan_totalMove = 0;
    bool CamPan_Dragging = false;
    public static bool CamPan_JustReleased = false;
    void CameraPanner()
    {
        if (!eventSystem.IsPointerOverGameObject())
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

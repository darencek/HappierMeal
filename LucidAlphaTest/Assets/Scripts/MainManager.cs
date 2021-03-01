using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public int dreamPoints = 0;

    bool isSleeping = false;
    float sleepTime = 0;

    public GameObject prefab_building_dreamMachine;
    public GameObject prefab_building_factory;

    public GameObject prefab_building_dreamTree;

    public GameObject prefab_buildingTest;

    public GameObject prefab_creatureTest;

    public delegate void BuildingTicksDelegate();
    public BuildingTicksDelegate buildingTicks;

    //public List<GameObject> activeCreatures = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSleeping)
        {
            sleepTime += Time.deltaTime;
            UIManager.instance.updateSleepScreen((int)sleepTime);
        }
    }
    public void Sleep_StartSleeping()
    {
        sleepTime = 0f;
        isSleeping = true;
    }
    public void Sleep_StopSleeping(out string rating, out int pointsGained)
    {
        rating = "WELL";
        pointsGained = 0;

        isSleeping = false;

        GameObject[] crs = GameObject.FindGameObjectsWithTag("Creature");
        foreach (GameObject j in crs)
        {
            Destroy(j);
        }

        float hoursSlept = sleepTime;
        if (hoursSlept < 3f)
            rating = "NOT GREAT";
        else if (hoursSlept < 5f)
            rating = "GOOD";
        else if (hoursSlept < 6f)
            rating = "WELL";
        else if (hoursSlept < 8f)
            rating = "GREAT";
        else
            rating = "WONDERFUL";

        pointsGained = AddSleepPoints((int)hoursSlept);
    }

    int AddSleepPoints(int hoursSlept)
    {
        int p = hoursSlept * 100;
        AddDreamPoints(p);
        return p;
    }

    public void AddDreamPoints(int points)
    {
        dreamPoints += points;
    }

    public int getDreamPoints()
    {
        return dreamPoints;
    }

    public bool buildBuilding(int buildingIndex)
    {
        Building b = prefab_building_dreamMachine.GetComponent<Building>();

        switch (buildingIndex)
        {
            case 0:
                //Dream Machine
                b = prefab_building_dreamMachine.GetComponent<Building>();
                break;
            case 1:
                //Factory
                b = prefab_building_factory.GetComponent<Building>();
                break;
            case 60:
                //DreamTree
                b = prefab_building_dreamTree.GetComponent<Building>();
                break;
        }

        Debug.Log("Building: " + b.name);

        if (dreamPoints < b.cost)
            return false;

        Instantiate(b);
        dreamPoints -= b.cost;

        return true;
    }

    public void spawnCreatures()
    {
        spawnCreature();
        if (Random.Range(0, 100) < 50) spawnCreature();
        if (Random.Range(0, 100) < 50) spawnCreature();
    }
    public void spawnCreature()
    {
        Instantiate(prefab_creatureTest, new Vector3(Random.Range(-8, 8), Random.Range(0, -3), 0), Quaternion.identity);
    }
}

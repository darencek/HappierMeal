using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public int dreamPoints = 0;

    bool isSleeping = false;
    float sleepTime = 0;

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
        }
    }
    public void Sleep_StartSleeping()
    {
        sleepTime = 0f;
        isSleeping = true;
    }
    public void Sleep_StopSleeping()
    {
        isSleeping = false;

        GameObject[] crs = GameObject.FindGameObjectsWithTag("Creature");
        foreach (GameObject j in crs)
        {
            Destroy(j);
        }

        AddSleepPoints((int)sleepTime);
    }

    void AddSleepPoints(int hoursSlept)
    {
        dreamPoints += hoursSlept * 100;
    }

    public void AddDreamPoints(int points)
    {
        dreamPoints += points;
    }

    public int getDreamPoints()
    {
        return dreamPoints;
    }

    public bool buildBuilding()
    {
        if (dreamPoints < Building.cost)
            return false;

        Instantiate(prefab_buildingTest);
        dreamPoints -= Building.cost;

        return true;
    }

    public void spawnCreature()
    {
        Instantiate(prefab_creatureTest, new Vector3(Random.Range(-8, 8), Random.Range(0, -3), 0), Quaternion.identity);
    }
}

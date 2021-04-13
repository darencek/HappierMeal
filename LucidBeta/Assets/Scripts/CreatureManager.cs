using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public GameObject creaturePrefab;

    public Sprite[] creatureSprites;
    public GameObject[] creatureSpawnLocations;

    public List<Creature.CreatureType> encountered;

    public float hoursSleptForSpawn = 0;
    public float spawnTime = 0;

    int maxCreatures = 6;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = (12 * 60 * 60);

        MainManager.creatureManager = this;
        encountered = new List<Creature.CreatureType>();
    }

    void Update()
    {
        if (MainManager.instance.sleepState == 1)
        {
            hoursSleptForSpawn += Time.deltaTime * MainManager.dreamTimeScale;
        }
    }

    public void CheckSpawn()
    {
        if (hoursSleptForSpawn >= spawnTime)
        {
            hoursSleptForSpawn = 0;
            if (Random.Range(0, 100) <= 90)
                SpawnNewCreature((Creature.CreatureType)Random.Range(0, System.Enum.GetNames(typeof(Creature.CreatureType)).Length));

        }
    }
    public Sprite GetCreatureSprite(Creature.CreatureType type)
    {
        return creatureSprites[(int)type];
    }

    public void SpawnNewCreature(Creature.CreatureType t)
    {
        GameObject[] crs = GameObject.FindGameObjectsWithTag("Creature");
        if (crs.Length > maxCreatures)
            for (int i = 0; i < (crs.Length - maxCreatures); i++)
                Destroy(crs[i]);

        GameObject spawnLoc = creatureSpawnLocations[Random.Range(0, creatureSpawnLocations.Length)];
        GameObject g = Instantiate(creaturePrefab, spawnLoc.transform.position, Quaternion.identity);
        g.GetComponent<CreatureController>().SetType(t);
    }

    public void CheckNewEncounter(CreatureController c)
    {
        if (!CheckEncountered(c.type))
        {
            encountered.Add(c.type);
            MainManager.instance.revealEvents.Add(c.gameObject);
        }
    }
    public bool CheckEncountered(Creature.CreatureType t)
    {
        return encountered.Contains(t);
    }

}

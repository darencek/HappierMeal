using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    public GameObject creaturePrefab;

    public Sprite[] creatureSprites;
    public GameObject[] creatureSpawnLocations;

    public List<Creature.CreatureType> encountered;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.creatureManager = this;
        encountered = new List<Creature.CreatureType>();
    }
    public Sprite GetCreatureSprite(Creature.CreatureType type)
    {
        return creatureSprites[(int)type];
    }

    public void SpawnNewCreature()
    {
        GameObject spawnLoc = creatureSpawnLocations[Random.Range(0, creatureSpawnLocations.Length)];
        Instantiate(creaturePrefab, spawnLoc.transform.position, Quaternion.identity);
    }

    public void CheckNewEncounter(CreatureController c)
    {
        if (!CheckEncountered(c.type))
        {
            encountered.Add(c.type);
            _newEncounter = c;
            StartCoroutine("NewEncounterSequence");
        }
    }
    public bool CheckEncountered(Creature.CreatureType t)
    {
        return encountered.Contains(t);
    }

    CreatureController _newEncounter;
    IEnumerator NewEncounterSequence()
    {
        Time.timeScale = 0f;
        MainManager.instance.CamPan_SmoothToTarget(_newEncounter.transform.position);
        yield return new WaitForSecondsRealtime(2f);
        MainManager.uiManager.UI_ShowNewCreaturePopup(_newEncounter);
        Time.timeScale = 1f;
        yield return null;
    }
}

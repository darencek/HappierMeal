using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public enum BuildingType { DREAM_MACHINE, DREAM_ENGINE, FACTORY, REFINERY, FOUNDRY, CRYSTARIUM, GARDEN_SHED, INCUBATOR, WORKSHOP, BAKERY, FISHERY };

    public BuildingType type;

    public bool underConstruction = false;
    public bool placing = false;

    public float buildHours = 0;

    public GameObject[] buildingSprites;
    public int buildingSpriteId = 0;

    GameObject currentSprite;

    static float gridX = 1f;
    static float gridY = 0.5f;

    public Color buildRed;
    public Color buildColor;

    // Start is called before the first frame update
    void Start()
    {
        currentSprite = buildingSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprite();
        if (placing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
            snapToGrid();

            bool validPlacment = true;
            foreach (Collider2D c in Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 0.5f), 0))
            {
                if (c.gameObject == gameObject) continue;
                validPlacment = false;
            }

            if (validPlacment)
            {
                currentSprite.GetComponent<SpriteRenderer>().color = buildColor;
                if (Input.GetKeyUp(KeyCode.Mouse0) && !MainManager.CamPan_JustReleased)
                {
                    currentSprite.GetComponent<SpriteRenderer>().color = Color.white;
                    placing = false;

                    buildHours = GetBuildHours(type) * 60f * 60f;
                    underConstruction = buildHours > 0 ? true : false;
                }
            }
            else
            {
                currentSprite.GetComponent<SpriteRenderer>().color = buildRed;
            }
        }

        if (buildHours > 0 && MainManager.instance.sleepState >= 1)
        {
            buildHours -= Time.deltaTime * MainManager.dreamTimeScale;
            if (buildHours <= 0)
                underConstruction = false;
        }

    }

    void snapToGrid()
    {
        Vector3 pos = transform.position;

        pos.y = Mathf.Round(pos.y / gridY) * gridY;
        pos.x = Mathf.Round(pos.x / gridX) * gridX;


        transform.position = pos;
    }
    public void UpdateSprite()
    {
        foreach (GameObject o in buildingSprites)
            o.SetActive(false);

        int targetSpriteId = 0;
        if (underConstruction)
        {
            targetSpriteId = 0;
        }
        else
        {
            targetSpriteId = (int)type + 1;
        }
        currentSprite = buildingSprites[targetSpriteId];
        currentSprite.SetActive(true);
    }

    public static float GetPrice(BuildingType type)
    {
        return GetStat(type, 0);
    }
    public static float GetBuildHours(BuildingType type)
    {
        return GetStat(type, 1);
    }

    static float GetStat(BuildingType type, byte statId)
    {
        float price = 0;
        float buildHours = 1;

        switch (type)
        {
            case BuildingType.DREAM_MACHINE:
                price = 1000;
                buildHours = 1;
                break;
            case BuildingType.DREAM_ENGINE:
                price = 5000;
                buildHours = 5;
                break;
            case BuildingType.FACTORY:
                price = 25000;
                buildHours = 5;
                break;
            case BuildingType.REFINERY:
                price = 50000;
                buildHours = 8;
                break;
            case BuildingType.FOUNDRY:
                price = 75000;
                buildHours = 8;
                break;
            case BuildingType.WORKSHOP:
                price = 20000;
                buildHours = 3;
                break;
        }

        switch (statId)
        {
            case 0:
                return price;
            case 1:
                return buildHours;
        }

        return 0;
    }
}

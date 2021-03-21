using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpriteManager : MonoBehaviour
{
    public Sprite[] buildingSprites;

    // Start is called before the first frame update
    void Start()
    {
        MainManager.buildingSpriteManager = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Sprite GetBuildingSprite(Building.BuildingType type)
    {
        return buildingSprites[(int)type];
    }
}

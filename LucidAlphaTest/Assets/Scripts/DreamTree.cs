using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamTree : BuildingScript
{
    public int fruits = 0;
    public int pointsAdd = 100;
    public GameObject[] fruitObjs;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < fruitObjs.Length; i++)
        {
            fruitObjs[i].SetActive(i < fruits);
        }
    }

    private void OnMouseDown()
    {
        if (fruits > 0)
        {
            fruits--;
            MainManager.instance.AddDreamPoints(pointsAdd);
        }
    }

    public override void Tick()
    {
        if (fruits < fruitObjs.Length)
            fruits++;
    }
}

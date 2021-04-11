using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoomanManager : MonoBehaviour
{
    public GameObject hoomanPrefab;

    List<GameObject> hoomans = new List<GameObject>();


    void Update()
    {
        int pCount = MainManager.instance.buildings_total * MainManager.instance.buildings_total;
        pCount = Mathf.Clamp(pCount, 0, 40);
        if (pCount > hoomans.Count)
        {
            GameObject g = Instantiate(hoomanPrefab);
            hoomans.Add(g);
        }
        if (pCount < hoomans.Count)
        {
            GameObject h = hoomans[0];
            hoomans.RemoveAt(0);
            Destroy(h);
        }
    }
}


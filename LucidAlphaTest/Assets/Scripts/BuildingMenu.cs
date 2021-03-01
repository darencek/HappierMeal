using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    public GameObject[] panels;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchPanel(int p)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == p);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    public GameObject blocker;

    public GameObject mainTree;
    public GameObject pan1Target;

    public GameObject welcomePanel1;
    public GameObject welcomePanel2;
    public GameObject welcomePanel3;
    public GameObject welcomePanel4;

    public int tutorialState = 0;
    // Start is called before the first frame update
    void Start()
    {
        welcomePanel1.SetActive(true);
        blocker.SetActive(true);

        MainManager.instance.CamPan_SmoothToTarget(mainTree.transform.position, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialState == 1)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
            {
                Building b = g.GetComponent<Building>();
                if (b.type == Building.BuildingType.DREAM_MACHINE)
                {
                    tutorialState = 2;
                    welcomePanel3.SetActive(true);
                    break;
                }
            }
            return;
        }
        if (tutorialState == 2)
        {
            if (MainManager.instance.sleepState == 1)
            {
                tutorialState = 3;
                welcomePanel3.SetActive(false);
            }
            return;
        }
    }

    public void Welcome_ShowCloud()
    {
        StartCoroutine("_c1");
    }

    public void panel2_close()
    {
        welcomePanel2.SetActive(false);
        blocker.SetActive(false);
    }

    IEnumerator _c1()
    {
        MainManager.instance.CamPan_SmoothToTarget(pan1Target.transform.position, 0.5f);
        yield return new WaitForSecondsRealtime(2f);
        welcomePanel2.SetActive(true);
        tutorialState = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI pointsText;
    public GameObject sleepMenu;
    public GameObject buildMenu;
    public GameObject sleepingScreen;
    public GameObject wakeMenu;

    public GameObject noPointsError;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = MainManager.instance.getDreamPoints() + " z";
    }
    public void UI_ToggleBuildMenu()
    {
        sleepMenu.SetActive(false);
        buildMenu.SetActive(!buildMenu.activeSelf);
    }
    public void UI_ToggleSleepMenu()
    {
        buildMenu.SetActive(false);
        sleepMenu.SetActive(!sleepMenu.activeSelf);
    }
    public void UI_Sleep_Confirm()
    {
        sleepMenu.SetActive(false);
        buildMenu.SetActive(false);
        sleepingScreen.SetActive(true);

        MainManager.instance.Sleep_StartSleeping();
    }
    public void UI_Sleep_WakeUp()
    {
        sleepingScreen.SetActive(false);
        wakeMenu.SetActive(true);

        MainManager.instance.Sleep_StopSleeping();
    }
    public void UI_WakeUp_Exit()
    {
        wakeMenu.SetActive(false);
        MainManager.instance.spawnCreature();
    }

    public void UI_Build_TestBuilding()
    {
        if (MainManager.instance.buildBuilding())
        {
            buildMenu.SetActive(false);
        }
        else
        {
            StartCoroutine("noPointsErrorFlash");
        }
    }

    IEnumerator noPointsErrorFlash()
    {
        noPointsError.SetActive(true);
        yield return new WaitForSeconds(1f);
        noPointsError.SetActive(false);
    }
}
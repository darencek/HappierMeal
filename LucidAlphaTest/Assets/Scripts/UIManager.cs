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

    public GameObject dialogueBox;

    public GameObject noPointsError;

    public TextMeshProUGUI wake_ratingText;
    public TextMeshProUGUI sleep_hoursText;

    public static UIManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = MainManager.instance.getDreamPoints() + " z";
    }

    public void updateSleepScreen(int hours)
    {
        sleep_hoursText.text = hours + " hours slept...";
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
        wakeMenu.GetComponentInChildren<WordCloudManager>().StartWakeUpQuiz();

        string sleepRating = "";
        int pointsGained = 0;

        MainManager.instance.Sleep_StopSleeping(out sleepRating, out pointsGained);

        wake_ratingText.text = "You slept: " + sleepRating + "\nEarned: " + pointsGained;
    }
    public void UI_WakeUp_Exit()
    {
        wakeMenu.SetActive(false);
        MainManager.instance.spawnCreatures();
    }

    public void UI_Build_TestBuilding(int buildingIndex)
    {
        bool success = MainManager.instance.buildBuilding(buildingIndex);

        if (success)
        {
            buildMenu.SetActive(false);
        }
        else
        {
            StartCoroutine("noPointsErrorFlash");
        }
    }

    public void UI_ShowDialogueBox()
    {
        dialogueBox.SetActive(true);
    }

    IEnumerator noPointsErrorFlash()
    {
        noPointsError.SetActive(true);
        yield return new WaitForSeconds(1f);
        noPointsError.SetActive(false);
    }
}
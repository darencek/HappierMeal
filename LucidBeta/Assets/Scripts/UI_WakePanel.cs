using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_WakePanel : MonoBehaviour
{
    public TextAsset wordsListFile;

    public TextMeshProUGUI word1;
    public TextMeshProUGUI word2;

    public TextMeshProUGUI ratingText;

    List<string> wordsList;
    List<string> wordsList_work;
    bool wordsLoaded = false;

    int answerCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        LoadWords();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartWakeUp(float sleepTime)
    {

        string rating = "GOOD";
        float hoursSlept = sleepTime / (60f * 60f);
        if (hoursSlept < 3f)
            rating = "NOT GREAT";
        else if (hoursSlept < 5f)
            rating = "GOOD";
        else if (hoursSlept < 6f)
            rating = "WELL";
        else if (hoursSlept < 8f)
            rating = "GREAT";
        else
            rating = "WONDERFUL";

        ratingText.text = "Sleep duration: " + Mathf.FloorToInt(hoursSlept) + " hours\nYou slept: " + rating;

        answerCount = 0;
        RefreshQuestion();
    }

    void LoadWords()
    {
        string[] wordsArray = wordsListFile.text.Split('\n');
        wordsList = new List<string>(wordsArray);
    }
    void RefreshQuestion()
    {
        if (!wordsLoaded) LoadWords();

        word1.text = wordsList[Random.Range(0, wordsList.Count)];

        int breaker = 0;
        while ((word2.text = wordsList[Random.Range(0, wordsList.Count)]) == word1.text)
        {
            if (breaker++ > 100) break;
        }

    }

    public void UI_SelectWord()
    {
        RefreshQuestion();
        answerCount++;
        if (answerCount >= 3)
        {
            gameObject.SetActive(false);
            MainManager.instance.CompleteWakeUp();
        }
    }
}

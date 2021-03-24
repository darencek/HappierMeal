using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_WakePanel : MonoBehaviour
{
    public TextAsset wordsListFile;

    public TextAsset positiveWordsFile;
    public TextAsset negativeWordsFile;

    public TextMeshProUGUI word1;
    public TextMeshProUGUI word2;

    public TextMeshProUGUI ratingText;

    List<string> wordsList;

    List<string> positiveWords;
    List<string> negativeWords;

    bool wordsLoaded = false;

    int answerCount = 0;
    // Start is called before the first frame update
    void Awake()
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
        else if (hoursSlept < 7f)
            rating = "GREAT";
        else if (hoursSlept < 10f)
            rating = "WONDERFUL";
        else
            rating = "OVERSLEPT";

        ratingText.text = "Sleep duration: " + Mathf.FloorToInt(hoursSlept) + " hours\nYou slept: " + rating;

        LoadWords();

        answerCount = 0;
        RefreshQuestion();
    }

    void LoadWords()
    {
        string[] positiveWordsArray = positiveWordsFile.text.Split('\n');
        string[] negativeWordsArray = negativeWordsFile.text.Split('\n');

        positiveWords = new List<string>(positiveWordsArray);
        negativeWords = new List<string>(negativeWordsArray);

        wordsLoaded = true;
    }

    void RefreshQuestion()
    {
        if (!wordsLoaded) LoadWords();

        string w1 = positiveWords[Random.Range(0, positiveWords.Count)];
        string w2 = negativeWords[Random.Range(0, negativeWords.Count)];

        positiveWords.Remove(w1);
        negativeWords.Remove(w2);

        if (Random.Range(0, 100) < 50)
        {
            word1.text = w1;
            word2.text = w2;
        }
        else
        {
            word1.text = w2;
            word2.text = w1;
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

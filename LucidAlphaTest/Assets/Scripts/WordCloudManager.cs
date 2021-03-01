using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCloudManager : MonoBehaviour
{
    public UIManager UImanager;
    public GameObject wordPrefab;
    public TextAsset raw_moodList;

    List<string> selectedMoods;

    public List<GameObject> spawnedWords;

    public string[] moods;

    public int responsesLeft = 3;

    // Start is called before the first frame update
    void Start()
    {
        spawnedWords = new List<GameObject>();
        StartWakeUpQuiz();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartWakeUpQuiz()
    {
        moods = raw_moodList.text.Split('\n');

        selectedMoods = new List<string>(moods);
        responsesLeft = 3;
        SpawnWords();
    }

    public void SelectResponse()
    {
        responsesLeft--;
        SpawnWords();

        if (responsesLeft <= 0)
            UImanager.UI_WakeUp_Exit();
    }

    public void SpawnWords()
    {
        foreach (GameObject k in spawnedWords)
            Destroy(k);

        spawnedWords.Clear();


        int wordCount = 3;

        List<string> displayMoods = new List<string>();

        for (int i = 0; i < wordCount; i++)
        {
            string w = selectedMoods[Random.Range(0, selectedMoods.Count - 1)];
            displayMoods.Add(w);
            selectedMoods.Remove(w);
        }

        int r = 0;
        int c = 0;
        int maxWidth = 3;

        float wordWidth = 420f;
        float wordHeight = 200f;

        float startX = -(maxWidth * wordWidth) / 2 + (wordWidth / 2);
        float startY = 0;
        foreach (string mood in displayMoods)
        {
            GameObject g = Instantiate(wordPrefab, gameObject.transform);
            g.transform.localPosition = new Vector3(startX + wordWidth * c, startY - wordHeight * r, 0);
            g.GetComponent<WordCloudButton>().SetWord(mood, this);

            if (++c >= maxWidth)
            {
                c = 0;
                r++;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCloudManager : MonoBehaviour
{
    public UIManager UImanager;
    public GameObject wordPrefab;
    public TextAsset raw_moodList;

    public List<WordCloudButton> selectedWords;

    public string[] moods;
    // Start is called before the first frame update
    void Start()
    {
        selectedWords = new List<WordCloudButton>();

        moods = raw_moodList.text.Split('\n');
        Debug.Log(moods.Length + " mood words loaded");
        SpawnWords();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void doneButtonPressed()
    {
        if (selectedWords.Count < 3) return;

        selectedWords.Clear();
        UImanager.UI_WakeUp_Exit();
    }
    public bool isWordSelected(WordCloudButton word)
    {
        return selectedWords.Contains(word);
    }
    public void selectWord(WordCloudButton word)
    {
        if (selectedWords.Contains(word))
        {
            selectedWords.Remove(word);
        }
        else
        {
            if (selectedWords.Count >= 3)
                selectedWords.RemoveAt(0);
            selectedWords.Add(word);
        }
    }
    public void SpawnWords()
    {
        List<string> selectedMoods = new List<string>(moods);

        int wordCount = 50;
        for (int i = 0; i < (moods.Length - wordCount); i++)
        {
            selectedMoods.RemoveAt(Random.Range(0, selectedMoods.Count));
        }


        int r = 0;
        int c = 0;
        int maxWidth = 5;

        float wordWidth = 250f;
        float wordHeight = 50f;

        float startX = -(maxWidth * wordWidth) / 2 + (wordWidth / 2);
        float startY = gameObject.GetComponent<RectTransform>().sizeDelta.y / 2 - wordHeight;
        foreach (string mood in selectedMoods)
        {
            GameObject g = Instantiate(wordPrefab, gameObject.transform);
            g.transform.localPosition = new Vector3(startX + wordWidth * c, startY - wordHeight * r, 0);
            g.GetComponent<WordCloudButton>().SetWord(mood,this);

            if (++c >= maxWidth)
            {
                c = 0;
                r++;
            }
        }
    }
}

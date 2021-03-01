using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WordCloudButton : MonoBehaviour
{
    public string word;
    public TextMeshProUGUI wordText;
    public WordCloudManager manager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //  wordText.color = manager.isWordSelected(this) ? Color.green : Color.black;
    }

    public void SetWord(string word, WordCloudManager mn)
    {
        this.word = word;
        wordText.text = word;
        manager = mn;
    }

    public void WordClicked()
    {
        manager.SelectResponse();
    }
}

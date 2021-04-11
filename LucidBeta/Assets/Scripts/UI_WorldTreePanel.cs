using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_WorldTreePanel : MonoBehaviour
{
    public GameObject progressBar;
    public TextMeshProUGUI level;
    public TextMeshProUGUI progress;

    public Image ascendButtonSprite;

    public GameObject popupWindow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        level.text = "Ascension Level " + MainManager.instance.ascensionLevel;
        progress.text = "$" + MainManager.FormatMoney(MainManager.instance.zees) + "/$" + MainManager.FormatMoney(MainManager.instance.nextAscension);

        progressBar.transform.localScale = new Vector3(Mathf.Clamp((float)(MainManager.instance.zees / MainManager.instance.nextAscension), 0, 1), 1, 1);

        if (MainManager.instance.zees >= MainManager.instance.nextAscension)
        {
            ascendButtonSprite.color = Color.white;
        }
        else
        {
            ascendButtonSprite.color = Color.grey;
        }
    }

    public void UI_Ascend()
    {
        if (MainManager.instance.zees >= MainManager.instance.nextAscension)
        {
            MainManager.instance.Ascend();
            gameObject.SetActive(false);
        }
    }

    public void UI_Close()
    {
        gameObject.SetActive(false);
    }

}

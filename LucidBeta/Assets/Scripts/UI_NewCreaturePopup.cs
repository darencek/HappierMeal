using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_NewCreaturePopup : MonoBehaviour
{

    public Image charSprite;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenPopup(CreatureController c)
    {
        charSprite.sprite = MainManager.creatureManager.GetCreatureSprite(c.type);
    }

    public void UI_Close()
    {
        gameObject.SetActive(false);
    }
}

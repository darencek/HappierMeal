using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_DialogBox : MonoBehaviour
{
    RectTransform rect;
    Vector3 hidePosition;
    Vector3 outPosition;

    Vector3 _v;
    bool showing = true;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public Image charSprite;

    CreatureController creature;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        outPosition = transform.position;
        hidePosition = outPosition + Vector3.down * rect.rect.height * 1.5f;
    }

    private void OnEnable()
    {
        transform.position = hidePosition;
        showing = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, showing ? outPosition : hidePosition, ref _v, 0.1f);
    }

    public void UI_AdvanceDialog()
    {
        CloseDialogBox();
    }

    public void OpenDialog(CreatureController c)
    {
        creature = c;
        nameText.text = c.info.name;
        charSprite.sprite = MainManager.creatureManager.GetCreatureSprite(c.type);

        dialogText.text = c.info.defaultDialog;
    }

    public void CloseDialogBox()
    {
        showing = false;
        StartCoroutine("CloseDialogBox_CR");
    }

    IEnumerator CloseDialogBox_CR()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}

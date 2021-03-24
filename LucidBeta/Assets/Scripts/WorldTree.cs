using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree : MonoBehaviour
{
    public Sprite[] treeSprites;

    SpriteRenderer ren;
    public GameObject ascendReadyParticles;
    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ren.sprite = treeSprites[Mathf.Clamp(MainManager.instance.ascensionLevel - 1, 0, treeSprites.Length - 1)];

        ascendReadyParticles.SetActive(MainManager.instance.zees >= MainManager.instance.nextAscension);
    }

    bool clicked = false;
    private void OnMouseDown()
    {
        clicked = true;
    }
    private void OnMouseUp()
    {
        if (clicked)
        {
            if (!MainManager.CamPan_JustReleased && !MainManager.MouseOnUI)
            {
                MainManager.uiManager.UI_ShowAscendPanel();
            }
        }
        clicked = false;
    }

}

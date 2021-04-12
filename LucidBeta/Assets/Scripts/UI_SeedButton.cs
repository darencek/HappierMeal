using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_SeedButton : MonoBehaviour
{
    public Crop.CropType seedType;

    public TextMeshProUGUI seedName;
    public TextMeshProUGUI seedInfo;
    public TextMeshProUGUI xbreed;
    public TextMeshProUGUI quantity;

    public Image mainSprite;

    // Start is called before the first frame update
    void Start()
    {
        Crop.CropInfo info = new Crop.CropInfo(seedType);
        seedName.text = info.name;
        seedInfo.text = info.info;
        xbreed.text = "";

        if (info.x1 != Crop.CropType.NONE && info.x2 != Crop.CropType.NONE)
        {
            xbreed.text = new Crop.CropInfo(info.x1).name + " + " + new Crop.CropInfo(info.x2).name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!MainManager.instance.SeedInventory.ContainsKey(seedType))
        {
            MainManager.instance.SeedInventory[seedType] = 0;
        } 

        float q = MainManager.instance.SeedInventory[seedType];

        quantity.text = q == -1 ? "\u221E" : q.ToString();

        if (q > 0 || q == -1)
        {
            mainSprite.color = Color.white;
        }
        else
        {
            mainSprite.color = Color.grey;
        }
    }

    public void ButtonPress()
    {
        MainManager.musicManager.PlayPop();
        MainManager.uiManager.farmPanel.GetComponent<UI_FarmPanel>().UI_SelectSeed(seedType);
    }
}

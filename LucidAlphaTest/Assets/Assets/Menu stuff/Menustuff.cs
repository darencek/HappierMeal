using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menustuff : MonoBehaviour
{
    public GameObject houseButton;
    public GameObject plantButton;
    public GameObject menuButton;

    public GameObject buildMenu;
    public GameObject plantMenu;

    bool isMenuOpen;

    public bool isPlantOn;
    public bool isBuildingOn; 


    // Start is called before the first frame update
    void Start()
    {
        menuButton.SetActive(true);
        
    }
    void Update()
    {
       if (!isMenuOpen)
        {
            // Buttons off
            houseButton.SetActive(false);
            plantButton.SetActive(false);
           
            // Additional menu off
            isPlantOn = false;
            isBuildingOn = false; 

        }
       else
        {
            houseButton.SetActive(true);
            plantButton.SetActive(true);
        }
        // ^^^ For hiding and showing build / Deco buttons  ^^^


        if (!isPlantOn)
        {
            plantMenu.SetActive(false);
        }
        else
        {
            plantMenu.SetActive(true);
            isBuildingOn = false;
        }
        // ^^^ for hiding / showing plant menu ^^^


        if (!isBuildingOn)
        {
            buildMenu.SetActive(false);
        }
        else
        {
            buildMenu.SetActive(true);
            isPlantOn = false;
        }

        // ^^^ for hiding / showing build menu ^^^
    }
    public void OpenMenu()
    {
        if (!isMenuOpen)
        {
            isMenuOpen = true;
        }
        else
        {
            isMenuOpen = false;
        }
        // ^^^ For hiding and showing build / Deco buttons  ^^^
    }
    public void OpenPlantMenu()
    {
        if (!isPlantOn)
        {
            isPlantOn = true;
        }
        else
        {
            isPlantOn = false;
        }
        // ^^^ for hiding / showing plant menu ^^^
    }
    public void OpenBuildMenu()
    {
        if (!isBuildingOn)
        {
            isBuildingOn = true; 
        }
        else
        {
            isBuildingOn = false;
        }
        // ^^^ for hiding / showing build menu ^^^
    }
}

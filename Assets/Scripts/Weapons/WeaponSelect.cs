using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public GameObject ParentRanged, ParentMelee;

    // Start is called before the first frame update
    void Start()
    {
        // Set the default weapon to the pen
        SelectPen();
    }

    // Call this method when the "SelectPen" button is pressed
    public void SelectPen()
    {
        // Enable the ParentMelee and disable the ParentRanged
        ParentMelee.SetActive(true);
        ParentRanged.SetActive(false);


    }

    // Call this method when the "SelectCalculator" button is pressed
    public void SelectCalculator()
    {
        // Disable the ParentMelee and enable the ParentRanged
        ParentMelee.SetActive(false);
        ParentRanged.SetActive(true);
        
    }
    

    // Update is called once per frame
    void Update()
    {
        // Check for key presses
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectPen();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectCalculator();
        }
    }
}


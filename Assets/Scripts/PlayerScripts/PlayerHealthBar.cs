using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
    {
    public PlayerHealth playerHealth;
    public Image FillImage;
    private Slider slider;



    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        // Check if the player's health is not in full health
        if (playerHealth.currentHealth < playerHealth.maxHealth)
        {
            // Set the fill color to red
            FillImage.color = Color.red;
        }
        else
        {
            // Set the fill color to green if the player is fully healed
            FillImage.color = Color.green;
        }

        // Calculate the fill value based on the player's health
        float fillValue = playerHealth.currentHealth;
        slider.value = fillValue;


        if (slider.maxValue != playerHealth.maxHealth)
        {
            slider.maxValue = playerHealth.maxHealth;
        }
    }
}

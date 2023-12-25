using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    public Slider experienceSlider;
    public TextMeshProUGUI expText;

    public PlayerHealth playerHealth;

    private void Start()
    {
        // Find the Slider component on the same GameObject
        experienceSlider = GetComponent<Slider>();

        // Optional: Set initial values or other settings
        // experienceSlider.minValue = 0;
        // experienceSlider.maxValue = yourMaxExperienceValue;
    }

    void Update()
    {
        // Update the Slider value based on the current experience
        float fillValue = playerHealth.currentExperience;
        experienceSlider.value = fillValue;

        // Set the maximum value of the Slider based on the max experience
        experienceSlider.maxValue = playerHealth.maxExperience;

        // Update the TextMeshProUGUI for the current level
        UpdateTextValue();
    }

    void UpdateTextValue()
    {
        expText.text = "Player Level: " + playerHealth.currentLevel;
    }
}
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
        experienceSlider = GetComponent<Slider>();
    }

    void Update()
    {
        float fillValue = playerHealth.currentExperience;
        experienceSlider.value = fillValue;
        experienceSlider.maxValue = playerHealth.maxExperience;
        UpdateTextValue();
    }

    void UpdateTextValue()
    {
        expText.text = "Player Level: " + playerHealth.currentLevel;
    }
}
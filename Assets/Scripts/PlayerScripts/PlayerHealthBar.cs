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
        if (playerHealth.currentHealth < playerHealth.maxHealth)
        {
            FillImage.color = Color.red;
        }
        else
        {
            FillImage.color = Color.green;
        }

        float fillValue = playerHealth.currentHealth;
        slider.value = fillValue;

        if (slider.maxValue != playerHealth.maxHealth)
        {
            slider.maxValue = playerHealth.maxHealth;
        }
    }
}

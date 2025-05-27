using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;
    
    [Header("Colors")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private float lowHealthThreshold = 0.3f;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        ValidateReferences();
        InitializeSlider();
    }

    private void InitializeSlider()
    {
        if (slider != null && playerHealth != null)
        {
            slider.minValue = 0;
            slider.maxValue = playerHealth.maxHealth;
            slider.value = playerHealth.CurrentHealth;
        }
    }

    private void ValidateReferences()
    {
        if (!playerHealth)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
            if (!playerHealth)
            {
                Debug.LogError("PlayerHealth reference missing on " + gameObject.name);
                enabled = false;
                return;
            }
        }

        if (!fillImage)
        {
            fillImage = transform.Find("Fill Area/Fill")?.GetComponent<Image>();
            if (!fillImage)
            {
                Debug.LogError("Fill Image reference missing on " + gameObject.name);
                enabled = false;
                return;
            }
        }

        if (!slider)
        {
            Debug.LogError("Slider component missing on " + gameObject.name);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (!playerHealth || !slider) return;

        UpdateHealthBar(playerHealth.CurrentHealth);
    }

    private void OnEnable()
    {
        // Subscribe to player level up events
        if (playerHealth)
        {
            playerHealth.OnLevelUp += HandleLevelUp;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from player level up events
        if (playerHealth)
        {
            playerHealth.OnLevelUp -= HandleLevelUp;
        }
    }

    private void HandleLevelUp()
    {
        // Update slider max value when player levels up
        if (slider && playerHealth)
        {
            slider.maxValue = playerHealth.maxHealth;
            // Update current health display
            UpdateHealthBar(playerHealth.CurrentHealth);
        }
    }

    private void UpdateHealthBar(float currentHealth)
    {
        if (!playerHealth) return;

        // Update slider value
        slider.value = currentHealth;
        
        // Calculate health percentage based on current max health
        float percentage = currentHealth / playerHealth.maxHealth;
        
        // Update fill color
        fillImage.color = percentage <= lowHealthThreshold ? 
            lowHealthColor : 
            Color.Lerp(lowHealthColor, fullHealthColor, percentage);
    }
}
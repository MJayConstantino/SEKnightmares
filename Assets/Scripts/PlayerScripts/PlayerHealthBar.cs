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
            fillImage = GetComponentInChildren<Image>();
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
        if (!playerHealth) return;

        float healthPercentage = playerHealth.GetHealthPercentage();
        UpdateHealthBar(healthPercentage);
    }

    private void UpdateHealthBar(float percentage)
    {
        slider.value = percentage;
        
        fillImage.color = percentage <= lowHealthThreshold ? 
            lowHealthColor : 
            Color.Lerp(lowHealthColor, fullHealthColor, percentage);
    }
}
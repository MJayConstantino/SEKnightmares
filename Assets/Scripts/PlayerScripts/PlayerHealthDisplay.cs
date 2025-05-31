using UnityEngine;
using TMPro;

public class PlayerHealthDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Format")]
    [SerializeField] private string format = "{0}/{1}";
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private Color normalHealthColor = Color.white;
    [SerializeField] private float lowHealthThreshold = 0.3f;

    private void Start()
    {
        ValidateReferences();
        UpdateHealthDisplay();
    }

    private void ValidateReferences()
    {
        if (!playerHealth)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
        
        if (!healthText)
        {
            healthText = GetComponent<TextMeshProUGUI>();
        }

        if (!playerHealth || !healthText)
        {
            Debug.LogError("Missing required references on PlayerHealthDisplay");
            enabled = false;
        }
    }

    private void Update()
    {
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        if (!playerHealth || !healthText) return;

        healthText.text = string.Format(format, playerHealth.CurrentHealth, playerHealth.maxHealth);
        
        float healthPercentage = playerHealth.GetHealthPercentage();
        healthText.color = healthPercentage <= lowHealthThreshold ? lowHealthColor : normalHealthColor;
    }

    private void OnEnable()
    {
        if (playerHealth)
        {
            playerHealth.OnLevelUp += UpdateHealthDisplay;
        }
    }

    private void OnDisable()
    {
        if (playerHealth)
        {
            playerHealth.OnLevelUp -= UpdateHealthDisplay;
        }
    }
}
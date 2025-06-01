using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [Header("Health Bar Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Boss Images")]
    [SerializeField] private Image bossPhaseOneImage;
    [SerializeField] private Image bossPhaseTwoImage;
    
    [Header("Health Bar Colors")]
    [SerializeField] private Color healthColor = new Color(0.8f, 0.2f, 0.2f, 1f); // Dark red
    [SerializeField] private Color lowHealthColor = new Color(0.5f, 0f, 0f, 1f);  // Very dark red
    [SerializeField] private Color phaseChangeColor = Color.yellow;

    

    private float maxHealth;

    private void Awake()
    {
        if (bossPhaseTwoImage)
        {
            bossPhaseTwoImage.gameObject.SetActive(false);
        }
    }

    public void Initialize(float maxHealth, string bossName)
    {
        this.maxHealth = maxHealth;
        
        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        if (bossNameText)
        {
            bossNameText.text = bossName;
        }

        if (healthText)
        {
            healthText.text = $"{maxHealth}/{maxHealth}";
        }

        // Ensure phase one image is showing
        if (bossPhaseOneImage)
        {
            bossPhaseOneImage.gameObject.SetActive(true);
        }
    }

    public void UpdateHealth(float currentHealth)
    {
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        if (healthText)
        {
            healthText.text = $"{currentHealth:F0}/{maxHealth:F0}";
        }

        if (fillImage)
        {
            float healthPercentage = currentHealth / maxHealth;
            fillImage.color = Color.Lerp(lowHealthColor, healthColor, healthPercentage);
        }
    }

    public void OnPhaseTransition()
    {
        StartCoroutine(PhaseTransitionRoutine());
    }

    private System.Collections.IEnumerator PhaseTransitionRoutine()
    {
        // Flash health bar
        if (fillImage)
        {
            Color originalColor = fillImage.color;
            fillImage.color = phaseChangeColor;
            yield return new WaitForSeconds(0.2f);
            fillImage.color = originalColor;
        }

        // Switch boss images
        if (bossPhaseOneImage && bossPhaseTwoImage)
        {
            // Fade out phase one
            float fadeTime = 0.5f;
            float elapsed = 0;
            
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                float alpha = 1 - (elapsed / fadeTime);
                bossPhaseOneImage.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            bossPhaseOneImage.gameObject.SetActive(false);
            bossPhaseTwoImage.gameObject.SetActive(true);
            
            // Fade in phase two
            elapsed = 0;
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                float alpha = elapsed / fadeTime;
                bossPhaseTwoImage.color = new Color(1, 1, 1, alpha);
                yield return null;
            }
        }
    }
}
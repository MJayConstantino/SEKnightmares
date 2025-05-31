using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SkillUI
{
    [Header("UI Elements")]
    public Image backgroundImage;      // Square background
    public Image skillIcon;            // Skill icon
    public Image cooldownFill;         // Square cooldown overlay
    public TextMeshProUGUI timerText;  // Cooldown text
    
    [Header("Colors")]
    public Color availableColor = Color.white;
    public Color cooldownColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    public Color cooldownOverlayColor = new Color(0, 0, 0, 0.6f);
}
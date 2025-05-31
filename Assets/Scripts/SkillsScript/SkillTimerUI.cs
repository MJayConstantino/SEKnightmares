using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTimerUI : MonoBehaviour
{
    [Header("Skills")]
    public SkillUI dashSkill;
    public SkillUI rewindSkill;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TimeRewindController timeRewind;

    private void Start()
    {
        InitializeSkillUI(dashSkill);
        InitializeSkillUI(rewindSkill);
    }

    private void Update()
    {
        UpdateDashUI();
        UpdateRewindUI();
    }

    private void InitializeSkillUI(SkillUI skill)
    {
        if (skill.cooldownFill != null)
        {
            skill.cooldownFill.type = Image.Type.Filled;
            skill.cooldownFill.fillMethod = Image.FillMethod.Vertical;
            skill.cooldownFill.fillOrigin = (int)Image.Origin360.Top;
            skill.cooldownFill.color = skill.cooldownOverlayColor;
        }

        UpdateSkillAvailability(skill, true, 0f);
    }


    private void UpdateDashUI()
    {
        // Access dash cooldown from PlayerMovement
        bool canDash = playerMovement.canDash;
        float remainingCooldown = playerMovement.DashCooldownRemaining;
        float totalCooldown = playerMovement.dashCooldown;

        UpdateSkillUI(dashSkill, canDash, remainingCooldown, totalCooldown);
    }

    private void UpdateRewindUI()
    {
        bool canRewind = timeRewind.CanRewind;
        float remainingCooldown = timeRewind.RewindCooldownRemaining;
        float totalCooldown = timeRewind.rewindCooldown;

        UpdateSkillUI(rewindSkill, canRewind, remainingCooldown, totalCooldown);
    }

    private void UpdateSkillUI(SkillUI skill, bool isAvailable, float remainingCooldown, float totalCooldown)
    {
        // Update cooldown fill
        if (skill.cooldownFill != null)
        {
            skill.cooldownFill.fillAmount = isAvailable ? 0f : (remainingCooldown / totalCooldown);
        }

        // Update skill icon color
        UpdateSkillAvailability(skill, isAvailable, remainingCooldown);

        // Update timer text
        if (skill.timerText != null && !isAvailable)
        {
            skill.timerText.text = remainingCooldown.ToString("0.0");
            skill.timerText.color = skill.cooldownColor;
        }
        else if (skill.timerText != null)
        {
            skill.timerText.text = "";
        }
    }

    private void UpdateSkillAvailability(SkillUI skill, bool isAvailable, float remainingCooldown)
    {
        if (skill.skillIcon != null)
        {
            skill.skillIcon.color = isAvailable ? skill.availableColor : skill.cooldownColor;
        }
    }
}
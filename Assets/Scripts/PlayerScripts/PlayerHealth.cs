using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int maxExperience = 10;
    [SerializeField] private float damageFlashDuration = 0.2f;
    [SerializeField] private float deathDelay = 1f;

    [Header("Components")]
    [SerializeField] private Animator transition;
    [SerializeField] private AudioSource damageTakenSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource levelUpSound;
    [SerializeField] private VideoPlayer videoPlayer;

    private SpriteRenderer playerRenderer;
    private bool isInitialized;
    private Color originalColor;

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentHealth { get; private set; }
    public int CurrentExperience { get; private set; }

    private void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        if (!playerRenderer)
        {
            Debug.LogError("PlayerRenderer not found on " + gameObject.name);
            return;
        }

        originalColor = playerRenderer.color;
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        ValidateComponents();
        StartCoroutine(InitializeWhenReady());
    }

    private void ValidateComponents()
    {
        if (!damageTakenSound) Debug.LogWarning("DamageTaken sound not assigned on " + gameObject.name);
        if (!deathSound) Debug.LogWarning("Death sound not assigned on " + gameObject.name);
        if (!levelUpSound) Debug.LogWarning("LevelUp sound not assigned on " + gameObject.name);
        if (!transition) Debug.LogWarning("Transition animator not assigned on " + gameObject.name);
    }

    private IEnumerator InitializeWhenReady()
    {
        while (ExperienceManager.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
        isInitialized = true;
    }

    private void OnDisable()
    {
        if (isInitialized && ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
        }
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        
        if (damageTakenSound) damageTakenSound.Play();
        StartCoroutine(FlashRed());

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (transition) transition.SetBool("IsDead", true);
        if (deathSound) deathSound.Play();
        StartCoroutine(PlayerDeath());
    }

    private IEnumerator FlashRed()
    {
        if (!playerRenderer) yield break;

        playerRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFlashDuration);
        playerRenderer.color = originalColor;
    }

    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene("GameOver");
    }

    private void HandleExperienceChange(int experienceGained)
    {
        if (experienceGained <= 0) return;

        CurrentExperience += experienceGained;
        
        if (CurrentExperience >= maxExperience)
        {
            if (levelUpSound) levelUpSound.Play();
            LevelUp();
        }
    }

    private void LevelUp()
    {
        CurrentLevel++;
        maxHealth += 5;
        CurrentHealth = maxHealth;
        
        CurrentExperience = 0;
        maxExperience += 5;

        // Notify WeaponManager of level up
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.HandlePlayerLevelUp();
        }
    }

    public float GetHealthPercentage()
    {
        return (float)CurrentHealth / maxHealth;
    }

    public float GetExperiencePercentage()
    {
        return (float)CurrentExperience / maxExperience;
    }
}
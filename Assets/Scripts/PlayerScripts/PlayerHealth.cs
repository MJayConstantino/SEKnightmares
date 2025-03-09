using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerHealth : MonoBehaviour
{
    public Animator transition;

    [SerializeField] public int currentHealth, maxHealth, currentExperience, maxExperience, currentLevel;
    [SerializeField] AudioSource damageTaken, died, levelUp;
    public VideoPlayer videoPlayer;
    public SpriteRenderer playerRenderer;
    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }


    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        damageTaken.Play();
        currentHealth -= amount;
        StartCoroutine(FlashRed());
        if (currentHealth <=0)
        {
            //Animation.SetBool("IsDead", true);
            died.Play();
            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator FlashRed()
    {
        playerRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        playerRenderer.color = Color.white;
    }

    IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");
    }

    private void HandleExperienceChange(int newExperience)
    {
        currentExperience += newExperience;
        if(currentExperience >= maxExperience)
        {
            levelUp.Play();
            LevelUp();
        }
    }

    private void LevelUp()
    {
        maxHealth += 5;
        currentHealth = maxHealth;

        currentLevel++;

        currentExperience = 0;
        maxExperience += 5;
    }
}

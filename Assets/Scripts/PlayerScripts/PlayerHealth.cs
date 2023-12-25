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
        // Set the sprite color to red
        playerRenderer.color = Color.red;

        // Wait for a short duration
        yield return new WaitForSeconds(0.2f);

        // Reset the sprite color to its original color
        playerRenderer.color = Color.white;
    }

    IEnumerator PlayerDeath()
    {
        // Set the "IsDead" parameter to trigger the death animation
        //if (transition != null)
        //{
            //transition.SetBool("IsDead", true);
        //}

        // Wait for the animation to complete (you can adjust the time based on your animation)
        yield return new WaitForSeconds(1);

        // Load the GameOver scene
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
    void Update()
    {
        
    }
}

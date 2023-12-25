using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GEMathLogic : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 10f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] AudioSource ghostSpawn, ghostwalk, ghostHurt, ghostDie;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    SpriteRenderer characterRenderer; // Add a reference to the SpriteRenderer component
    [SerializeField] EnemyHealthBar healthBar;

    public Dissolve dissolve;

    public int damage = 1;

    int expAmount = 1;

    private bool canMove = false;

    private void Awake()
    {
        dissolve.Appear();
        rb = GetComponent<Rigidbody2D>();
        characterRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        StartCoroutine(EnableMoveAfterAppearance());
        FindPlayer();
    }

    private IEnumerator EnableMoveAfterAppearance()
    {
        dissolve.Appear(); // Start the appearance animation
        yield return new WaitForSeconds(1f);
        canMove = true;
        
         // Enable dash after appearance animation is complete
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    private void Update()
    {
        if (target && canMove)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            // Update the moveDirection
            moveDirection = direction;

            // Flip the sprite based on the movement direction
            if (moveDirection.x < 0)
            {
                characterRenderer.flipX = true; // Flip the sprite when moving left
            }
            else if (moveDirection.x > 0)
            {
                characterRenderer.flipX = false; // Reset scale to normal when moving right
            }
        }
        else
        {
            // Try to find the player if the target is null (e.g., if player is destroyed)
            FindPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (target && canMove)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        ghostHurt.Play();
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            ghostDie.Play();
            dissolve.Vanish();
            canMove = false;
            StartCoroutine(DestroyAfterDelay(0.75f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
    // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        ExperienceManager.Instance.AddExperience(expAmount);
    // Destroy the game object after waiting
        Destroy(gameObject);
    }

    

    // Example: Call this function when you want the enemy to deal damage to the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }
}
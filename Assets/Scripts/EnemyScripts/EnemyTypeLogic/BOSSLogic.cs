using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSLogic : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 100f;
    [SerializeField] float moveSpeed = 0f;
    [SerializeField] Transform[] bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float timeBetweenShots;



    [SerializeField] AudioSource bossFire, bossDeath, bossSpawn, secondPhase;

    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    EnemyHealthBar healthBar;

    public BossAttack bossAttack;

    public Collider2D bossCollider;

    public Canvas healthBarCanvas;

    int expAmount = 25;

    public Dissolve dissolve;

    public Animator animator;

    private bool isDead = false;

    private void Awake()
    {
        dissolve.Appear();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        FindPlayer();
        StartCoroutine(ShootRoutine());
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


        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            moveDirection = direction;
        }
        else
        {
            // Try to find the player if the target is null (e.g., if player is destroyed)
            FindPlayer();
        }
    }



    private void FixedUpdate()
    {
        if (target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }

    private void DealDamageToPlayer(int amount)
    {
        // Assuming you have a PlayerHealth script or a similar script for the player
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(amount);
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (health > 0 && !isDead)  // Add this condition to check if the boss is still alive
        {
            bossFire.Play();
            yield return new WaitForSeconds(timeBetweenShots);
            
            if (health > 0 && !isDead)  // Add this condition to check if the boss is still alive before shooting
            {
                bossAttack.Fire();
            }
        }
    }

    

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);

        // Check if health is below half for the 2nd phase
        if (health == maxHealth / 2)
        {
            bossAttack.SetPhaseTwo();
            secondPhase.Play();
            animator.SetBool("2ndPhase", true);
        }

        if (health <= 0)
        {
            isDead = true;
            bossCollider.enabled = false;
            healthBarCanvas.gameObject.SetActive(false);
            bossDeath.Play();
            dissolve.Vanish();
            StartCoroutine(DestroyAfterDelay(2f));
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int amount = 2;
            DealDamageToPlayer(amount);
            Debug.Log("-1");
        }
    }
}

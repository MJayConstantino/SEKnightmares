using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculusLogic : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 10f;
    [SerializeField] float moveSpeed = 0f;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float timeBetweenShots = 2f;
    [SerializeField] AudioSource robotspawn, robothurt, robotfire, robotdie;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    EnemyHealthBar healthBar;
    public SpawnBullet spawnBullet;
    SpriteRenderer characterRenderer;
    public Transform firePoint;
    int expAmount = 1;
    public Dissolve dissolve;

    private void Awake()
    {
        dissolve.Appear();
        rb = GetComponent<Rigidbody2D>();
        characterRenderer = GetComponent<SpriteRenderer>();
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
            Vector3 direction = (target.position - firePoint.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            moveDirection = direction;
            if (moveDirection.x < 0)
            {
                characterRenderer.flipX = false;
            }
            else if (moveDirection.x > 0)
            {
                characterRenderer.flipX = true;
            }
        }
        else
        {
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
        while (true)
        {
            //if (canShoot)
            //{
                robotfire.Play();
                yield return new WaitForSeconds(timeBetweenShots);
                spawnBullet.Fire();
            //}
        }
    }

    

    public void TakeDamage(float damageAmount)
    {
        robothurt.Play();
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            robotdie.Play();
            dissolve.Vanish();
            //canShoot = false;
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

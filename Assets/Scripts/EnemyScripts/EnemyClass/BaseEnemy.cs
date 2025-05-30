using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int experienceValue;
    [SerializeField] protected int damageAmount;

    [Header("Components")]
    protected Rigidbody2D rb;
    protected Transform target;
    protected Vector2 moveDirection;
    protected SpriteRenderer spriteRenderer;
    protected EnemyHealthBar healthBar;
    protected Dissolve dissolveEffect;

    [Header("Audio")]
    [SerializeField] protected AudioSource spawnSound;
    [SerializeField] protected AudioSource hurtSound;
    [SerializeField] protected AudioSource deathSound;

    [Header("Movement Settings")]
    [SerializeField] protected float stuckCheckInterval = 0.5f;
    [SerializeField] protected float stuckThreshold = 0.1f;
    [SerializeField] protected float unstuckForce = 5f;
    [SerializeField] protected float obstacleDetectionRange = 1f;
    [SerializeField] protected LayerMask obstacleLayer;

    private Vector2 lastPosition;
    private float stuckCheckTimer;
    public bool isStuck;
    private Vector2 unstuckDirection;

    protected bool canMove = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        dissolveEffect = GetComponent<Dissolve>();
    }

    protected virtual void Start()
    {
        Initialize();
        FindPlayer();
        StartCoroutine(EnableMovementAfterSpawn());
        lastPosition = transform.position;
    }

    protected virtual void Initialize()
    {
        health = maxHealth;
        healthBar?.UpdateHealthBar(health, maxHealth);
        dissolveEffect?.Appear();
        if (spawnSound) spawnSound.Play();
    }

    protected virtual void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player ? player.transform : null;
    }

    protected virtual void Update()
    {
        if (!target)
        {
            FindPlayer();
            return;
        }

        CheckIfStuck();
        UpdateMovementDirection();
        UpdateSpriteDirection();
    }

    protected virtual void CheckIfStuck()
    {
        stuckCheckTimer += Time.deltaTime;
        
        if (stuckCheckTimer >= stuckCheckInterval)
        {
            float distanceMoved = Vector2.Distance(lastPosition, transform.position);
            isStuck = distanceMoved < stuckThreshold && rb.velocity.magnitude < 0.1f;
            
            if (isStuck)
            {
                TryToUnstuck();
            }
            
            lastPosition = transform.position;
            stuckCheckTimer = 0f;
        }
    }

    protected virtual void TryToUnstuck()
    {
        // Generate random direction
        unstuckDirection = Random.insideUnitCircle.normalized;
        
        // Check if there's an obstacle in that direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, unstuckDirection, obstacleDetectionRange, obstacleLayer);
        
        if (hit.collider != null)
        {
            // If we hit an obstacle, try the opposite direction
            unstuckDirection = -unstuckDirection;
        }
        
        // Apply unstuck force
        rb.AddForce(unstuckDirection * unstuckForce, ForceMode2D.Impulse);
    }

    protected virtual void UpdateMovementDirection()
    {
        if (isStuck)
        {
            moveDirection = unstuckDirection;
            return;
        }

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        
        // Check for obstacles in the path to target
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, obstacleDetectionRange, obstacleLayer);
        
        if (hit.collider != null)
        {
            // Calculate avoidance direction
            Vector2 avoidanceDirection = Vector2.Perpendicular(directionToTarget);
            if (Random.value > 0.5f) avoidanceDirection = -avoidanceDirection;
            
            // Blend between target direction and avoidance
            moveDirection = Vector2.Lerp(directionToTarget, avoidanceDirection, 0.5f).normalized;
        }
        else
        {
            moveDirection = directionToTarget;
        }
    }

    protected virtual void Move()
    {
        if (!isStuck)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (target && canMove)
        {
            Move();
        }
    }

    protected virtual void UpdateSpriteDirection()
    {
        if (moveDirection.x != 0)
        {
            spriteRenderer.flipX = moveDirection.x < 0;
        }
    }

    public virtual void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar?.UpdateHealthBar(health, maxHealth);
        
        if (hurtSound) hurtSound.Play();

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (deathSound) deathSound.Play();
        dissolveEffect?.Vanish();
        canMove = false;
        StartCoroutine(DestroyAfterDelay(0.75f));
    }

    protected virtual IEnumerator EnableMovementAfterSpawn()
    {
        yield return new WaitForSeconds(0.75f);
        canMove = true;
    }

    protected virtual IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ExperienceManager.Instance.AddExperience(experienceValue);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
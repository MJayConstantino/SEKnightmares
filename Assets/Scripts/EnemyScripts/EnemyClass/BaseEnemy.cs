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

        UpdateMovementDirection();
        UpdateSpriteDirection();
    }

    protected virtual void FixedUpdate()
    {
        if (target && canMove)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    protected virtual void UpdateMovementDirection()
    {
        moveDirection = (target.position - transform.position).normalized;
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
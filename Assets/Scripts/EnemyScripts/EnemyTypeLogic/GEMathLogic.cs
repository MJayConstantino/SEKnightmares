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
    SpriteRenderer characterRenderer;
    [SerializeField] EnemyHealthBar healthBar;

    public Dissolve dissolve;
    public int damage = 1;
    int expAmount = 1;
    private bool canMove = false;

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
        StartCoroutine(EnableMoveAfterAppearance());
        FindPlayer();
    }

    private IEnumerator EnableMoveAfterAppearance()
    {
        dissolve.Appear();
        yield return new WaitForSeconds(1f);
        canMove = true;
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
            moveDirection = direction;
            if (moveDirection.x < 0)
            {
                characterRenderer.flipX = true;
            }
            else if (moveDirection.x > 0)
            {
                characterRenderer.flipX = false;
            }
        }
        else
        {
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
        yield return new WaitForSeconds(delay);
        ExperienceManager.Instance.AddExperience(expAmount);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }
}
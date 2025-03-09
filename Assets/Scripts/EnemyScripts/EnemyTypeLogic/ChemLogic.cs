using System.Collections;
using UnityEngine;

public class ChemLogic : MonoBehaviour
{
    [SerializeField] float health, maxHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashCooldown;
    [SerializeField] AudioSource witchspawn, witchidle, witchdash, withchdie;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    SpriteRenderer characterRenderer;
    EnemyHealthBar healthBar;
    public int damage = 5;
    private bool canDash = false;
    int expAmount = 1;
    public Dissolve dissolve;
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
        FindPlayer();
        StartCoroutine(EnableDashAfterAppearance());
    }

    private IEnumerator EnableDashAfterAppearance()
    {
        dissolve.Appear();
        yield return new WaitForSeconds(0.75f);
        canMove = true;
        yield return new WaitForSeconds(3f);
        canDash = true;
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        {
            target = player.transform;
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
                characterRenderer.flipX = false;
            }
            else if (moveDirection.x > 0)
            {
                characterRenderer.flipX = true;
            }
            if (canDash)
            {
                StartCoroutine(Dash());
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

    private IEnumerator Dash()
    {
        canDash = false;
        float originalSpeed = moveSpeed;
        moveSpeed = dashSpeed;
        witchdash.Play();
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = originalSpeed;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            withchdie.Play();
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }


}
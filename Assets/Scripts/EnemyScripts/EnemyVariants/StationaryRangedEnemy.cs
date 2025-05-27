using UnityEngine;

public class StationaryRangedEnemy : RangedEnemy
{
    [Header("Stationary Enemy Components")]
    [SerializeField] private SpawnBullet spawnBullet;

    protected override void Start()
    {
        // Initialize base stats
        experienceValue = 1;
        moveSpeed = 0f; // Stationary enemy doesn't move
        maxHealth = 10f;
        damageAmount = 2;
        timeBetweenShots = 2f;
        
        base.Start();
    }

    protected override void Fire()
    {
        if (spawnBullet && canMove)
        {
            spawnBullet.Fire();
        }
    }

    // Override Move to prevent any movement
    protected override void Move()
    {
        rb.velocity = Vector2.zero;
    }

    // Override collision behavior if needed
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"Player took {damageAmount} damage from stationary enemy");
            }
        }
    }
}
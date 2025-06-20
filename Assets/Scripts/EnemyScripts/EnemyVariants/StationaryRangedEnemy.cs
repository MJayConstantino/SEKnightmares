using UnityEngine;

public class StationaryRangedEnemy : RangedEnemy
{
    [Header("Stationary Enemy Components")]
    [SerializeField] private SpawnBullet spawnBullet;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateSpriteDirection()
    {
        if (target != null)
        {
            // Flip based on target direction since this enemy doesn't move
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            spriteRenderer.flipX = directionToTarget.x > 0;
        }
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
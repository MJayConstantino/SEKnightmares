using UnityEngine;

public class BigBossEnemy : BossEnemy
{
    [Header("Boss Attack Settings")]
    [SerializeField] private BossAttack bossAttack;
    [SerializeField] private Collider2D bossCollider;
    [SerializeField] private float attackInterval = 2f;
    private float nextAttackTime;

    protected override void Start()
    {
        // Set base stats
        experienceValue = 25;
        moveSpeed = 0f;
        maxHealth = 100f;
        damageAmount = 2;
        health = maxHealth;
        
        base.Start();
        
        if (!bossAttack || !bossCollider)
        {
            Debug.LogError($"Missing required components on {gameObject.name}");
            enabled = false;
            return;
        }

        // Initialize attack timing
        nextAttackTime = Time.time + attackInterval;
    }

    protected override void Update()
    {
        if (isDead) return;

        // Check if it's time to attack
        if (Time.time >= nextAttackTime)
        {
            Fire();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    protected override void Fire()
    {
        if (bossAttack && !isDead)
        {
            bossAttack.Fire();
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        
        if (bossCollider) bossCollider.enabled = false;
        if (bossAttack) Destroy(bossAttack);
        
        base.Die();
    }

    protected override void EnterSecondPhase()
    {
        if (isSecondPhase) return;
        
        base.EnterSecondPhase();
        if (bossAttack)
        {
            bossAttack.SetPhaseTwo();
            // Optionally increase attack frequency in phase 2
            attackInterval *= 0.75f;
        }
    }
}
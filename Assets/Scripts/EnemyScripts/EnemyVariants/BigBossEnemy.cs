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
        base.Start();

        if (!bossAttack || !bossCollider)
        {
            Debug.LogError($"Missing required components on {gameObject.name}");
            enabled = false;
            return;
        }

        nextAttackTime = Time.time + attackInterval;
        
        if (healthBar)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        if (healthBar)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }
    }

    protected override void EnterSecondPhase()
    {
        if (isSecondPhase) return;
        
        base.EnterSecondPhase();
        
        // Optionally heal a bit when entering second phase
        health = Mathf.Min(health + (maxHealth * 0.1f), maxHealth);
        healthBar.UpdateHealthBar(health, maxHealth);
        
        if (bossAttack)
        {
            bossAttack.SetPhaseTwo();
            attackInterval *= 0.75f;
        }
    }

    protected override void Update()
    {
        if (isDead) return;
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
}
using UnityEngine;

public abstract class BossEnemy : RangedEnemy
{
    [Header("Boss Specific")]
    [SerializeField] protected AudioSource phaseTransitionSound;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Canvas healthBarCanvas;
    
    protected bool isSecondPhase = false;
    protected bool isDead = false;

    protected override void Start()
    {
        base.Start();
        if (healthBarCanvas)
        {
            healthBarCanvas.worldCamera = Camera.main;
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        base.TakeDamage(damageAmount);

        if (!isSecondPhase && health <= maxHealth / 2)
        {
            EnterSecondPhase();
        }
    }

    protected virtual void EnterSecondPhase()
    {
        isSecondPhase = true;
        if (phaseTransitionSound) phaseTransitionSound.Play();
        if (animator) animator.SetBool("2ndPhase", true);
    }

    protected override void Die()
    {
        if (isDead) return;
        
        isDead = true;
        StopAllCoroutines();
        if (healthBarCanvas) healthBarCanvas.gameObject.SetActive(false);
        base.Die();
    }
}
using UnityEngine;

public abstract class BossEnemy : RangedEnemy
{
    [Header("Boss Specific")]
    [SerializeField] protected AudioSource phaseTransitionSound;
    [SerializeField] protected Animator animator;
    
    protected bool isSecondPhase = false;
    protected bool isDead = false;

    [Header("Boss UI")]
    [SerializeField] protected GameObject bossHealthBarPrefab;
    [SerializeField] protected string uiCanvasTag = "MainUICanvas"; // Add this to identify the correct canvas
    [SerializeField] protected string bossName = "Boss";
    protected BossHealthBar bossHealthBar;

    protected override void Start()
    {
        base.Start();
        
        if (healthBar)
        {
            healthBar.gameObject.SetActive(false);
        }

        // Find the correct canvas using tag
        Canvas uiCanvas = GameObject.FindGameObjectWithTag(uiCanvasTag).GetComponent<Canvas>();
        
        if (bossHealthBarPrefab)
        {
            if (uiCanvas == null)
            {
                Debug.LogError($"No Canvas found with tag '{uiCanvasTag}' for {gameObject.name}");
                return;
            }

            GameObject healthBarObj = Instantiate(bossHealthBarPrefab, uiCanvas.transform);
            bossHealthBar = healthBarObj.GetComponent<BossHealthBar>();
            if (bossHealthBar)
            {
                bossHealthBar.Initialize(maxHealth, bossName);
            }
            else
            {
                Debug.LogError("BossHealthBar component not found on instantiated prefab");
            }
        }
        else
        {
            Debug.LogError("Boss health bar prefab not assigned to " + gameObject.name);
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        base.TakeDamage(damageAmount);

        if (bossHealthBar)
        {
            bossHealthBar.UpdateHealth(health);
        }

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
        if (bossHealthBar) bossHealthBar.OnPhaseTransition();
    }

    protected override void Die()
    {
        if (isDead) return;
        
        isDead = true;
        StopAllCoroutines();
        
        // Destroy the boss health bar
        if (bossHealthBar)
        {
            Destroy(bossHealthBar.gameObject);
        }
        
        base.Die();
    }
}
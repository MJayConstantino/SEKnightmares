using System.Collections;
using UnityEngine;

public class DashingEnemy : BaseEnemy
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private AudioSource dashSound;
    
    [Header("Trail Settings")]
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private float trailTime = 0.5f;
    
    private bool canDash = false;
    private bool isDashing = false;

    protected override void Start()
{
    base.Start();
    experienceValue = 1;
    moveSpeed = 2f;
    maxHealth = 15f;
    damageAmount = 5;
    
    // Flip the initial scale to correct the sprite orientation
    transform.localScale = new Vector3(
        -Mathf.Abs(transform.localScale.x),
        transform.localScale.y,
        transform.localScale.z
    );
    
    InitializeComponents();
    StartCoroutine(EnableDashAfterDelay());
}
    private void InitializeComponents()
    {
        // Add TrailRenderer if it doesn't exist
        if (!dashTrail)
        {
            dashTrail = gameObject.AddComponent<TrailRenderer>();
            SetupTrailRenderer();
        }
        
        // Initially disable the trail
        dashTrail.enabled = false;
    }

    private void SetupTrailRenderer()
    {
        dashTrail.time = trailTime;
        dashTrail.startWidth = 0.5f;
        dashTrail.endWidth = 0f;
        dashTrail.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    protected override void Update()
    {
    base.Update();
    
    if (target && !isDashing)
    {
        // Calculate direction to target
        Vector2 direction = (target.position - transform.position).normalized;
        
        // Update sprite orientation (inverted from before)
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

        if (canMove && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float originalSpeed = moveSpeed;
        moveSpeed = dashSpeed;

        dashTrail.enabled = true;
        if (dashSound) dashSound.Play();

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalSpeed;
        dashTrail.enabled = false;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator EnableDashAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        canDash = true;
    }
}
using System.Collections;
using UnityEngine;

public class DashingEnemy : BaseEnemy
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 6f;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private TrailRenderer dashTrail;
    
    private bool canDash = false;
    private bool isDashing = false;

     protected override void Start()
    {
        base.Start();
        experienceValue = 1;
        moveSpeed = 2f;
        maxHealth = 15f;
        damageAmount = 5;
        
        StartCoroutine(EnableDashAfterDelay());
    }

    protected override void UpdateSpriteDirection()
    {
        // Override the base sprite direction logic
        if (moveDirection.x != 0)
        {
            // working last time- commented out for testing
            // transform.localScale = new Vector3(
            //     Mathf.Abs(transform.localScale.x) * (moveDirection.x > 0 ? 1 : -1),
            //     transform.localScale.y,
            //     transform.localScale.z
            // );
            spriteRenderer.flipX = moveDirection.x > 0;
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if (canMove && canDash && !isDashing && target)
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
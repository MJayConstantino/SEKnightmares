using System.Collections;
using UnityEngine;

public class DashingEnemy : BaseEnemy
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private AudioSource dashSound;
    private bool canDash = false;

    protected override void Start()
    {
        experienceValue = 1;
        moveSpeed = 2f;
        maxHealth = 15f;
        damageAmount = 5;
        base.Start();
        StartCoroutine(EnableDashAfterDelay());
    }

    private IEnumerator EnableDashAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        canDash = true;
    }

    protected override void Update()
    {
        base.Update();
        if (canMove && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        float originalSpeed = moveSpeed;
        moveSpeed = dashSpeed;
        if (dashSound) dashSound.Play();
        
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = originalSpeed;
        
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
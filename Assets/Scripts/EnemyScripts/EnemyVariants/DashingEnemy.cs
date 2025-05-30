using System.Collections;
using UnityEngine;

public class DashingEnemy : BaseEnemy
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 6f;
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
        
        InitializeComponents();
        StartCoroutine(EnableDashAfterDelay());
    }

    protected override void UpdateSpriteDirection()
    {
        // Override the base sprite direction logic
        if (moveDirection.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * (moveDirection.x > 0 ? 1 : -1),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    private void SetupTrailRenderer()
    {
        dashTrail.time = trailTime;
        dashTrail.startWidth = 0.5f;
        dashTrail.endWidth = 0f;
        dashTrail.sortingOrder = spriteRenderer.sortingOrder - 1;
        
        // Set trail color
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(Color.white, 0.0f), 
                new GradientColorKey(Color.white, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(0.5f, 0.0f), 
                new GradientAlphaKey(0f, 1.0f) 
            }
        );
        dashTrail.colorGradient = gradient;
        
        // Set trail material
        dashTrail.material = new Material(Shader.Find("Sprites/Default"));
    }

    protected override void Update()
    {
        base.Update();
        
        if (canMove && canDash && !isDashing && target)
        {
            StartCoroutine(Dash());
        }
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
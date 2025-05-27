using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int speedPlayer = 5;
    [SerializeField] private AudioSource dashingSound;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference dash;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCooldown = 3f;

    private Vector2 moveDirection;
    private bool isDashing;
    private bool canDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDashing)
        {
            HandleDashInput();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        moveDirection = movement.action.ReadValue<Vector2>();
        bool isMoving = moveDirection.magnitude > 0.1f;
        
        animator.SetBool("IsMoving", isMoving);
        rb.velocity = isMoving ? moveDirection.normalized * speedPlayer : Vector2.zero;
    }

    private void HandleDashInput()
    {
        if (dash.action.triggered && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        
        if (dashingSound) dashingSound.Play();
        rb.velocity = moveDirection.normalized * dashSpeed;
        
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
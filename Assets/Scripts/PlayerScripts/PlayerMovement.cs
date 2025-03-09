using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int speedPlayer = 5;
    public Rigidbody2D rb;
    public WeaponCalculator weaponCalculator;
    [SerializeField] AudioSource dashing, fire;
    public Animator animator;
    Vector2 moveDirection, pointerInputRanged;
    [SerializeField]
    private InputActionReference movement, dash, attack, pointerPosition;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 3f;
    bool isDashing;
    bool canDash = true;
    private WeaponParentRanged weaponParentRanged;

    private void Start()
    {
        canDash = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponParentRanged = GetComponentInChildren<WeaponParentRanged>();
        animator= GetComponent<Animator>();
    }

    private void OnMovement(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        pointerInputRanged = GetPointerInput();
        weaponParentRanged.PointerPosition = pointerInputRanged;
        if (attack.action.triggered)
        {
            fire.Play();
            weaponCalculator.Fire();
        }

        if (dash.action.triggered && canDash)
        {
            dashing.Play();
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Vector2 lookDirection = pointerInputRanged - (Vector2)transform.position;

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            animator.SetBool("IsMoving", true);
            rb.velocity = moveDirection.normalized * speedPlayer;
        }
        else
        {
            animator.SetBool("IsMoving", false);
            rb.velocity = Vector2.zero;
        }
    }
    
    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

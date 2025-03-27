using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParentRanged : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer characterRenderer, weaponRenderer;
    // Removed the firepoint reference since all weapons share the same one
    // public Transform firepoint;
    
    [Header("Settings")]
    public float rotationSpeed = 1000f;
    public float weaponFlipThreshold = 0.1f;
    public bool useSmoothing = true;
    public float aimSmoothing = 5f;
    
    [Header("Weapon Bobbing")]
    public bool enableWeaponBobbing = true;
    public float bobbingAmount = 0.05f;
    public float bobbingSpeed = 1f;
    
    // Properties
    public Vector2 PointerPosition { get; set; }
    private Vector2 _targetDirection;
    private Vector2 _currentDirection;
    private float _bobbingTimer = 0f;
    private Vector3 _originalLocalPosition;
    
    private void Start()
    {
        _originalLocalPosition = transform.localPosition;
        _currentDirection = transform.right;
    }

    private void Update()
    {
        HandleAiming();
        HandleWeaponFlipping();
        HandleLayerSorting();
        
        if (enableWeaponBobbing)
        {
            ApplyWeaponBobbing();
        }
    }
    
    private void HandleAiming()
    {
        _targetDirection = (PointerPosition - (Vector2)transform.position).normalized;
        
        if (useSmoothing)
        {
            _currentDirection = Vector2.Lerp(_currentDirection, _targetDirection, Time.deltaTime * aimSmoothing);
            transform.right = _currentDirection;
        }
        else
        {
            transform.right = _targetDirection;
        }
    }
    
    private void HandleWeaponFlipping()
    {
        Vector2 scale = transform.localScale;
        
        if (_currentDirection.x < -weaponFlipThreshold)
        {
            scale.y = -1;
            // Removed RotateFirepoint(90f) because the firepoint is shared and handled externally
        }
        else if (_currentDirection.x > weaponFlipThreshold)
        {
            scale.y = 1;
            // Removed RotateFirepoint(-90f)
        }
        
        transform.localScale = scale;
    }
    
    private void HandleLayerSorting()
    {
        float angle = transform.eulerAngles.z;
        
        if (angle > 0 && angle < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }
    
    private void ApplyWeaponBobbing()
    {
        _bobbingTimer += Time.deltaTime * bobbingSpeed;
        float bobbingOffset = Mathf.Sin(_bobbingTimer) * bobbingAmount;
        
        transform.localPosition = _originalLocalPosition + new Vector3(0f, bobbingOffset, 0f);
    }

    // Removed RotateFirepoint method because a shared firepoint will be managed externally.
}

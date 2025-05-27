using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionReference attack;
    [SerializeField] private InputActionReference pointerPosition;
    
    private WeaponParentRanged weaponParent;
    private BaseWeapon weapon;
    private bool isInitialized;

    private void Awake()
    {
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        // Look for WeaponParentRanged in the parent GameObject
        weaponParent = GetComponentInParent<WeaponParentRanged>();
        if (!weaponParent)
        {
            Debug.LogError($"WeaponParentRanged not found in parent of {gameObject.name}");
            enabled = false;
            return;
        }

        // Look for BaseWeapon in siblings
        weapon = weaponParent.GetComponentInChildren<BaseWeapon>();
        if (!weapon)
        {
            Debug.LogError($"BaseWeapon not found in children of {weaponParent.gameObject.name}");
            enabled = false;
            return;
        }

        if (!attack || !pointerPosition)
        {
            Debug.LogError($"Input actions not assigned on {gameObject.name}");
            enabled = false;
            return;
        }

        isInitialized = true;
    }

    private void OnEnable()
    {
        if (!isInitialized) return;
        
        attack.action.Enable();
        pointerPosition.action.Enable();
    }

    private void OnDisable()
    {
        if (!isInitialized) return;
        
        attack.action.Disable();
        pointerPosition.action.Disable();
    }

    private void Update()
    {
        if (!isInitialized) return;
        
        UpdateWeaponAim();
        HandleWeaponFiring();
    }

    private void UpdateWeaponAim()
    {
        if (weaponParent != null)
        {
            weaponParent.PointerPosition = GetPointerInput();
        }
    }

    private void HandleWeaponFiring()
    {
        if (attack.action.IsPressed() && weapon != null && weapon.CanFire())
        {
            weapon.Fire();
        }
    }

    private Vector2 GetPointerInput()
    {
        Vector2 screenPosition = pointerPosition.action.ReadValue<Vector2>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
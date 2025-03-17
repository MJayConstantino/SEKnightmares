using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Weapon manager to handle weapon switching
public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public List<WeaponBase> availableWeapons = new List<WeaponBase>();
    
    [Header("Settings")]
    public KeyCode nextWeaponKey = KeyCode.E;
    public KeyCode previousWeaponKey = KeyCode.Q;
    public KeyCode[] weaponHotkeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    
    private int _currentWeaponIndex = 0;
    private WeaponBase _currentWeapon;
    
    void Start()
    {
        // Initialize with the first weapon
        if (availableWeapons.Count > 0)
        {
            _currentWeaponIndex = 0;
            EquipWeapon(_currentWeaponIndex);
        }
    }
    
    void Update()
    {
        // Handle weapon switching
        if (Input.GetKeyDown(nextWeaponKey))
        {
            SwitchToNextWeapon();
        }
        else if (Input.GetKeyDown(previousWeaponKey))
        {
            SwitchToPreviousWeapon();
        }
        
        // Handle hotkeys
        for (int i = 0; i < weaponHotkeys.Length && i < availableWeapons.Count; i++)
        {
            if (Input.GetKeyDown(weaponHotkeys[i]))
            {
                EquipWeapon(i);
                break;
            }
        }
        
        // Handle firing
        if (Input.GetButton("Fire1") && _currentWeapon != null)
        {
            _currentWeapon.Fire();
        }
        
        // Handle reloading
        if (Input.GetKeyDown(KeyCode.R) && _currentWeapon != null)
        {
            StartCoroutine(_currentWeapon.Reload());
        }
    }
    
    public void SwitchToNextWeapon()
    {
        if (availableWeapons.Count <= 1) return;
        
        _currentWeaponIndex = (_currentWeaponIndex + 1) % availableWeapons.Count;
        EquipWeapon(_currentWeaponIndex);
    }
    
    public void SwitchToPreviousWeapon()
    {
        if (availableWeapons.Count <= 1) return;
        
        _currentWeaponIndex = (_currentWeaponIndex - 1 + availableWeapons.Count) % availableWeapons.Count;
        EquipWeapon(_currentWeaponIndex);
    }
    
    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= availableWeapons.Count) return;
        
        // Deactivate all weapons
        foreach (var weapon in availableWeapons)
        {
            weapon.gameObject.SetActive(false);
        }
        
        // Activate the selected weapon
        _currentWeapon = availableWeapons[index];
        _currentWeapon.gameObject.SetActive(true);
        
        // Optional: Play weapon switch animation/sound
        // PlayWeaponSwitchEffect();
    }
}
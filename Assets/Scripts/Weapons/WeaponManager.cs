using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Weapon manager to handle weapon switching
public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public List<WeaponBase> availableWeapons = new List<WeaponBase>();
    
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
    
    // New method: switch to a random weapon from the list
    public void SwitchToRandomWeapon()
    {
        if (availableWeapons.Count == 0) return;
        int randomIndex = Random.Range(0, availableWeapons.Count);
        EquipWeapon(randomIndex);
    }
    
    public void SwitchToWeaponIndex(int index)
    {
        if (index < 0 || index >= availableWeapons.Count) return;
        EquipWeapon(index);
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
    
    public WeaponBase GetCurrentWeapon()
    {
        return _currentWeapon;
    }
    
    public int GetCurrentWeaponIndex()
    {
        return _currentWeaponIndex;
    }
}

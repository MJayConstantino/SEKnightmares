using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager instance;
    public static WeaponManager Instance 
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WeaponManager>();
            }
            return instance;
        }
    }

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private WeaponCalculator calculator;

    private void Start()
    {
        StartCoroutine(FindCalculatorWeapon());
    }

    private IEnumerator FindCalculatorWeapon()
    {
        yield return null; // Wait one frame for initialization

        // Find WeaponParentRanged in siblings
        WeaponParentRanged weaponParent = transform.parent.GetComponentInChildren<WeaponParentRanged>();
        if (weaponParent != null)
        {
            calculator = weaponParent.GetComponentInChildren<WeaponCalculator>();
            if (calculator != null && showDebug)
            {
                Debug.Log($"Found Calculator weapon, starting level: {calculator.WeaponLevel}");
            }
        }
        else
        {
            Debug.LogError("WeaponParentRanged not found in siblings");
        }
    }

    public void HandlePlayerLevelUp()
    {
        if (calculator != null)
        {
            calculator.LevelUp();
            
            if (showDebug)
            {
                Debug.Log($"Weapon leveled up to {calculator.WeaponLevel}, " +
                         $"Damage: {calculator.CurrentDamage:F1}, " +
                         $"Fire Rate: {calculator.CurrentFireRate:F1}");
            }
        }
    }
}
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }
    [SerializeField] private BaseWeapon[] weapons;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ValidateWeapons();
    }

    private void ValidateWeapons()
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogWarning("No weapons assigned to WeaponManager");
            weapons = GetComponentsInChildren<BaseWeapon>();
        }
    }

    public void HandlePlayerLevelUp()
    {
        foreach (BaseWeapon weapon in weapons)
        {
            if (weapon != null)
            {
                weapon.LevelUp();
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
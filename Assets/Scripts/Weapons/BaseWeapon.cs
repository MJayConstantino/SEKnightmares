using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Base Weapon Settings")]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float baseFireForce = 20f;
    [SerializeField] protected float baseDamage = 1f;
    [SerializeField] protected float baseFireRate = 20f;
    [SerializeField] protected AudioSource shootSound;
    [SerializeField] protected float bulletLifetime = 2f;

    protected int weaponLevel = 1;
    protected float currentDamage;
    protected float currentFireRate;
    protected float currentFireForce;
    protected float nextFireTime;

    protected virtual void Start()
    {
        InitializeWeapon();
    }

    protected virtual void InitializeWeapon()
    {
        currentDamage = baseDamage;
        currentFireRate = baseFireRate;
        currentFireForce = baseFireForce;
    }

    public virtual void LevelUp()
    {
        weaponLevel++;
        ApplyUpgrade();
    }

    protected virtual void ApplyUpgrade()
    {
        currentDamage *= 1.2f;
        currentFireRate *= 1.1f;
        currentFireForce *= 1.05f;
    }

    public virtual bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    public abstract void Fire();

    protected virtual void UpdateNextFireTime()
    {
        nextFireTime = Time.time + (1f / currentFireRate);
    }

    protected virtual void InitializeBullet(GameObject bullet, Quaternion rotation)
    {
        if (bullet.TryGetComponent<Bullet>(out Bullet bulletComponent))
        {
            bulletComponent.Initialize(currentDamage);
        }
        
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero; // Reset velocity
            rb.AddForce(rotation * Vector2.right * currentFireForce, ForceMode2D.Impulse);
        }

        Destroy(bullet, bulletLifetime); // Destroy after lifetime
    }
}
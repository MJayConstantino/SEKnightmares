using UnityEngine;

public class WeaponCalculator : BaseWeapon
{
    [Header("Calculator Specific")]
    [SerializeField] private float spreadAngle = 15f;
    [SerializeField] private int projectilesPerShot = 1;

    protected override void Start()
    {
        base.Start();
        if (!firePoint)
        {
            firePoint = transform.Find("FirePoint");
            if (!firePoint)
            {
                Debug.LogError("FirePoint not found on " + gameObject.name);
                enabled = false;
            }
        }
    }

    public override void Fire()
    {
        if (!CanFire()) return;

        float startAngle = -spreadAngle / 2f;
        float angleStep = projectilesPerShot > 1 ? spreadAngle / (projectilesPerShot - 1) : 0;

        // Get the base rotation from the weapon parent
        Quaternion baseRotation = transform.parent.rotation;

        for (int i = 0; i < projectilesPerShot; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Quaternion bulletRotation = baseRotation * Quaternion.Euler(0, 0, currentAngle);
            
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            InitializeBullet(bullet, bulletRotation);
        }

        if (shootSound) shootSound.Play();
        UpdateNextFireTime();
    }

    protected override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        
        // Every few levels, add more projectiles
        if (weaponLevel % 2 == 0)
        {
            projectilesPerShot++;
            spreadAngle += 5f;
        }
    }
}
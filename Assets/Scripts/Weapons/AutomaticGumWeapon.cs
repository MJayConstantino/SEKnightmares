using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Automatic rapid-fire gum weapon
public class AutomaticGumWeapon : WeaponBase
{
    [Header("Automatic Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 18f;
    public float bulletLifetime = 1f;
    public float bulletSpread = 5f;
    
    protected override void Awake()
    {
        base.Awake();
        fireRate = 10f; // High fire rate for automatic weapon
    }
    
    protected override void PerformFire()
    {
        // Create bullet with increased spread
        float spreadAngle = Random.Range(-bulletSpread, bulletSpread);
        Quaternion bulletRotation = Quaternion.Euler(0, 0, firePoint.eulerAngles.z + spreadAngle);
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        
        if (bullet != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = 1;
                bulletScript.lifetime = bulletLifetime;
                bulletScript.sourceWeapon = this;
            }
            
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = bullet.transform.right * bulletSpeed;
            }
        }
    }
}

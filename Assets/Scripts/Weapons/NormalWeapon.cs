using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Standard single-shot gum weapon
public class GumWeapon : WeaponBase
{
    [Header("Gum Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 15f;
    public float bulletLifetime = 2f;
    public float bulletSpread = 0f;
    
    protected override void PerformFire()
    {
        // Create bullet with slight spread
        float spreadAngle = Random.Range(-bulletSpread, bulletSpread);
        Quaternion bulletRotation = Quaternion.Euler(0, 0, firePoint.eulerAngles.z + spreadAngle);
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        
        // Set bullet properties
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


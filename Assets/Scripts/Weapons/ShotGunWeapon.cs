using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shotgun variant
public class ShotgunGumWeapon : WeaponBase
{
    [Header("Shotgun Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 12f;
    public float bulletLifetime = 1.5f;
    public int pelletsPerShot = 6;
    public float spreadAngle = 15f;
    
    protected override void PerformFire()
    {
        // Fire multiple pellets in a spread pattern
        for (int i = 0; i < pelletsPerShot; i++)
        {
            float angle = Random.Range(-spreadAngle, spreadAngle);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, firePoint.eulerAngles.z + angle);
            
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
}
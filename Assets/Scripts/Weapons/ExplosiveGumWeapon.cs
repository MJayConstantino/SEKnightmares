using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Explosive gum weapon
public class ExplosiveGumWeapon : WeaponBase
{
    [Header("Explosive Settings")]
    public GameObject explosiveBulletPrefab;
    public float bulletSpeed = 8f;
    public float bulletLifetime = 3f;
    
    protected override void Awake()
    {
        base.Awake();
        fireRate = 0.5f; // Slow fire rate for explosive weapon
    }
    
    protected override void PerformFire()
    {
        GameObject bullet = Instantiate(explosiveBulletPrefab, firePoint.position, firePoint.rotation);
        
        if (bullet != null)
        {
            ExplosiveBullet explosiveBullet = bullet.GetComponent<ExplosiveBullet>();
            if (explosiveBullet == null)
            {
                explosiveBullet = bullet.AddComponent<ExplosiveBullet>();
            }
            
            explosiveBullet.damage = 1;
            explosiveBullet.lifetime = bulletLifetime;
            explosiveBullet.sourceWeapon = this;
            
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = bullet.transform.right * bulletSpeed;
            }
        }
    }
}

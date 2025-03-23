using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bouncing gum weapon
public class BouncingGumWeapon : WeaponBase
{
    [Header("Bouncing Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;
    public int maxBounces = 3;
    
    protected override void PerformFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        if (bullet != null)
        {
            BouncingBullet bouncingBullet = bullet.GetComponent<BouncingBullet>();
            if (bouncingBullet == null)
            {
                bouncingBullet = bullet.AddComponent<BouncingBullet>();
            }
            
            bouncingBullet.damage = 1;
            bouncingBullet.lifetime = bulletLifetime;
            bouncingBullet.maxBounces = maxBounces;
            bouncingBullet.sourceWeapon = this;
            
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = bullet.transform.right * bulletSpeed;
                bulletRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                bulletRb.sharedMaterial = CreateBouncyMaterial();
            }
        }
    }
    
    private PhysicsMaterial2D CreateBouncyMaterial()
    {
        PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D("BouncyGum");
        bouncyMaterial.bounciness = 0.8f;
        bouncyMaterial.friction = 0.2f;
        return bouncyMaterial;
    }
}

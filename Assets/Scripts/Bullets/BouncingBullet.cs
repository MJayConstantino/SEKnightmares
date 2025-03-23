using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Specialized bouncing bullet class
public class BouncingBullet : Bullet
{
    public int maxBounces = 3;
    private int _currentBounces = 0;
    
    protected override void ProcessHit(GameObject hitObject)
    {
        // Only destroy the bullet if it's an enemy or we've reached max bounces
        IDamageable damageable = hitObject.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            // If we hit an enemy, apply damage and destroy the bullet
            ApplyDamage(hitObject);
            CreateImpactEffect();
            Destroy(gameObject);
        }
        else
        {
            // If we hit a wall or other non-enemy object
            _currentBounces++;
            CreateBounceEffect();
            
            if (_currentBounces >= maxBounces)
            {
                CreateImpactEffect();
                Destroy(gameObject);
            }
        }
    }
    
    private void CreateImpactEffect()
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
    }
    
    private void CreateBounceEffect()
    {
        // Optional: Create a smaller effect for bounces
        if (impactEffectPrefab != null)
        {
            GameObject bounceEffect = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            bounceEffect.transform.localScale *= 0.5f;
        }
    }
}

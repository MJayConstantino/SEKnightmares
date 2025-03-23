using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Explosive bullet class
public class ExplosiveBullet : Bullet
{
    [Header("Explosion Properties")]
    public float explosionRadius = 2f;
    public int explosionDamage = 2;
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound;
    public float explosionForce = 5f;
    
    protected override void ProcessHit(GameObject hitObject)
    {
        // Create explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }
        
        // Apply damage to all enemies in radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            // Apply damage
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(explosionDamage);
            }
            
            // Apply force
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }
        
        // Destroy bullet
        Destroy(gameObject);
    }
}

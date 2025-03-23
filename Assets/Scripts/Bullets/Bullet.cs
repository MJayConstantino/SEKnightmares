using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Enhanced bullet base class
public class Bullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    public int damage = 1;
    public float lifetime = 2f;
    public WeaponBase sourceWeapon;
    
    [Header("Effects")]
    public GameObject impactEffectPrefab;
    public AudioClip impactSound;
    
    private float _spawnTime;
    
    private void Start()
    {
        _spawnTime = Time.time;
    }
    
    private void Update()
    {
        // Destroy bullet after lifetime
        if (Time.time > _spawnTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessHit(collision.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessHit(collision.gameObject);
    }
    
    protected virtual void ProcessHit(GameObject hitObject)
    {
        // Create impact effect
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // Play impact sound
        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
        }
        
        // Process damage for different enemy types
        ApplyDamage(hitObject);
        
        // Destroy bullet
        Destroy(gameObject);
    }
    
    protected virtual void ApplyDamage(GameObject hitObject)
    {
        // Check for all enemy types using a more efficient approach
        IDamageable damageable = hitObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            return;
        }
        
        // Legacy damage application (for backward compatibility)
        if (hitObject.TryGetComponent<GEMathLogic>(out GEMathLogic geMathEnemy))
        {
            geMathEnemy.TakeDamage(damage);
        }
        else if (hitObject.TryGetComponent<DiscreteMathLogic>(out DiscreteMathLogic discreteMathEnemy))
        {
            discreteMathEnemy.TakeDamage(damage);
        }
        else if (hitObject.TryGetComponent<ChemLogic>(out ChemLogic chemEnemy))
        {
            chemEnemy.TakeDamage(damage);
        }
        else if (hitObject.TryGetComponent<CalculusLogic>(out CalculusLogic calculusEnemy))
        {
            calculusEnemy.TakeDamage(damage);
        }
        else if (hitObject.TryGetComponent<BOSSLogic>(out BOSSLogic bossEnemy))
        {
            bossEnemy.TakeDamage(damage);
        }
    }
}
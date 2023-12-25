using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 1;
    public SpriteRenderer spriteRenderer;

    public Dissolve dissolve;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dissolve.Vanish();
    
        if (collision.gameObject.CompareTag("Player"))
        {
            // If collided with the player, deal damage
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }

        
        StartCoroutine(DestroyAfterDelay(0.01f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
    // Wait for the specified delay
        yield return new WaitForSeconds(delay);
    // Destroy the game object after waiting
        Destroy(gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Bullet exits collision with an enemy, revert to the original sorting order
            spriteRenderer.sortingOrder = 0;
        }
    }
}
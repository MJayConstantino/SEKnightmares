using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    
    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
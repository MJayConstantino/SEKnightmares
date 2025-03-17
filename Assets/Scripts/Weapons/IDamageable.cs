using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Common interface for all damageable entities
public interface IDamageable
{
    void TakeDamage(int amount);
}

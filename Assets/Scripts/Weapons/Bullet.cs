using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if(collision.gameObject.TryGetComponent<GEMathLogic>(out GEMathLogic geMathEnemy))
        {
            geMathEnemy.TakeDamage(1);
        }

        if(collision.gameObject.TryGetComponent<DiscreteMathLogic>(out DiscreteMathLogic discreteMathEnemy))
        {
            discreteMathEnemy.TakeDamage(1);
        }

        if(collision.gameObject.TryGetComponent<ChemLogic>(out ChemLogic chemEnemy))
        {
            chemEnemy.TakeDamage(1);
        }

        if(collision.gameObject.TryGetComponent<CalculusLogic>(out CalculusLogic calculusEnemy))
        {
            calculusEnemy.TakeDamage(1);
        }

        if(collision.gameObject.TryGetComponent<BOSSLogic>(out BOSSLogic BOSSEnemy))
        {
            BOSSEnemy.TakeDamage(1);
        }
    }
}
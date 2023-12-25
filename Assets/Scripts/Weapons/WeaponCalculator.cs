using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCalculator : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 1f;


    public void Fire()
    {
        Debug.Log("Fire!"); // Debugging statement

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bullet != null)
        {
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            }
        }
    }
}
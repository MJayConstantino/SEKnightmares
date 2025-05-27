using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [Header("Ranged Attack Settings")]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float timeBetweenShots = 2f;
    [SerializeField] protected AudioSource attackSound;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ShootRoutine());
    }

    protected override void Update()
    {
        base.Update();
        if (target)
        {
            UpdateFirePointRotation();
        }
    }

    protected virtual void UpdateFirePointRotation()
    {
        Vector2 direction = (target.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    protected virtual IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (canMove && target)
            {
                if (attackSound) attackSound.Play();
                Fire();
                yield return new WaitForSeconds(timeBetweenShots);
            }
            yield return null;
        }
    }

    protected virtual void Fire()
    {
        if (bulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D bulletRb))
            {
                bulletRb.AddForce(firePoint.up * 10f, ForceMode2D.Impulse);
            }
        }
    }
}
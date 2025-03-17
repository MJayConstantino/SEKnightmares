using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base weapon class for all weapon types
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponName;
    public Sprite weaponSprite;
    public float fireRate = 1f;
    public int magazineSize = 10;
    public float reloadTime = 1.5f;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    
    [Header("Effects")]
    public GameObject muzzleFlashPrefab;
    public float muzzleFlashDuration = 0.1f;
    public bool useScreenShake = true;
    public float screenShakeAmount = 0.1f;
    public float screenShakeDuration = 0.1f;
    
    // State
    protected Transform firePoint;
    protected AudioSource audioSource;
    protected int currentAmmo;
    protected float nextFireTime;
    protected bool isReloading = false;
    
    protected virtual void Awake()
    {
        firePoint = transform.Find("FirePoint");
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        currentAmmo = magazineSize;
    }
    
    public virtual void Fire()
    {
        if (Time.time < nextFireTime || isReloading || currentAmmo <= 0)
            return;
        
        // Set the next fire time based on fire rate
        nextFireTime = Time.time + (1f / fireRate);
        
        // Decrease ammo
        currentAmmo--;
        
        // Play fire sound
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
        
        // Show muzzle flash
        if (muzzleFlashPrefab != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }
        
        // Apply screen shake
        if (useScreenShake)
        {
            // Notify camera to shake - implementation depends on your camera system
            CameraShaker.Instance?.Shake(screenShakeAmount, screenShakeDuration);
        }
        
        // Perform actual firing (implemented by derived classes)
        PerformFire();
        
        // Auto reload if empty
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }
    
    protected abstract void PerformFire();
    
    public virtual IEnumerator Reload()
    {
        if (isReloading || currentAmmo == magazineSize)
            yield break;
        
        isReloading = true;
        
        // Play reload sound
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
        
        // Wait for reload time
        yield return new WaitForSeconds(reloadTime);
        
        // Refill ammo
        currentAmmo = magazineSize;
        isReloading = false;
    }
    
    protected IEnumerator ShowMuzzleFlash()
    {
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
        yield return new WaitForSeconds(muzzleFlashDuration);
        Destroy(muzzleFlash);
    }
}
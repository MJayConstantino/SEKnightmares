using System.Collections;
using UnityEngine;

[System.Serializable]
public class Phase
{
    public string phaseName;
    public float fireInterval;
}

public class BossAttack : MonoBehaviour
{
    [SerializeField] Phase[] phases;
    public GameObject bulletPrefab;
    public Transform[] firstPhaseFirePoints;
    public Transform[] secondPhaseFirePoints;
    public Animator[] animators;
    public Animator[] gun2animators;
    public float fireForce = 25f;

    private int currentPhase = 1; // Initial phase is 1

    void Start()
    {
        // Start the attack routine
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            foreach (Phase phase in phases)
            {
                yield return new WaitForSeconds(phase.fireInterval);
                Fire();
            }
        }
    }

    public void SwitchToPhase(int phase)
    {
        // Disable fire points of the current phase
        if (currentPhase == 1)
        {
            foreach (Transform firePoint in firstPhaseFirePoints)
            {
                firePoint.gameObject.SetActive(false);
            }
        }
        else if (currentPhase == 2)
        {
            foreach (Transform firePoint in secondPhaseFirePoints)
            {
                firePoint.gameObject.SetActive(false);
            }
        }

        // Switch to the new phase
        currentPhase = phase;

        // Enable fire points of the new phase
        if (currentPhase == 1)
        {
            foreach (Transform firePoint in firstPhaseFirePoints)
            {
                firePoint.gameObject.SetActive(true);
            }
        }
        else if (currentPhase == 2)
        {
            foreach (Transform firePoint in secondPhaseFirePoints)
            {
                firePoint.gameObject.SetActive(true);
            }
        }
    }

    public void Fire()
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("IsAttacking", true);
        }

        foreach (Animator gun2animator in gun2animators)
        {
            gun2animator.SetBool("IsAttackingPhase2", true);
        }

        Debug.Log("RAHHH!"); // Debugging statement

        // Use fire points based on the current phase
        if (currentPhase == 1)
        {
            foreach (Transform firePoint in firstPhaseFirePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            }
        }
        else if (currentPhase == 2)
        {
            foreach (Transform firePoint in secondPhaseFirePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            }
        }

        StartCoroutine(ResetAfterDelay(0.4f));
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Animator animator in animators)
        {
            animator.SetBool("IsAttacking", false);
        }

        foreach (Animator gun2animator in gun2animators)
        {
            gun2animator.SetBool("IsAttackingPhase2", false);
        }
    }

    public void SetPhaseTwo(){
        currentPhase = 2;
    }
}

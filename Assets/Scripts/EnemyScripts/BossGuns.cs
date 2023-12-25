using System.Collections;
using UnityEngine;

public class BossGun : MonoBehaviour
{
    public Transform bossTransform; // Reference to the boss's transform
    public float orbitRadius; // Radius of the orbit
    public float rotationSpeed = 30.0f; // Speed of the rotation
    public float initialOffsetAngle; // Offset angle for starting position in the orbit

    // Update is called once per frame
    void Update()
    {
        OrbitAroundBoss();
    }

    void OrbitAroundBoss()
    {
        // Ensure the boss transform is assigned
        if (bossTransform == null)
        {
            Debug.LogWarning("Boss Transform not assigned to EnemyGun script.");
            return;
        }

        // Calculate the desired position in the orbit with offset
        float angle = bossTransform.eulerAngles.z + initialOffsetAngle + rotationSpeed * Time.time;
        Vector2 offset = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * orbitRadius;
        Vector2 orbitPosition = (Vector2)bossTransform.position + offset;

        // Move the gun towards the orbit position
        transform.position = Vector2.MoveTowards(transform.position, orbitPosition, rotationSpeed * Time.deltaTime);

        // Rotate the gun to always face away from the boss
        Vector2 directionAwayFromBoss = (transform.position - bossTransform.position).normalized;
        float lookAngle = Mathf.Atan2(directionAwayFromBoss.y, directionAwayFromBoss.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);

        // Rotate the sprite to match the gun's rotation
        float spriteAngle = Mathf.Atan2(-directionAwayFromBoss.x, directionAwayFromBoss.y) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.AngleAxis(spriteAngle, Vector3.forward);
    }

    // Method to shoot bullets away from the boss
    public void Shoot()
    {
        // Ensure the boss transform is assigned
        if (bossTransform == null)
        {
            Debug.LogWarning("Boss Transform not assigned to EnemyGun script.");
            return;
        }

        // Calculate the direction away from the boss
        Vector2 directionAwayFromBoss = (transform.position - bossTransform.position).normalized;

        
    }
}
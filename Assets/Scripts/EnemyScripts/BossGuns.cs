using System.Collections;
using UnityEngine;

public class BossGun : MonoBehaviour
{
    public Transform bossTransform;
    public float orbitRadius;
    public float rotationSpeed = 30.0f;
    public float initialOffsetAngle;
    void Update()
    {
        OrbitAroundBoss();
    }

    void OrbitAroundBoss()
    {
        float angle = bossTransform.eulerAngles.z + initialOffsetAngle + rotationSpeed * Time.time;
        Vector2 offset = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * orbitRadius;
        Vector2 orbitPosition = (Vector2)bossTransform.position + offset;
        transform.position = Vector2.MoveTowards(transform.position, orbitPosition, rotationSpeed * Time.deltaTime);
        Vector2 directionAwayFromBoss = (transform.position - bossTransform.position).normalized;
        float lookAngle = Mathf.Atan2(directionAwayFromBoss.y, directionAwayFromBoss.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
        float spriteAngle = Mathf.Atan2(-directionAwayFromBoss.x, directionAwayFromBoss.y) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.AngleAxis(spriteAngle, Vector3.forward);
    }

    public void Shoot()
    {
        Vector2 directionAwayFromBoss = (transform.position - bossTransform.position).normalized;
    }
}
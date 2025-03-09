using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParentRanged : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public GameObject firepoint;
    public Vector2 PointerPosition { get; set; }
    public float rotationSpeed = 1000f;


    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;

        transform.right = direction;

        Vector2 scale = transform.localScale;

        if (direction.x < 0)
        {
            scale.y = -1;
            RotateFirepoint(90f);
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
            RotateFirepoint(-90f);
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    private void RotateFirepoint(float targetRotation)
    {
        float currentRotation = firepoint.transform.localEulerAngles.z;
        float newRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
        firepoint.transform.localEulerAngles = new Vector3(0f, 0f, newRotation);
    }
}

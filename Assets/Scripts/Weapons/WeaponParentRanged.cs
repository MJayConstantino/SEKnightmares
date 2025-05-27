using UnityEngine;

public class WeaponParentRanged : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    private void Start()
    {
        if (!characterRenderer)
            characterRenderer = GetComponentInParent<SpriteRenderer>();
        
        if (!weaponRenderer)
            weaponRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector3 scale = transform.localScale;
        scale.y = direction.x < 0 ? -1 : 1;
        transform.localScale = scale;

        UpdateWeaponOrder();
    }

    private void UpdateWeaponOrder()
    {
        if (!characterRenderer || !weaponRenderer) return;
        
        float angle = transform.eulerAngles.z;
        weaponRenderer.sortingOrder = (angle > 0 && angle < 180) 
            ? characterRenderer.sortingOrder - 1 
            : characterRenderer.sortingOrder + 1;
    }
}
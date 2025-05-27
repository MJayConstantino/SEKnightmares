using UnityEngine;

public class WeaponParentRanged : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    private void Start()
    {
        // Get character renderer from parent (Player)
        if (!characterRenderer)
            characterRenderer = GetComponentInParent<SpriteRenderer>();
        
        // Get weapon renderer from child (Calculator)
        if (!weaponRenderer)
            weaponRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        // Flip weapon based on direction
        Vector3 scale = transform.localScale;
        scale.y = direction.x < 0 ? -1 : 1;
        transform.localScale = scale;

        // Update weapon rendering order
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
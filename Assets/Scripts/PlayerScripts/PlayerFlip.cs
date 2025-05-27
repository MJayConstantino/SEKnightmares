using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlip : MonoBehaviour
{
    public SpriteRenderer characterRenderer;
    public InputActionReference pointerPosition;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 mouseScreenPos = pointerPosition.action.ReadValue<Vector2>();
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = mouseWorldPos - (Vector2)transform.position;

        // Flip the character based on mouse position relative to player
        characterRenderer.flipX = direction.x < 0;
    }
}
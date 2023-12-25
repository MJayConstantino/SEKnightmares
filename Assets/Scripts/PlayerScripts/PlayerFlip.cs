using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlip : MonoBehaviour
{
    public SpriteRenderer characterRenderer;
    public InputActionReference pointerPosition;

    private void Update()
    {
        Vector2 mousePos = pointerPosition.action.ReadValue<Vector2>();


        // Flip the player based on the mouse direction
        if (mousePos.x < 1000)
        {
            characterRenderer.flipX = true;
        }
        else if (mousePos.x > 1000)
        {
            characterRenderer.flipX = false; // Reset scale to normal
        }
    }
}

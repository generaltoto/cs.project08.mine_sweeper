using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputProvider : MonoBehaviour
{
    public event Action LeftClicked;

    public event Action RightClicked;
    
    public event Action MiddleClicked;

    private void OnAction(InputValue input)
    {
        Debug.LogWarning("Entered OnAction");
        
        if (!input.isPressed) return;
        
        Debug.LogWarning("Mouse input detected");

        if (Mouse.current.leftButton.wasReleasedThisFrame) LeftClicked?.Invoke();
        
        else if (Mouse.current.rightButton.wasReleasedThisFrame) RightClicked?.Invoke();
        
        else if (Mouse.current.middleButton.wasReleasedThisFrame) MiddleClicked?.Invoke();
    }
}
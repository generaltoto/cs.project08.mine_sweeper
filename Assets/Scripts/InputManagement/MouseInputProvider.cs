using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputProvider : MonoBehaviour
{
    public Vector2 MousePosition { get; private set; }
    public event Action LeftClicked;

    public event Action RightClicked;
    
    private void OnMove (InputValue input) => MousePosition = Camera.main.ScreenToWorldPoint(input.Get<Vector2>());

    private void OnLeftClick(InputValue input) => LeftClicked?.Invoke();

    private void OnRightClick(InputValue input) => RightClicked?.Invoke();
}
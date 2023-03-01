using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputProvider : MonoBehaviour
{
    public Vector3 MousePosition { get; private set; }
    public event Action LeftClicked;
    public event Action RightClicked;

    public void OnMove(InputValue input) => MousePosition = Camera.main!.ScreenToWorldPoint(input.Get<Vector2>());

    public void OnLeftClick(InputValue _) => LeftClicked?.Invoke();

    public void OnRightClick(InputValue _) => RightClicked?.Invoke();
}
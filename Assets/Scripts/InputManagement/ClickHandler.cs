using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent leftClicked;
        [SerializeField] private UnityEvent rightClicked;
        
        private BoxCollider2D _boxCollider2D;

        private MouseInputProvider _mouseInputProvider;

        private void Awake()
        {
            _mouseInputProvider = FindObjectOfType<MouseInputProvider>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _mouseInputProvider.LeftClicked += OnMouseLeftClick;
            _mouseInputProvider.RightClicked += OnMouseRightClick;
        }

        private void OnMouseLeftClick()
        {
            if (_boxCollider2D.OverlapPoint(_mouseInputProvider.MousePosition)) leftClicked.Invoke();
        }
        private void OnMouseRightClick()
        {
            if (_boxCollider2D.OverlapPoint(_mouseInputProvider.MousePosition)) rightClicked.Invoke();
        }
    }
}
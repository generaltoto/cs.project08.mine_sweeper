using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent leftClicked;
        [SerializeField] private UnityEvent rightClicked;
        [SerializeField] private UnityEvent middleClicked;
        
        private MouseInputProvider _mouseInputProvider;
        
        private void Awake()
        {
            _mouseInputProvider = FindObjectOfType<MouseInputProvider>();
            _mouseInputProvider.LeftClicked += OnMouseLeftClick;
            _mouseInputProvider.RightClicked += OnMouseRightClick;
            _mouseInputProvider.MiddleClicked += OnMouseMiddleClick;
        }
        
        private void OnMouseLeftClick() => leftClicked.Invoke();
        private void OnMouseRightClick() => rightClicked.Invoke();
        private void OnMouseMiddleClick() => middleClicked.Invoke();
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class CameraMoveHandler : MonoBehaviour
    {
        public void OnDrag(InputAction.CallbackContext ctx)
        {
            if (ctx.started) _dragOrigin = GetMousePosition;
            _isDragging = ctx.started || ctx.performed;
        }

        public void OnZoom(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;

            float zoomValue = ctx.ReadValue<float>() * ZOOM_FACTOR;
            mainCamera.orthographicSize -= zoomValue;
        }

        private const float ZOOM_FACTOR = 0.005f;
        private Vector3 _dragOrigin;

        [SerializeField] private Camera mainCamera;

        private bool _isDragging;
        
        private void LateUpdate()
        {
            if (!_isDragging) return;

            Transform camTransform = transform;
            Vector3 difference = GetMousePosition - camTransform.position;
            camTransform.position = _dragOrigin - difference;
        }
        
        private Vector3 GetMousePosition => mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
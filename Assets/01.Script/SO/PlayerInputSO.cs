using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Camera = UnityEngine.Camera;

namespace _01.Script.SO
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/Input/PlayerInput")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public Vector2 MovementKey { get; private set; }
        public event Action<bool> IsMoveThreshold;
        public event Action OnEscapePressed;
        public event Action<int> OnClickEvent;

        public Vector2 mousePosition;

        public Vector3 _worldPosition;

        private Controls _controls;

        [SerializeField] private LayerMask _clickLayerMask;
        [SerializeField] private LayerMask _interactLayerMask;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }

            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsMoveThreshold?.Invoke(true);
            }
            else if (context.canceled)
            {
                IsMoveThreshold?.Invoke(false);
            }

            MovementKey = context.ReadValue<Vector2>();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnClickEvent?.Invoke(0);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnClickEvent?.Invoke(1);
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {

        }

        public void OnESC(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnEscapePressed?.Invoke();
            }
        }

        public void OnMousePos(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
        }

        public RaycastHit GetClickRay()
        {
            Camera mainCam = Camera.main;
            Debug.Assert(mainCam != null, "No main camera in this scene");
            Ray camRay = mainCam.ScreenPointToRay(mousePosition);
            Physics.Raycast(camRay, out RaycastHit hit, mainCam.farClipPlane, _clickLayerMask);
            return hit;
        }

        public RaycastHit GetInteractRay()
        {
            Camera mainCam = Camera.main;
            Debug.Assert(mainCam != null, "No main camera in this scene");
            Ray camRay = mainCam.ScreenPointToRay(mousePosition);
            Physics.Raycast(camRay, out RaycastHit hit, mainCam.farClipPlane, _interactLayerMask);
            return hit;
        }
    }
}

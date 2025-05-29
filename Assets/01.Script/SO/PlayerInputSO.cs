using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Script.SO
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/Input/PlayerInput")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public Vector2 MovementKey { get; private set; }
        public Action<bool> IsMoveThreshold;
        
        public Action OnEscapePressed;
        
        public Action OnJumpEvent;
        
        private Controls _controls;
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

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnESC(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnEscapePressed?.Invoke();
            }
        }
    }
}

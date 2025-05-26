using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Script.SO
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInput")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public Vector2 MoveVector { get; private set; }
        
        public Action OnEscapePressed;
        
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
            MoveVector = context.ReadValue<Vector2>();
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

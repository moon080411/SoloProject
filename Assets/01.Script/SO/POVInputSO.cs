using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Script.SO
{
    [CreateAssetMenu(fileName = "POVInput", menuName = "SO/Input/POV", order = 0)]
    public class POVInputSO : ScriptableObject,Controls.IPOVActions
    {
        public Vector2 PointerPos { get; private set; }
        public event Action<Vector2> PointerDeltaChanged;
        public event Action<bool> LookedChange;
        public event Action<float> ZoomedChange;
        private Controls _controls;
        
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.POV.SetCallbacks(this);
            }
            _controls.POV.Enable();
        }
        private void OnDisable()
        {
            _controls.POV.Disable();
        }
        public void OnLooked(InputAction.CallbackContext context)
        {
            if(context.performed)
                LookedChange?.Invoke(true);
            if(context.canceled)
                LookedChange?.Invoke(false);
        }

        public void OnPointerDelta(InputAction.CallbackContext context)
        {
            PointerDeltaChanged?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            PointerPos = context.ReadValue<Vector2>();
        }

        public void OnZoomIn(InputAction.CallbackContext context)
        {
            ZoomedChange?.Invoke(context.ReadValue<Vector2>().y);
        }
    }
}
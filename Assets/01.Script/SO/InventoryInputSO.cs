using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Script.SO
{
    [CreateAssetMenu(fileName = "InventoryInputSO", menuName = "SO/Input/Inventory")]
    public class InventoryInputSO : ScriptableObject , Controls.IInventoryActions
    {
        public event Action<int> OnNumberKeyPressed;
        
        public event Action<float> OnScrollWheel;
        
        public event Action OnQKeyPressed;
        
        private Controls _controls;
        
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Inventory.SetCallbacks(this);
            }
            _controls.Inventory.Enable();
        }

        private void OnDisable()
        {
            _controls.Inventory.Disable();
        }
        
        public void OnScroll(InputAction.CallbackContext context)
        {
            OnScrollWheel?.Invoke(context.ReadValue<Vector2>().y);
        }

        public void On_1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(0);
            }
        }

        public void On_2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(1);
            }
        }

        public void On_3(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(2);
            }
        }

        public void On_4(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(3);
            }
        }

        public void On_5(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(4);
            }
        }

        public void On_6(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(5);
            }
        }

        public void On_7(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(6);
            }
        }

        public void On_8(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(7);
            }
        }

        public void On_9(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(8);
            }
        }

        public void On_0(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnNumberKeyPressed?.Invoke(9);
            }
        }

        public void OnQ(InputAction.CallbackContext context)
        {
            OnQKeyPressed?.Invoke();
        }
    }
}

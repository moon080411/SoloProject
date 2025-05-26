using System;
using _01.Script.SO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

namespace _01.Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeedX = 5f;
        [SerializeField] private Transform parent;
        [SerializeField] private Rigidbody rb;
        
        private Vector2 _moveDir;
        
        private bool _canMovement;

        private void Awake()
        {
            CursorLock(true);
            playerInput.OnEscapePressed += CursorChange;
        }
        
        private void OnDestroy()
        {
            playerInput.OnEscapePressed -= CursorChange;
        }

        private void CursorChange()
        {
            CursorLock(!_canMovement);
        }
        
        private void CursorLock(bool lockState)
        {
            if (lockState)
            {
                Cursor.lockState = CursorLockMode.Locked;
                _canMovement = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                _canMovement = false;
            }
        }
        
        private void FixedUpdate()
        {
            if(!_canMovement)
                return;
            _moveDir = playerInput.MoveVector;
            MovePosition();
            MoveRotation();
        }

        private void MovePosition()
        {
            Vector3 velocity = parent.forward * (_moveDir.y * moveSpeed * Time.fixedDeltaTime);
            rb.AddForce(velocity * 100f, ForceMode.Force);
        }
        
        private void MoveRotation()
        {
            LookChange(_moveDir);
        }
        
        public void LookChange(Vector2 delta)
        {
            if (_canMovement == false)
                return;

            float rotationY = delta.x * turnSpeedX * Time.fixedDeltaTime;

            Vector3 changedEuler = parent.eulerAngles;

            changedEuler.y += rotationY;

            rb.angularVelocity = new Vector3(0f, rotationY, 0f);
        }
    }
}

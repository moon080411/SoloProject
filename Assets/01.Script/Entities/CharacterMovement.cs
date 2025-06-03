using _01.Script.Players;
using _01.Script.SO;
using UnityEngine;

namespace _01.Script.Entities
{
    public class CharacterMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private Transform rotateTarget;
        [SerializeField] private float jumpPower = 6f;
        [SerializeField] private float moveThresholdTime;
        
        private bool _isMoveCheck = false;
        private float _timer;
        public bool IsCanMove { get; private set; } = false;

        
        PlayerInputSO _playerInput;
        
        private Transform parent;
        private float RotateTargetY => rotateTarget.eulerAngles.y;
        
        private Rigidbody _rb;


        private Vector3 _velocity;
        private Vector3 _movementDirection;
        
        private Entity _entity;

        
        public void Initialize(Entity entity)
        {
            Player player = entity as Player;
            _playerInput = player.PlayerInput;
            _entity = entity;
            parent = entity.transform;
            _rb = entity.Rb;
            _playerInput.IsMoveThreshold += IsMoveChecking;
            _timer = moveThresholdTime;
        }

        public void OnDestroy()
        {
            _playerInput.IsMoveThreshold -= IsMoveChecking;
        }

        public void SetMovementDirection(Vector2 movementInput , float movementProportion = 1f)
        {
            _movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
            _movementDirection *= movementProportion;
        }
        
        private void IsMoveChecking(bool isMove)
        {
            _isMoveCheck = isMove;
        }
        private void FixedUpdate()
        {
            if (IsCanMove)
            {
                CalculateMovement();
                Move();
            }
            else
            {
                StopImmediately();
            }
        }

        private void Update()
        {
            if (_isMoveCheck)
            {
                if (_timer <= 0)
                {
                    IsCanMove = true;
                }
                else
                {
                    IsCanMove = false;
                    _timer -= Time.deltaTime;
                }
            }
            else
            {
                _timer = moveThresholdTime;
                IsCanMove = false;
            }
        }

        private void CalculateMovement()
        {
            _velocity = Quaternion.Euler(0, RotateTargetY , 0) * _movementDirection;
            _velocity *= moveSpeed * Time.fixedDeltaTime * 100f;
            if (_velocity.magnitude > 0)
            {
                Quaternion targetRot = Quaternion.LookRotation(_velocity);
                float rotationSpeed = 8f;
                parent.rotation = Quaternion.Lerp(parent.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
            }

            _velocity.y = _rb.linearVelocity.y;
        }

        private void Move()
        {
            _rb.linearVelocity = _velocity;
        }

        public void StopImmediately()
        {
            _movementDirection = Vector3.zero;
            _rb.linearVelocity = new Vector3(0,_rb.linearVelocity.y,0);
        }

        public void Jump()
        {
            _rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}

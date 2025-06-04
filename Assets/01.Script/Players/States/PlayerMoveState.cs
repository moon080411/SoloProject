using _01.Script.Entities;
using UnityEngine;

namespace _01.Script.Players.States
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            _movement.SetMovementDirection(movementKey);
            if(movementKey.magnitude < _inputThreshold && !_movement.IsCanMove)
            {
                _player.ChangeState("IDLE");
            }
        }
    }
}

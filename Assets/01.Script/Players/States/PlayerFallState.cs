using _01.Script.Entities;
using UnityEngine;

namespace _01.Script.Players.States
{
    public class PlayerFallState : PlayerState
    {
        public PlayerFallState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            _movement.SetMovementDirection(movementKey , 0.5f);
            if(_groundChecker.GroundCheck())
            {
                _player.ChangeState("IDLE");
            }
        }
    }
}

using _01.Script.Entities;
using UnityEngine;

namespace _01.Script.Players.States
{
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(Entity entity, int animationHash) : base(entity, animationHash)
        {

        }

        public override void Enter()
        {
            base.Enter();
            Jump();
            //_animatorTrigger.OnJumpEventTrigger += Jump;
        }

        public override void Exit()
        {
            base.Exit();
            isJumped = false;
            //_animatorTrigger.OnJumpEventTrigger -= Jump;
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            _movement.SetMovementDirection(movementKey , 0.5f);
            if(isJumped == false)
                return;
            if(_player.Rb.linearVelocity.y <= 0.01 )
            {
                _player.ChangeState("FALL");
            }
        }
    }
}

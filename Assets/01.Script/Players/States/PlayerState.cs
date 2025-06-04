using System.Diagnostics;
using _01.Script.Entities;
using _01.Script.FSM;

namespace _01.Script.Players.States
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;
        protected readonly float _inputThreshold = 0.1f;

        protected CharacterMovement _movement;

        protected GroundChecker _groundChecker;
        
        protected bool isJumped;

        public PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            Debug.Assert(true, "1");
            _movement = entity.GetCompo<CharacterMovement>();
            _groundChecker = entity.GetCompo<GroundChecker>();
            _player = entity as Player;
        }

        protected void JumpStateChange()
        {
            if(_groundChecker.GroundCheck())
            {
                _player.ChangeState("JUMP");
            }
        }
        
        protected void Jump()
        {
            if (_groundChecker.GroundCheck())
            {
                _movement.Jump();
                isJumped = true;
            }
        }

        public override void Exit() 
        { 
            base.Exit();
        }
    }
}
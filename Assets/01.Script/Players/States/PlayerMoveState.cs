using _01.Script.Entities;
using _01.Script.Manager;
using UnityEngine;

namespace _01.Script.Players.States
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        private AudioSource _audioSource;
        

        public override void Enter()
        {
            base.Enter();
            _audioSource = SoundManager.Instance.PlayWithLoop("Walk");
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

        public override void Exit()
        {
            base.Exit();
            SoundManager.Instance.StopPlay(_audioSource);
        }
    }
}

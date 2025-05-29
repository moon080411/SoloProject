using System;
using UnityEngine;

namespace _01.Script.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;
        public Action OnJumpEventTrigger;
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd()//매서드 명을 이벤트 이름과 동일하게 만들어야 한다.
        {
            OnAnimationEndTrigger?.Invoke();
        }

        private void JumpEvent()
        {
            OnJumpEventTrigger?.Invoke();
        }
    }
}

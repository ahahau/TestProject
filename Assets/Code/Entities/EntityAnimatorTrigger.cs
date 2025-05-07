using System;
using UnityEngine;

namespace Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public event Action OnAnimationEnd;

        protected Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity; //당장 쓸 곳은 없다.
        }

        protected virtual void AnimationEnd()
        {
            OnAnimationEnd?.Invoke();
        }
    }
}
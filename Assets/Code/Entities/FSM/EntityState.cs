using UnityEngine;

namespace Code.Entities.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected AnimParamSO _stateAnim;
        protected bool _isTriggerCall;
        
        protected EntityRenderer _renderer;

        public EntityState(Entity entity, AnimParamSO stateAnim)
        {
            _entity = entity;
            _stateAnim = stateAnim;
            _renderer = entity.GetCompo<EntityRenderer>();
        }

        public virtual void Enter()
        {
            _renderer.SetParam(_stateAnim, true);
            _isTriggerCall = false;
        }

        public virtual void Update()  { } //아무것도 하지 않는다.

        public virtual void Exit()
        {
            _renderer.SetParam(_stateAnim, false);
        }

        public virtual void AnimationEnd() => _isTriggerCall = true;
    }
}
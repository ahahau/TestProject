using Code.Entities;
using Code.Entities.FSM;

namespace Code.Player.FSM
{
    public abstract class PlayerState : EntityState
    {
        protected PlayerController _player;
        protected EntityMover _mover;
        
        protected PlayerState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
            _player = entity as PlayerController;
            _mover = entity.GetCompo<EntityMover>();
        }
    }
}
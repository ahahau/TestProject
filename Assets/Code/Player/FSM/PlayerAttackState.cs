using Code.Entities;

namespace Code.Player.FSM
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if(_mover.IsGrounded)
                _mover.StopImmediately(true);
            else 
                _mover.StopFalling();
        }

        public override void Update()
        {
            base.Update();
            const string idleName = "IDLE";
            if(_isTriggerCall)
                _player.ChangeState(idleName);
        }
    }
}
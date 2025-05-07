using Code.Entities;
using Code.Entities.FSM;

namespace Code.Player.FSM
{
    public class PlayerFallState : PlayerAirState, ICanAttackState, ICanDashState
    {
        public PlayerFallState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
        }

        public override void Update()
        {
            base.Update();
            if (_mover.IsGrounded)
            {
                _player.ChangeState("IDLE");
            }
        }

        
    }
}
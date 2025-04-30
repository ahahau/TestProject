using Code.Player.FSM;

namespace Code.Entities.FSM
{
    public class PlayerFallState : PlayerAirState
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

        public override void Exit()
        {
            _mover.StopImmediately(false);
            base.Exit();
        }
    }
}
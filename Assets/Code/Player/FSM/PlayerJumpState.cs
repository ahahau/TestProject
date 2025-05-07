using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Player.FSM
{
    public class PlayerJumpState : PlayerAirState, ICanAttackState, ICanDashState
    {
        public PlayerJumpState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(true);
            _mover.AddForceToEntity(new Vector2(0, _player.jumpPower));
            _mover.OnMoveVelocityChange += HandleVelocityChange;
        }

        public override void Exit()
        {
            _mover.OnMoveVelocityChange -= HandleVelocityChange;
            base.Exit();
        }

        private void HandleVelocityChange(Vector2 velocity)
        {
            if(velocity.y < 0) 
                _player.ChangeState("FALL");
        }
    }
}
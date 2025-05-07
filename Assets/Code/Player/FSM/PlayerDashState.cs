using Code.Entities;
using UnityEngine;

namespace Code.Player.FSM
{
    public class PlayerDashState : PlayerState
    {
        private float _dashStartTime;
        public PlayerDashState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.SetGravityScale(0); //중력 0으로 
            _mover.StopImmediately(true);
            _mover.CanManualMove = false;

            Vector2 dashPower = new Vector2(_renderer.FacingDirection * _player.dashSpeed, 0);
            _mover.AddForceToEntity(dashPower);
            _dashStartTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            if (_dashStartTime + _player.dashDuration < Time.time)
            {
                const string idleName = "IDLE";
                _player.ChangeState(idleName);
            }
        }

        public override void Exit()
        {
            _mover.SetGravityScale(1f); //중력 0으로 
            _mover.StopImmediately(true);
            _mover.CanManualMove = true;
            base.Exit();
        }
    }
}
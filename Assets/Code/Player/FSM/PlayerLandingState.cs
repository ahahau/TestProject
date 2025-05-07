using Code.Entities;
using UnityEngine;

namespace Code.Player.FSM
{
    public class PlayerLandingState : PlayerState
    {
        private const float DROP_POWER = -20f;
        private const float LANDING_TIME = 0.3f;
        private float _lastLandingTime;
        
        public PlayerLandingState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanManualMove = false; //못움직이게
            _mover.StopImmediately(false); //좌우 정지
            _mover.AddForceToEntity(new Vector2(0, DROP_POWER));
            _mover.OnGroundStatusChange += HandleGroundDetect;
            //_lastLandingTime = Time.time; //현재 시간 기록
        }

        public override void Exit()
        {
            _mover.OnGroundStatusChange -= HandleGroundDetect;
            _mover.CanManualMove = true; //다시 움직이게
            base.Exit();
        }

        private void HandleGroundDetect(bool isGround)
        {
            if (isGround)
            {
                _lastLandingTime = Time.time;
                _mover.StopImmediately(true);
                _mover.SendOnGroundParam(); //그라운드 착지 애니메이션으로 전환.
            }
        }

        public override void Update()
        {
            base.Update();
            if (_mover.IsGrounded == false) return;
            
            const string idleName = "IDLE";
            if (_lastLandingTime + LANDING_TIME < Time.time)
            {
                _player.ChangeState(idleName);
            }
        }
    }
}
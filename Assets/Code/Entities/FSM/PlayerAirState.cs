using Code.Player.FSM;
using UnityEngine;

namespace Code.Entities.FSM
{
    public abstract class PlayerAirState : PlayerState
    {
        private const float AirSpeedMultiplier = 0.7f;
        protected PlayerAirState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.SetMoveSpeedMultiplier(0.7f);
        }

        public override void Exit()
        {
            _mover.SetMoveSpeedMultiplier(1f);
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            float xMove = _player.PlayerInput.InputDirection.x;
            if(Mathf.Abs(xMove) > 0f)
            {
                _mover.SetMovementX(xMove);
            }
        }
    }
}
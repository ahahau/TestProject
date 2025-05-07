using Code.Entities;
using UnityEngine;

namespace Code.Player.FSM
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
            _player.PlayerInput.OnDropPress += HandleOnDrop;
            _mover.SetMoveSpeedMultiplier(AirSpeedMultiplier);
        }

        public override void Exit()
        {
            _player.PlayerInput.OnDropPress -= HandleOnDrop;
            _mover.SetMoveSpeedMultiplier(1f);
            base.Exit();
        }

        private void HandleOnDrop()
        {
            const string landingName = "LANDING";
            _player.ChangeState(landingName);
        }

        public override void Update()
        {
            base.Update();
            float xMove = _player.PlayerInput.InputDirection.x;
            if (Mathf.Abs(xMove) > 0)
            {
                _mover.SetMovementX(xMove);
            }
        }
    }
}
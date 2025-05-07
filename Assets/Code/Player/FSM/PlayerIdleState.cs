using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Player.FSM
{
    public class PlayerIdleState : PlayerState, ICanAttackState, ICanDashState
    {
        public PlayerIdleState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(false);
        }

        public override void Update()
        {
            base.Update();
            float xMove = _player.PlayerInput.InputDirection.x;
            if(Mathf.Abs(xMove) > 0) 
                _player.ChangeState("MOVE");
        }
    }
}
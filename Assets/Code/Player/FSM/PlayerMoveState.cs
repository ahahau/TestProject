using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;

namespace Code.Player.FSM
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Entity entity, AnimParamSO stateAnim) : base(entity, stateAnim)
        {
            
        }

        public override void Update()
        {
            base.Update();
            float xMove = _player.PlayerInput.InputDirection.x;
            _mover.SetMovementX(xMove);

            if (Mathf.Approximately(xMove, 0))
            {
                _player.ChangeState("IDLE");
            }
        }
    }
}
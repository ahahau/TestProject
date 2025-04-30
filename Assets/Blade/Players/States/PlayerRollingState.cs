using Blade.Entities;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerRollingState : PlayerState
    {
        private CharacterMovement _movement;
        private bool _isRolling;
        private Vector3 _rollingDirection;
        
        public PlayerRollingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
            _isRolling = false;

            _animatorTrigger.OnRollingStatusChange += HandleRollingStatusChange;
            _rollingDirection = _player.transform.forward;
        }

        public override void Exit()
        {
            _movement.CanManualMovement = true;
            _animatorTrigger.OnRollingStatusChange -= HandleRollingStatusChange;
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        private void HandleRollingStatusChange(bool isActive)
        {
            if (_isRolling != isActive && isActive)
            {
                _movement.SetAutoMovement(_rollingDirection * _player.rollingVelocity);
            }
            else if(isActive == false)
            {
                _movement.SetAutoMovement(_rollingDirection * (_player.rollingVelocity * 0.2f));
            }
            _isRolling = isActive;   
        }
    }
}
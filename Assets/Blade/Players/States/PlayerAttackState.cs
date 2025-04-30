using Blade.Combat;
using Blade.Entities;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerAttackState : PlayerState
    {
        private PlayerAttackCompo _attackCompo;
        private CharacterMovement _movement;
        
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }
        public override void Enter()
        {
            base.Enter();
            _attackCompo.Attack();

            _movement.CanManualMovement = false;
            ApplyAttackData();
        }

        private void ApplyAttackData()
        {
            AttackDataSO currentAtkData = _attackCompo.GetCurrentAttackData();
            Vector3 playerDirection = GetPlayerDirection();
            _player.transform.rotation = Quaternion.LookRotation(playerDirection); //이거 나중에 쓰인다.

            Vector3 movement = playerDirection * currentAtkData.movementPower;
            _movement.SetAutoMovement(movement);
        }

        private Vector3 GetPlayerDirection()
        {
            if(_attackCompo.useMouseDirection == false)
                return _player.transform.forward;
            
            Vector3 targetPos = _player.PlayerInput.GetWorldPosition();
            Vector3 direction = targetPos - _player.transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        public override void Exit()
        {
            _attackCompo.EndAttack();
            _movement.CanManualMovement = true;
            _movement.StopImmediately();
            base.Exit();
        }
        public override void Update()
        {
            base.Update();
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }
    }
}

/*
애니메이션 이벤트로 로직 함수를 건드리면 안된다.
이벤트로는 변수에 값을 저장하고
해당 값을 다음 프레임에서 이용하도록 해야 한다. 
*/
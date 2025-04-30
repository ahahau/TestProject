using Blade.Entities;
using Blade.FSM;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerMoveState : PlayerCanAttackState
    {
        private CharacterMovement _movement;
        private EntityVFX _vfxCompo;
        private readonly string _footStepEffectName = "FootStep";
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
            _vfxCompo = entity.GetCompo<EntityVFX>();
        }

        public override void Enter()
        {
            base.Enter();
            _vfxCompo.PlayVfx(_footStepEffectName, Vector3.zero, Quaternion.identity);
        }

        public override void Exit()
        {
            _vfxCompo.StopVfx(_footStepEffectName);
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            _movement.SetMovementDirection(movementKey);
            if(movementKey.magnitude < _inputThreshold)
                            _player.ChangeState("IDLE");
        }
    }
}
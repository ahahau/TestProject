using Blade.Entities;
using Blade.FSM;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerIdleState : PlayerCanAttackState
    {
        private CharacterMovement _movement;
        public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            _movement.SetMovementDirection(movementKey);
            if (movementKey.magnitude > _inputThreshold)
            {
                _player.ChangeState("MOVE");
            }
        }
    }
}
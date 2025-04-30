using Blade.Entities;

namespace Blade.Players.States
{
    public abstract class PlayerCanAttackState : PlayerState
    {
        public PlayerCanAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAttackPressed += HandleAttackPressed;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnAttackPressed -= HandleAttackPressed;
            base.Exit();
        }

        private void HandleAttackPressed()
        {
            _player.ChangeState("ATTACK");
        }
    }
}
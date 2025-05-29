using Code.Entities;
using UnityEngine;

namespace Code.Combat
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IDamageable
    {
        [SerializeField] private float maxHealth = 50f;
        [SerializeField] private float knockBackRecoverDuration = 0.5f;
        
        private float _currentHealth;
        private Entity _entity;
        private EntityMover _mover;
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _mover = entity.GetCompo<EntityMover>();
            _currentHealth = maxHealth;
        }

        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockback, Entity dealer)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
            ApplyKnockback(direction, knockBackRecoverDuration);
            
            _entity.OnHitEvent?.Invoke();

            if (_currentHealth <= 0)
            {
                _entity.OnDeadEvent?.Invoke();
            }
        }

        private async void ApplyKnockback(Vector2 knockBack, float duration)
        {
            _mover.CanManualMove = false;
            _mover.StopImmediately(true);
            _mover.AddForceToEntity(knockBack);
            await Awaitable.WaitForSecondsAsync(duration);
            _mover.CanManualMove = true;
        }
    }
}
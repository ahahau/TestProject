using Code.Entities;
using UnityEngine;

namespace Code.Combat
{
    public interface IDamageable
    {
        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockback, Entity dealer);
    }
}
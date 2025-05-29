using System;
using Code.Entities;
using UnityEngine;

namespace Code.Combat
{
    public class DamageCaster : MonoBehaviour
    {
        [SerializeField] private ContactFilter2D contactFilter;
        [SerializeField] private int maxAvailableCount = 4;
        [SerializeField] private Vector2 castSize;

        protected Collider2D[] _resultColliders;
        protected Entity _owner;

        public void InitCaster(Entity entity)
        {
            _owner = entity;
            _resultColliders = new Collider2D[maxAvailableCount];
        }

        public void CastDamage(float damage, Vector2 direction, Vector2 knockBack)
        {
            int count = Physics2D.OverlapBox(transform.position, castSize, 0f, contactFilter, _resultColliders);
            for (int i = 0; i < count; i++)
            {
                if (_resultColliders[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(damage, direction, knockBack, _owner);
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, castSize); 
        }
#endif
    }
}
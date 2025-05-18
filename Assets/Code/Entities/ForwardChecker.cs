using System;
using UnityEngine;

namespace Code.Entities
{
    public class ForwardChecker : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float checkRadius = 0.25f;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float wallCheckOffset = 0.25f;
        [SerializeField] private LayerMask whatIsWall;
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public bool CheckForwardGround() 
            => Physics2D.OverlapCircle(transform.position, checkRadius, whatIsGround);

        public bool CheckForwardWall()
            => Physics2D.OverlapCircle(transform.position + new Vector3(0, wallCheckOffset), checkRadius, whatIsWall);
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, checkRadius);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, wallCheckOffset), checkRadius);
        }
    }
}
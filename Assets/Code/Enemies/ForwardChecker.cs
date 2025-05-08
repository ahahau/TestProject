using System;
using Code.Entities;
using UnityEngine;

namespace Code.Enemies
{
    public class ForwardChecker : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float checkRadius = 0.25f;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private GameObject wallChecker;
        [SerializeField] private LayerMask whatIsWall;
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        public bool CheckForwardGround() => Physics2D.OverlapCircle(transform.position, checkRadius, whatIsGround);
        public bool CheckForwardWall() => Physics2D.OverlapCircle(wallChecker.transform.position, checkRadius, whatIsWall);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, checkRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(wallChecker.transform.position, checkRadius);
        }
    }
}
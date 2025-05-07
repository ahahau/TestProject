using Code.Entities;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies
{
    public abstract class Enemy : Entity
    {
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected float bodyOffset = 0.5f;
        
        protected BehaviorGraphAgent _btAgent;
        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _btAgent = GetComponent<BehaviorGraphAgent>();
        }

        public RaycastHit2D CheckPlayerInRange(float distance)
        {
            return Physics2D.Raycast(transform.position+ new Vector3(0,bodyOffset), transform.right, distance, whatIsPlayer);
        }
    }
}
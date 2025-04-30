using System;
using Blade.Entities;
using Unity.Behavior;
using UnityEngine;

namespace Blade.Enemies
{
    public abstract class Enemy : Entity
    {
        [field: SerializeField] public EntityFinderSO PlayerFinder { get; protected set; }

        public BehaviorGraphAgent BtAgent => _btAgent;
        protected BehaviorGraphAgent _btAgent;

        #region Temp region

        public float detectRange;
        public float attackRange;

        #endregion

        private void Start()
        {
            BlackboardVariable<Transform> target = GetBlackboardVariable<Transform>("Target");
            target.Value = PlayerFinder.Target.transform; //플레이어를 타겟으로 넣어준다. 
        }

        protected override void AddComponents()
        {
            base.AddComponents();
            _btAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(_btAgent != null, $"{gameObject.name} does not have an BehaviorGraphAgent");
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (_btAgent.GetVariable(key, out BlackboardVariable<T> result))
                return result;
            return default;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
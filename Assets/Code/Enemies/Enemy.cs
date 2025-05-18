using Code.Entities;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Enemies
{
    public abstract class Enemy : Entity
    {
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected float bodyOffset = 0.5f;
        protected BehaviorGraphAgent _btAgent;
        public BehaviorGraphAgent BTAgent => _btAgent;

        private const string TargetKey = "Target";
        private Transform _target;

        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                BlackboardVariable<Transform> targetTrm = GetBlackboardVariable<Transform>(TargetKey);
                Debug.Assert(targetTrm != null, $"Blackboard variable {TargetKey} not found.");
                targetTrm.Value = value;
            }
        }

        private BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (_btAgent.GetVariable(key, out BlackboardVariable<T> variable))
            {
                return variable;
            }

            return null;
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _btAgent = GetComponent<BehaviorGraphAgent>();
        }

        public abstract bool CheckPlayerInRange();
        public abstract bool IsTargetInAttackRange();
    }
}
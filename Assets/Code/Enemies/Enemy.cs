using Code.Combat;
using Code.Enemies.BT;
using Code.Enemies.BT.Events;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Enemies
{
    public abstract class Enemy : Entity, IDamageable
    {
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected LayerMask whatIsObstacle;
        [SerializeField] protected float bodyOffset = 0.5f;
        protected BehaviorGraphAgent _btAgent;
        public BehaviorGraphAgent BTAgent => _btAgent;
        private StateChannel _stateChannel;

        public StateChannel StateChannel
        {
            get
            {
                if (_stateChannel == null)
                    _stateChannel = GetBlackboardVariable<StateChannel>("StateChannel").Value;
                Debug.Assert(_stateChannel != null, "StateChannel not found in blackboard");
                return _stateChannel;
            }
        }
        
        private const string TargetKey = "Target";
        private Transform _target;

        public Vector3 InitPosition { get; private set; }

        public UnityEvent<float, Vector2, Vector2, Entity> OnDamageApplyEvent;
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

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            InitPosition = transform.position;
            OnHitEvent.AddListener(ChangeToHitState);
        }

        private void ChangeToHitState()
        {
            StateChannel.SendEventMessage(EnemyState.HIT);
        }

        public abstract bool CheckPlayerInRange();
        public abstract bool IsTargetInAttackRange();
        public abstract bool CheckChaseTargetInRange(Transform target);
        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockBack, Entity dealer)
        {
            OnDamageApplyEvent?.Invoke(damage, direction, knockBack, dealer);
        }
    }
}
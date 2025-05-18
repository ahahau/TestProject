using System;
using System.Collections.Generic;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Code.Player
{
    public class PlayerController : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        public float dashSpeed = 25f;
        public float dashDuration = 0.2f;

        public float jumpPower = 4f;
        [SerializeField] private int jumpCount = 2;
        [SerializeField] private StateSO[] stateList;

        private int _currentJumpCount = 0;
        private EntityMover _mover;
        private PlayerAttackCompo _atkCompo;
        private StateMachine _stateMachine;

        private Dictionary<Type, (Action subscribe, Action unsubscribe)> _subscriptions;
        public string CurrentStateName { get; private set; }

        [SerializeField] private ActionBinderSO[] actionBinders;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(this, stateList);
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _mover = GetCompo<EntityMover>();
            _atkCompo = GetCompo<PlayerAttackCompo>();
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _mover.OnGroundStatusChange += HandleGroundStatusChange;
            PlayerInput.OnJumpPress += HandleJumpPress;

            _subscriptions = new Dictionary<Type, (Action, Action)>();
            foreach (var actionBinder in actionBinders)
            {
                actionBinder.Compile(this);
                _subscriptions.Add(actionBinder.InterfaceType,
                    (actionBinder.SubscribeAction, actionBinder.UnSubscribeAction));
            }
        }

        private void OnDestroy()
        {
            _mover.OnGroundStatusChange -= HandleGroundStatusChange;
            PlayerInput.OnJumpPress -= HandleJumpPress;
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE"); //처음 시작했을 때 IDLE로 설정한다.
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        public void ChangeState(string newStateName)
        {
            CurrentStateName = newStateName;
            EntityState oldState = _stateMachine.ChangeState(newStateName);

            foreach (var subscription in _subscriptions)
            {
                UpdateSubscription(subscription.Key, oldState, _stateMachine.CurrentState,
                    subscription.Value.subscribe, subscription.Value.unsubscribe);
            }
        }

        private void UpdateSubscription(Type interfaceType, EntityState oldState, EntityState newState,
            Action subscribe, Action unsubscribe)
        {
            //IsInstanceOfType => 지정된 object가 Type의 객체인지 확인합니다.
            if (interfaceType.IsInstanceOfType(oldState))
                unsubscribe?.Invoke();
            if (interfaceType.IsInstanceOfType(newState))
                subscribe?.Invoke();
        }

        private void HandleDashPress()
        {
            const string dashName = "DASH";
            ChangeState(dashName);
        }

        private void HandleAttackPress()
        {
            const string attackStateName = "ATTACK";
            if (_atkCompo.AttemptAttack())
            {
                ChangeState(attackStateName);
            }
        }

        private void HandleGroundStatusChange(bool isGrounded)
        {
            if (isGrounded)
                _currentJumpCount = jumpCount; //땅에 착지한 순간 점프카운트 리셋
        }

        private void HandleJumpPress()
        {
            const string jumpName = "JUMP";
            const string doubleJumpName = "DOUBLE_JUMP";

            if (_mover.IsGrounded || _currentJumpCount > 0)
            {
                _currentJumpCount--;
                string nextName = _mover.IsGrounded ? jumpName : doubleJumpName;
                ChangeState(nextName);
            }
        }

        public void AnimationEndTrigger() => _stateMachine.CurrentState.AnimationEnd();
    }
}
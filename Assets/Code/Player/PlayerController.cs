using System;
using Code.Entities;
using Code.Entities.FSM;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Code.Player
{
    public class PlayerController : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private float jumpPower = 4f;
        [SerializeField] private int jumpCount = 2;
        [SerializeField] private StateSO[] stateList;
        
        private int _currentJumpCount = 0;
        private EntityMover _mover;
        private StateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(this, stateList);
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _mover = GetCompo<EntityMover>();
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _mover.OnGroundStatusChange += HandleGroundStatusChange;
            PlayerInput.OnJumpPress += HandleJumpPress;
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
        
        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);

        private void HandleGroundStatusChange(bool isGrounded)
        {
            if (isGrounded)
                _currentJumpCount = jumpCount; //땅에 착지한 순간 점프카운트 리셋
        }

        private void HandleJumpPress()
        {
            if (_mover.IsGrounded || _currentJumpCount > 0)
            {
                _mover.StopImmediately(true);
                _mover.AddForceToEntity(new Vector2(0, jumpPower));
                _currentJumpCount--;
            }
        }

        
    }
}
using System;
using Blade.Core.Dependencies;
using Blade.Entities;
using Blade.FSM;
using UnityEngine;

namespace Blade.Players
{
    public class Player : Entity, IDependencyProvider
    {
        [field:SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] stateDataList;
        
        private EntityStateMachine _stateMachine;

        [Provide]
        public Player ProvidePlayer() => this;
        
        #region Temp region
        public float rollingVelocity = 12f;
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, stateDataList);
            PlayerInput.OnRollingPressed += HandleRollingPressed;
        }

        private void OnDestroy()
        {
            PlayerInput.OnRollingPressed -= HandleRollingPressed;
        }

        private void HandleRollingPressed()
        {
            ChangeState("ROLLING");
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void ChangeState(string newStateName) 
            => _stateMachine.ChangeState(newStateName);

        // private void HandleMovementChange(Vector2 movementInput)
        // {
        //     _movement.SetMovementDirection(movementInput);
        // }
    }
}
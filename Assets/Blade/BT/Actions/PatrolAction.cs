using System;
using Blade.Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Patrol", story: "[Self] patrol with [Waypoints]", category: "Action", id: "2377c1c7579fa6ff17ca308cdc7c0712")]
    public partial class PatrolAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<WayPoints> Waypoints;

        private int _currentPointIdx;
        private NavMovement _navMovement;
        
        protected override Status OnStart()
        {
            Initialize();
            _navMovement.SetDestination(Waypoints.Value[_currentPointIdx].position);
            return Status.Running;
        }

        private void Initialize()
        {
            if (_navMovement == null)
                _navMovement = Self.Value.GetCompo<NavMovement>();

        }

        protected override Status OnUpdate()
        {
            if(_navMovement.IsArrived)
                return Status.Success;
            return Status.Running;
        }

        protected override void OnEnd()
        {
            _currentPointIdx = (_currentPointIdx + 1) % Waypoints.Value.Length; 
        }
    }
}


using System;
using Code.Entities;
using Code.PathFinding;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChaseToTarget", story: "[Agent] chase to [LastTargetPosition]", category: "Enemy/Action", id: "91d3d61f54f43aeacfb68e3424dd7864")]
    public partial class ChaseToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavAgent2D> Agent;
        [SerializeReference] public BlackboardVariable<Vector3> LastTargetPosition;

        private bool _isCalculate;
        private bool _result;
        protected override Status OnStart()
        {
            const float distanceThreshold = 0.3f;
            if (Vector2.Distance(Agent.Value.Destination, LastTargetPosition.Value) > distanceThreshold)
            {
                _isCalculate = true;
                SetDestination();
                return Status.Running;
            }

            return Status.Success;
        }

        protected override Status OnUpdate()
        {
            if (_isCalculate) return Status.Running;

            if (_result) return Status.Success;
            return Status.Failure;
        }

        private async void SetDestination()
        {
            _result = await Agent.Value.SetDestination(LastTargetPosition.Value);
            _isCalculate = false;
        }

    }
}


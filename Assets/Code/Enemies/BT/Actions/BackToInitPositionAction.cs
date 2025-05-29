using System;
using Code.PathFinding;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "BackToInitPosition", story: "[Self] back to initPosition", category: "Enemy/Action", id: "83f234f43e43b76af7ca1b5e64672265")]
    public partial class BackToInitPositionAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        private NavAgent2D _agent;
        
        protected override Status OnStart()
        {
            if (_agent == null)
            {
                _agent = Self.Value.GetCompo<NavAgent2D>();
            }
            _agent.SetDestination(Self.Value.InitPosition);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(_agent.IsArrived)
                return Status.Success;
            return Status.Running;
        }
    }
}


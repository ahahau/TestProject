using System;
using Code.PathFinding;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopPathAgent", story: "Set stop [Agent] to [NewValue]", category: "Enemy/Action", id: "ec360ecaed2c112b8711026103494884")]
    public partial class StopPathAgentAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavAgent2D> Agent;
        [SerializeReference] public BlackboardVariable<bool> NewValue;

        protected override Status OnStart()
        {
            Agent.Value.IsStopped = NewValue.Value;
            return Status.Success;
        }
    }
}


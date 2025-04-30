using System;
using Blade.Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopMove", story: "[Movement] stop set to [newValue]", category: "Action", id: "869cf0828e4faaa1eaae57b2670f7533")]
    public partial class StopMoveAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<bool> NewValue;

        protected override Status OnStart()
        {
            Movement.Value.SetStop(NewValue.Value);
            return Status.Success;
        }

    }
}


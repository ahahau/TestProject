using Blade.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "[Movement] move to [Target]", category: "Action", id: "574d8030976247672ad59ebb8ad4d535")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMovement> Movement;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    protected override Status OnStart()
    {
        Movement.Value.SetDestination(Target.Value.position);
        return Status.Success;
    }

    
}


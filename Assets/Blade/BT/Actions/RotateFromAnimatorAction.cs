using Blade.Enemies;
using Blade.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateFromAnimator", story: "[Movement] rotate to [Target] with [Trigger]", category: "Enemy/Move", id: "e83c5432e2ddb3dca0d377e36445418d")]
public partial class RotateFromAnimatorAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMovement> Movement;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;

    private bool _isRotate = false;
    
    protected override Status OnStart()
    {
        Trigger.Value.OnManualRotationTrigger += HandleManualRotation;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_isRotate)
        {
            Movement.Value.LookAtTarget(Target.Value.position);
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Trigger.Value.OnManualRotationTrigger -= HandleManualRotation;
    }

    private void HandleManualRotation(bool isRotate)  => _isRotate = isRotate;
    
}


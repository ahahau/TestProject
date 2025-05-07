using Code.Enemies;
using System;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToFront", story: "[Self] move to front in [Second]", category: "Action", id: "59fc8cd7cbe15dfd0bd476dd83c9f2d5")]
public partial class MoveToFrontAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<float> Second;
    
    private float _startTime;
    protected override Status OnStart()
    {
        _startTime = Time.time;
        EntityMover mover = Self.Value.GetCompo<EntityMover>();

        float frontDirection = Self.Value.transform.right.x;
        mover.SetMovementX(frontDirection);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_startTime + Second.Value < Time.time)
        {
            return Status.Success;
            
        }
        return Status.Running;
    }
}


using Code.Enemies;
using System;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseToTarget", story: "[Enemy] chase to [Target]", category: "Action", id: "b13f14dbe439f5d7870dd5eabae868bd")]
public partial class ChaseToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    private EntityMover _mover;
    protected override Status OnStart()
    {
        if (_mover == null)
            _mover = Enemy.Value.GetCompo<EntityMover>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float directionX = Mathf.Sign(Target.Value.position.x - Enemy.Value.transform.position.x);
        _mover.SetMovementX(directionX);
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}


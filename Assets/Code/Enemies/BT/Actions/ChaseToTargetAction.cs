using System;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChaseToTarget", story: "[Enemy] chase to [Target]", category: "Enemy/Action", id: "91d3d61f54f43aeacfb68e3424dd7864")]
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

    }
}


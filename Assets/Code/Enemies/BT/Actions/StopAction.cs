using System;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Stop", story: "Stop [Enemy]", category: "Action", id: "421ce4afd4dbb476288af34b476d68db")]
    public partial class StopAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Enemy;
        private EntityMover _mover;
        
        protected override Status OnStart()
        {
            if (_mover == null)
                _mover = Enemy.Value.GetCompo<EntityMover>();
            _mover.StopImmediately(false);
            return Status.Success;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}


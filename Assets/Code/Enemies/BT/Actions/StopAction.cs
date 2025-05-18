using System;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Stop", story: "Stop [Enemy]", category: "Enemy/Action", id: "6945e41fefb47bd0622425bf075bcd26")]
    public partial class StopAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Enemy;

        private EntityMover _mover;
        protected override Status OnStart()
        {
            if(_mover == null)
                _mover = Enemy.Value.GetCompo<EntityMover>();
            _mover.StopImmediately(false);
            return Status.Success;
        }
    }
}


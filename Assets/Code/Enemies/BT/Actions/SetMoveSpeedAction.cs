using System;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetMoveSpeed", story: "[Enemy] speedMultiplier to [NewValue]", category: "Enemy/Action", id: "bd6e352592add39b68f886b19b682b70")]
    public partial class SetMoveSpeedAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Enemy;
        [SerializeReference] public BlackboardVariable<float> NewValue;

        private EntityMover _mover;
        protected override Status OnStart()
        {
            if(_mover == null)
                _mover = Enemy.Value.GetCompo<EntityMover>();
            
            _mover.SetMoveSpeedMultiplier(NewValue.Value);
            return Status.Success;
        }
    }
}


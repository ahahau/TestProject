using System;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Events
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetMoveSpeed", story: "[Enemy] speedMultiplier to [NewValue]", category: "Enemy/Action", id: "6b17bd3de83eace0fb01732da219e661")]
    public partial class SetMoveSpeedAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Enemy;
        [SerializeReference] public BlackboardVariable<float> NewValue;
        
        private EntityMover _mover;
        
        protected override Status OnStart()
        {
            if (_mover == null)
                _mover = Enemy.Value.GetCompo<EntityMover>();
            _mover.SetMoveSpeedMultiplier(NewValue.Value);
            return Status.Success;
        }
    }
}


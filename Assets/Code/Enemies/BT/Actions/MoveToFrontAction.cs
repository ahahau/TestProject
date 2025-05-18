using System;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToFront", story: "[Self] move to front in [Second]", category: "Enemy/Action", id: "412e6d0ac64fd56fc716ad6f3700ca1d")]
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
}


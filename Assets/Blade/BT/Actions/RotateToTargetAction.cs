using System;
using Blade.Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RotateToTarget", story: "[Self] rotate to [Target] in [Second]", category: "Action", id: "5afdef7a248e1fc964de452a417d9330")]
    public partial class RotateToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<float> Second;

        private const float _rotationSpeed = 20f;
        private float _startTime;
    
        protected override Status OnStart()
        {
            _startTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            LookTargetSmoothly();
            if(Time.time - _startTime >= Second.Value)
                return Status.Success;
            return Status.Running;
        }

        private void LookTargetSmoothly()
        {
            Transform trm = Self.Value.transform;
            Vector3 direction = Target.Value.position - trm.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            trm.rotation = Quaternion.Lerp(trm.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

    }
}


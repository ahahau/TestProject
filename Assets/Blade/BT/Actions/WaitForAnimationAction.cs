using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForAnimation", story: "wait for end [Trigger]", category: "Enemy/Animation", id: "77e0cb18d819ca86bb663538f1f271f9")]
    public partial class WaitForAnimationAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;

        private bool _isTriggered;
        
        protected override Status OnStart()
        {
            _isTriggered = false;
            Trigger.Value.OnAnimationEndTrigger += HandleAnimationEnd;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(_isTriggered)
                return Status.Success;
            return Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnAnimationEndTrigger -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd() => _isTriggered = true;
    }
}


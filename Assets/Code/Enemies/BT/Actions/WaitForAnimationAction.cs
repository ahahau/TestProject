using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForAnimation", story: "Wait for [Trigger]", category: "Enemy/Action", id: "13383145a141df90f71c2f5903b464c1")]
    public partial class WaitForAnimationAction : Action
    {
        [SerializeReference] public BlackboardVariable<EnemyAnimatorTrigger> Trigger;

        private bool _isAnimationEnd;
        
        protected override Status OnStart()
        {
            _isAnimationEnd = false;
            Trigger.Value.OnAnimationEnd += HandleAnimationEnd;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(_isAnimationEnd)
                return Status.Success;
            return Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnAnimationEnd -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd() => _isAnimationEnd = true;
    }
}


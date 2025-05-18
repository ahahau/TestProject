using System;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckForwardGround", story: "[Forward] ground is [CheckValue]", category: "Enemy/Conditions", id: "4b603e91df15fecda2e821ade0e7bd8f")]
    public partial class CheckForwardGroundCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<ForwardChecker> Forward;
        [SerializeReference] public BlackboardVariable<bool> CheckValue;

        public override bool IsTrue()
        {
            bool isGroundOnForward = Forward.Value.CheckForwardGround();
            return isGroundOnForward == CheckValue.Value;
        }

    }
}

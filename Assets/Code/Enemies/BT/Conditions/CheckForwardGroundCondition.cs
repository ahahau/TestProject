using System;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckForwardGround", story: "[Forward] ground is [CheckValue]", category: "Enemy/Conditions", id: "a430a299d4125ffd31fb1e7cbb087056")]
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

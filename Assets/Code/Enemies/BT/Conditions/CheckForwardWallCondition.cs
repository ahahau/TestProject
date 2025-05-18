using System;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckForwardWall", story: "[Forward] wall is [CheckValue]", category: "Enemy/Conditions", id: "ddc16ba4312932b12787c1766ca55130")]
    public partial class CheckForwardWallCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<ForwardChecker> Forward;
        [SerializeReference] public BlackboardVariable<bool> CheckValue;

        public override bool IsTrue()
        {
            bool isWallOnFront = Forward.Value.CheckForwardWall();
            return isWallOnFront == CheckValue.Value;
        }

    }
}

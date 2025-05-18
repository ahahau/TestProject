using System;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "TargetInSight", story: "[Enemy] check target in sight [CheckValue]", category: "Enemy/Conditions", id: "2af40e1465be0e9aebde54828d1eaa44")]
    public partial class TargetInSightCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Enemy> Enemy;
        [SerializeReference] public BlackboardVariable<bool> CheckValue;

        public override bool IsTrue()
        {
            bool isTargetInRange = Enemy.Value.CheckPlayerInRange();
            return isTargetInRange == CheckValue.Value;
        }
    }
}

using System;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckAttackRange", story: "[Self] check in attackRange [CheckValue]", category: "Enemy/Conditions", id: "42e820f25f60723f34aabaa45d958a57")]
    public partial class CheckAttackRangeCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<bool> CheckValue;

        public override bool IsTrue()
        {
            bool isInAttackRange = Self.Value.IsTargetInAttackRange(); //공격사거리에 있는 지 체크
            return isInAttackRange == CheckValue.Value;
        }

    }
}

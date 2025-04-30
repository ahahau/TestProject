using System;
using Blade.Enemies;
using Unity.Behavior;
using UnityEngine;

namespace Blade.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "IsInSight", story: "[Self] check [Target] in detect range", category: "Conditions", id: "2c8dc5293f31a8111671c54819c382aa")]
    public partial class IsInSightCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        public override bool IsTrue()
        {
            float distance = Vector3.Distance(Self.Value.transform.position, Target.Value.position);
            return distance < Self.Value.detectRange;
        }

    }
}

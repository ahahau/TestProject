using Code.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetInSight", story: "[Enemy] check target in sight [CheckValue]", category: "Conditions", id: "32e662a74c40985c8f90aa0061461f1c")]
public partial class TargetInSightCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;
    [SerializeReference] public BlackboardVariable<bool> CheckValue;

    public override bool IsTrue()
    {
        bool isTargetInRange = Enemy.Value.CheckPlayerInRange();
        Debug.Log(isTargetInRange);
        return isTargetInRange == CheckValue.Value;
    }
}

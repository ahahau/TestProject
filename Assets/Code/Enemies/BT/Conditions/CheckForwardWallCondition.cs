using Code.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckForwardWall", story: "[Forward] wall is [CheckValue]", category: "Conditions", id: "46fe4916cba4c7522d3369b838b1a768")]
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

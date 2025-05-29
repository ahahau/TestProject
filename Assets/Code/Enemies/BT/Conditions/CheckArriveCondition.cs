using System;
using Code.PathFinding;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckArrive", story: "[Agent] arrive status is [CheckValue]", category: "Enemy/Conditions", id: "809d32ccfcfe644b3dac7164dc973c0e")]
    public partial class CheckArriveCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<NavAgent2D> Agent;
        [SerializeReference] public BlackboardVariable<bool> CheckValue;

        public override bool IsTrue()
        {
            return Agent.Value.IsArrived == CheckValue.Value;
        }

    }
}

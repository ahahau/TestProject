using System;
using Code.PathFinding;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "IsInOffMesh", story: "[Agent] is in OffMesh [CheckValue]", category: "Enemy/Conditions", id: "2de2a665951417192207f87f6e423a63")]
    public partial class IsInOffMeshCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<NavAgent2D> Agent;
        [SerializeReference] public BlackboardVariable<bool> CheckValue;

        public override bool IsTrue()
        {
            return Agent.Value.OnOffMeshLink == CheckValue.Value;
        }
    }
}

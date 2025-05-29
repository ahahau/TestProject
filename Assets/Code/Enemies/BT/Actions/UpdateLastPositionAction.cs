using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "UpdateLastPosition", story: "[Self] update [LastTargetPosition] from [Target]", category: "Enemy/Action", id: "575c7663907544684b0b94832399fd46")]
    public partial class UpdateLastPositionAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<Vector3> LastTargetPosition;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            if (Self.Value.CheckChaseTargetInRange(Target.Value))
            {
                LastTargetPosition.Value = Target.Value.position;
            }
            return Status.Success;

        }

    }
}


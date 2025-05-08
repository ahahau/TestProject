using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FlipRenderer", story: "Flip [Renderer]", category: "Enemy/Action", id: "d50ec794ebec013da89d99022dea8e2e")]
    public partial class FlipRendererAction : Action
    {
        [SerializeReference] public BlackboardVariable<EnemyRenderer> Renderer;

        protected override Status OnStart()
        {
            Renderer.Value.Flip();
            return Status.Success;
        }
    }
}


using System;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeClip", story: "change to [newClip] from [oldClip]", category: "Goblin/Action", id: "9e8ac2278573e81b6f068e4b399f8370")]
    public partial class ChangeClipAction : Action
    {
        [SerializeReference] public BlackboardVariable<int> NewClip;
        [SerializeReference] public BlackboardVariable<int> OldClip;
        [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;

        protected override Status OnStart()
        {
            Renderer.Value.SetParam(OldClip.Value, false);
            Renderer.Value.SetParam(NewClip.Value, true);
            OldClip.Value = NewClip.Value;
            return Status.Success;
        }
    }
}


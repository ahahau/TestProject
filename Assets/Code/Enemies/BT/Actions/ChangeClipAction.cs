using Code.Enemies;
using Code.Enemies.BT;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChangeClip", story: "Change [newClip] in [renderer]", category: "Enemy/Action", id: "8fecbef111e43ba27274e0acc3aa6984")]
public partial class ChangeClipAction : Action
{
    [SerializeReference] public BlackboardVariable<AnimatorEnum> NewClip;
    [SerializeReference] public BlackboardVariable<EnemyRenderer> Renderer;

    protected override Status OnStart()
    {
        Renderer.Value.ChangeClip(NewClip.Value);
        return Status.Success;
    }

}


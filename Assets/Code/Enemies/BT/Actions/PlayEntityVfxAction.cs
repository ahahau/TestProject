using Code.Effect;
using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayEntityVFX", story: "[VfxCompo] play [Param]", category: "Enemy/Action", id: "6582b71a4a3c89ab6472bb1adfbd6f0d")]
public partial class PlayEntityVfxAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityVFX> VfxCompo;
    [SerializeReference] public BlackboardVariable<AnimParamSO> Param;

    protected override Status OnStart()
    {
        VfxCompo.Value.PlayVFX(Param.Value.hashValue);
        return Status.Success;
    }
}


using Code.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackToTarget", story: "[Self] attack to [Target]", category: "Action", id: "5dca398ed83b4a91d9e5a16a0b1488b1")]
public partial class AttackToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    private EnemyAttackCompo _attackCompo;
    
    protected override Status OnStart()
    {
        if (_attackCompo == null)
        {
            _attackCompo = Self.Value.GetCompo<EnemyAttackCompo>();
        }

        Vector3 direction = Target.Value.position - Self.Value.transform.position;
        float xDirection = Mathf.Sign(direction.x);
        
        _attackCompo.Attack(xDirection);
        return Status.Success;
    }
}


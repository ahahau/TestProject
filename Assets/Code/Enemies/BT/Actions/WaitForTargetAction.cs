using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForTarget", story: "[Self] wait for reappear in [Sec]", category: "Enemy/Action", id: "1d4ca95c8c9615f359439ec277d71f74")]
    public partial class WaitForTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<float> Sec;

        private float _waitStartTime;

        protected override Status OnStart()
        {
            _waitStartTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_waitStartTime + Sec.Value < Time.time)
            {
                return Status.Failure; //시간이 지나도록 타겟이 나오지 않았다.
            }

            if (Self.Value.CheckChaseTargetInRange(Target.Value))
            {
                return Status.Success; //시간 내에 플레이어가 시야범위 안에 나타났다.
            }

            return Status.Running; //그렇지 않으면 다음 프레임에 보자.
        }

    }
}


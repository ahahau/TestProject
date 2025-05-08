using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Code.Enemies.BT.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChannel", message: "Change to [NewState]", category: "Events", id: "051963ec9a2628cbb155582f1391dbde")]
    public partial class StateChannel : EventChannelBase
    {
        public delegate void StateChannelEventHandler(EnemyState NewState);
        public event StateChannelEventHandler Event; 

        public void SendEventMessage(EnemyState NewState)
        {
            Event?.Invoke(NewState);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<EnemyState> NewStateBlackboardVariable = messageData[0] as BlackboardVariable<EnemyState>;
            var NewState = NewStateBlackboardVariable != null ? NewStateBlackboardVariable.Value : default(EnemyState);

            Event?.Invoke(NewState);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            StateChannelEventHandler del = (NewState) =>
            {
                BlackboardVariable<EnemyState> var0 = vars[0] as BlackboardVariable<EnemyState>;
                if(var0 != null)
                    var0.Value = NewState;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as StateChannelEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as StateChannelEventHandler;
        }
    }
}


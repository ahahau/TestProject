using System;
using Blade.Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;


namespace Blade.BT.Events
{
    #if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChange")]
    #endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChange", message: "change state to [newValue]", category: "Events", id: "fc1e307aedd5a58285bf32e3295d8200")]
    public partial class StateChange : EventChannelBase
    {
        public delegate void StateChangeEventHandler(EnemyState newValue);
        public event StateChangeEventHandler Event; 

        public void SendEventMessage(EnemyState newValue)
        {
            Event?.Invoke(newValue);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<EnemyState> newValueBlackboardVariable = messageData[0] as BlackboardVariable<EnemyState>;
            var newValue = newValueBlackboardVariable != null ? newValueBlackboardVariable.Value : default(EnemyState);

            Event?.Invoke(newValue);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            StateChangeEventHandler del = (newValue) =>
            {
                BlackboardVariable<EnemyState> var0 = vars[0] as BlackboardVariable<EnemyState>;
                if(var0 != null)
                    var0.Value = newValue;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as StateChangeEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as StateChangeEventHandler;
        }
    }
}


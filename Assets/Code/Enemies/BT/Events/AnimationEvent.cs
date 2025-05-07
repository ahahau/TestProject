using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Code.Enemies.BT.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationEvent")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "AnimationEvent", message: "Send [newAnimation]", category: "Events", id: "45cfa80843ed7a9e3f8fb5834d5ae20b")]
    public partial class AnimationEvent : EventChannelBase
    {
        public delegate void AnimationEventEventHandler(int newAnimation);
        public event AnimationEventEventHandler Event; 

        public void SendEventMessage(int newAnimation)
        {
            Event?.Invoke(newAnimation);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<int> newAnimationBlackboardVariable = messageData[0] as BlackboardVariable<int>;
            var newAnimation = newAnimationBlackboardVariable != null ? newAnimationBlackboardVariable.Value : default(int);

            Event?.Invoke(newAnimation);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            AnimationEventEventHandler del = (newAnimation) =>
            {
                BlackboardVariable<int> var0 = vars[0] as BlackboardVariable<int>;
                if(var0 != null)
                    var0.Value = newAnimation;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as AnimationEventEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as AnimationEventEventHandler;
        }
    }
}


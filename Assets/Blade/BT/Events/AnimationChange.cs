using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Blade.BT.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationChange")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "AnimationChange", message: "change to [newAnimation]", category: "Events", id: "dc949436f5a6425bb3b04fd6517e96f5")]
    public partial class AnimationChange : EventChannelBase
    {
        public delegate void AnimationChangeEventHandler(string newAnimation);
        public event AnimationChangeEventHandler Event; 

        public void SendEventMessage(string newAnimation)
        {
            Event?.Invoke(newAnimation);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<string> newAnimationBlackboardVariable = messageData[0] as BlackboardVariable<string>;
            var newAnimation = newAnimationBlackboardVariable != null ? newAnimationBlackboardVariable.Value : default(string);

            Event?.Invoke(newAnimation);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            AnimationChangeEventHandler del = (newAnimation) =>
            {
                BlackboardVariable<string> var0 = vars[0] as BlackboardVariable<string>;
                if(var0 != null)
                    var0.Value = newAnimation;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as AnimationChangeEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as AnimationChangeEventHandler;
        }
    }
}


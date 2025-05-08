using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Code.Enemies.BT.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "AnimationChannel", message: "Change to [newClip]", category: "Events", id: "50e02803806e467ae899f8c81f11c494")]
    public partial class AnimationChannel : EventChannelBase
    {
        public delegate void AnimationChannelEventHandler(AnimatorEnum newClip);
        public event AnimationChannelEventHandler Event; 

        public void SendEventMessage(AnimatorEnum newClip)
        {
            Event?.Invoke(newClip);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<AnimatorEnum> newClipBlackboardVariable = messageData[0] as BlackboardVariable<AnimatorEnum>;
            var newClip = newClipBlackboardVariable != null ? newClipBlackboardVariable.Value : default(AnimatorEnum);

            Event?.Invoke(newClip);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            AnimationChannelEventHandler del = (newClip) =>
            {
                BlackboardVariable<AnimatorEnum> var0 = vars[0] as BlackboardVariable<AnimatorEnum>;
                if(var0 != null)
                    var0.Value = newClip;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as AnimationChannelEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as AnimationChannelEventHandler;
        }
    }
}


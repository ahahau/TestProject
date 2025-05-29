using System;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies.BT;
using Code.Entities;
using UnityEngine;

namespace Code.Enemies
{
    public class EnemyRenderer : EntityRenderer
    {
        [SerializeField] private AnimParamSO[] paramList;
        
        private AnimParamSO _currentClip;
        private Dictionary<AnimatorEnum, AnimParamSO> _paramDictionary;
        private Dictionary<string, AnimationClip> _clipDictionary;
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _paramDictionary = paramList
                .Where(param => Enum.TryParse(param.paramName, out AnimatorEnum _))
                .ToDictionary(param => (AnimatorEnum)Enum.Parse(typeof(AnimatorEnum), param.paramName),
                                param => param);

            _clipDictionary = animator.runtimeAnimatorController.animationClips
                .ToDictionary(clip => clip.name, clip => clip);
        }

        public AnimationClip GetClip(AnimationClip clip)
        {
            return _clipDictionary.GetValueOrDefault(clip.name); //지정된 클립을 가져온다.
        }
        
        public void ChangeClip(AnimatorEnum newClip)
        {
            if(_currentClip != null)
                SetParam(_currentClip, false);
            _currentClip = _paramDictionary.GetValueOrDefault(newClip);
            Debug.Assert(_currentClip != null, $"No clip found for {newClip}");
            SetParam(_currentClip, true);
        }
    }
}
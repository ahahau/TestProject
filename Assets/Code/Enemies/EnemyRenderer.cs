using System;
using System.Collections.Generic;
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

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _paramDictionary = new Dictionary<AnimatorEnum, AnimParamSO>();
            foreach (AnimParamSO param in paramList)
            {
                //param.paramName 에서 AnimatorEnum으로 변환
                if (AnimatorEnum.TryParse(param.paramName, out AnimatorEnum animatorEnum))
                {
                    _paramDictionary.Add(animatorEnum, param);
                }
                else
                {
                    Debug.LogWarning($"No enum found for {param.paramName}");
                }
            }
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
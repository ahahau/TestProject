using System;
using System.Collections.Generic;
using Code.Entities;
using UnityEngine;

namespace Code.Enemies.BT
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
                if (AnimatorEnum.TryParse(param.paramName, out AnimatorEnum animatorEnum))
                {
                    _paramDictionary.Add(animatorEnum, param);
                }
                else
                {
                    Debug.LogWarning($"{param.paramName} is not a valid animator parameter");
                }
            }
        }

        public void ChangeClip(AnimatorEnum newClip)
        {
            if(_currentClip != null)
                SetParam(_currentClip, false);
            _currentClip = _paramDictionary.GetValueOrDefault(newClip);
            Debug.Assert(_currentClip != null, $"{newClip} is not a valid animator parameter");
            SetParam(_currentClip, true);
        }
    }
}
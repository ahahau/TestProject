using System;
using Code.Player;
using UnityEngine;

namespace Code.Entities
{
    public class EntityRenderer : MonoBehaviour, IEntityComponent
    {
        public float FacingDirection { get; private set; } = 1f;

        [SerializeField] private Animator animator;
        
        protected Entity _entity;
        
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void SetParam(AnimParamSO param, bool value) => animator.SetBool(param.hashValue, value);
        public void SetParam(AnimParamSO param, float value) => animator.SetFloat(param.hashValue, value);
        public void SetParam(AnimParamSO param, int value) => animator.SetInteger(param.hashValue, value);
        public void SetParam(AnimParamSO param) => animator.SetTrigger(param.hashValue);
        
        public void SetParam(int param, bool value) => animator.SetBool(param, value);
        public void SetParam(int param, float value) => animator.SetFloat(param, value);
        public void SetParam(int param, int value) => animator.SetInteger(param, value);
        public void SetParam(int param) => animator.SetTrigger(param);

        #region FlipController

        public void Flip()
        {
            FacingDirection *= -1;
            _entity.transform.Rotate(0, 180f, 0);
        }

        public void FlipController(float xMove)
        {
            if (Mathf.Abs(FacingDirection + xMove) < 0.5f)
                Flip();
        }

        #endregion

        
    }
}
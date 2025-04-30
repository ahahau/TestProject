using System;
using Blade.Combat;
using Blade.Entities;
using UnityEngine;

namespace Blade.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        
        [Header("attack datas"), SerializeField] private AttackDataSO[] attackDataList;
        
        [SerializeField] private float comboWindow;
        private Entity _entity;
        private EntityAnimator _entityAnimator;
        private EntityVFX _vfxCompo;
        private EntityAnimatorTrigger _animatorTrigger;

        private readonly int _attackSpeedHash = Animator.StringToHash("ATTACK_SPEED");
        private readonly int _comboCounterHash = Animator.StringToHash("COMBO_COUNTER");

        private float _attackSpeed = 1f;
        private float _lastAttackTime;

        public bool useMouseDirection = false;
        //해당 값이 true이면 마우스 방향으로 공격이 나가고, false이면 바라보는 방향으로 공격이 나가도록 만드세요.
        // 3명까지 가산점
        public int ComboCounter { get; set; } = 0;

        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                _attackSpeed = value;
                _entityAnimator.SetParam(_attackSpeedHash, _attackSpeed);
            }
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _entityAnimator = entity.GetCompo<EntityAnimator>();
            _vfxCompo = entity.GetCompo<EntityVFX>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();  
            AttackSpeed = 1f;
            _animatorTrigger.OnAttackVFXTrigger += HandleAttackVFXTrigger; 
        }

        private void OnDestroy()
        {
            _animatorTrigger.OnAttackVFXTrigger -= HandleAttackVFXTrigger; 
        }

        private void HandleAttackVFXTrigger() 
        {
            _vfxCompo.PlayVfx($"Blade{ComboCounter}", Vector3.zero, Quaternion.identity);
        }

        public void Attack()
        {
            bool comboCounterOver = ComboCounter > 2;
            bool comboWindowExhaust = Time.time >= _lastAttackTime + comboWindow;
            if (comboCounterOver || comboWindowExhaust)
            {
                ComboCounter = 0;
            }
            _entityAnimator.SetParam(_comboCounterHash, ComboCounter);
            
        }

        public void EndAttack()
        {
            ComboCounter++;
            _lastAttackTime = Time.time;
        }

        public AttackDataSO GetCurrentAttackData()
        {
            Debug.Assert(attackDataList.Length > ComboCounter, "Combo counter is out of range");
            return attackDataList[ComboCounter];
        }
    }
}
using Code.Entities;
using UnityEngine;

namespace Code.Player
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float _atkCooldown;
        [SerializeField] private AnimParamSO attackTrigger;
        [SerializeField] private Animator attackAnimator;

        private PlayerController _player;
        private PlayerAnimatorTrigger _trigger;
        private float _lastAttackTime;
        
        public void Initialize(Entity entity)
        {
            _player = entity as PlayerController;
            _trigger = entity.GetCompo<PlayerAnimatorTrigger>();
        }

        //공격 시도
        public bool AttemptAttack()
        {
            const string attackStateName = "ATTACK";
            if (_player.CurrentStateName.Equals(attackStateName)) return false; //이미 공격중
            if (_lastAttackTime + _atkCooldown > Time.time) return false; //공격 쿨타임
            
            _trigger.OnAnimationEnd += HandleAnimationEnd;
            Attack();
            return true;
        }

        private void HandleAnimationEnd()
        {
            _lastAttackTime = Time.time;
            _trigger.OnAnimationEnd -= HandleAnimationEnd;
            _player.AnimationEndTrigger(); //구독 빼주고 끝났음을 알려줌.
        }

        private void Attack()
        {
            attackAnimator.SetTrigger(attackTrigger.hashValue);
            Debug.Log("<color=red>Attack!!</color>");
        }
    }
}
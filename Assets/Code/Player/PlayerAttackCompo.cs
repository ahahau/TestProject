using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.Player
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float _atkCooldown;
        [SerializeField] private AnimParamSO attackTrigger;
        [SerializeField] private Animator attackAnimator;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private AttackDataSO attackData;

        private PlayerController _player;
        private PlayerAnimatorTrigger _trigger;
        private float _lastAttackTime;
        
        public void Initialize(Entity entity)
        {
            _player = entity as PlayerController;
            _trigger = entity.GetCompo<PlayerAnimatorTrigger>();
            damageCaster.InitCaster(entity);
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

        private void CastAttack()
        {
            Vector3 direction = transform.position - _player.transform.position;
            Vector2 knockBack = attackData.knockBackPower;
            knockBack.x *= Mathf.Sign(direction.x);
            damageCaster.CastDamage(attackData.damage, direction, knockBack);
        }
    }
}
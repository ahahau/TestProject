using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        private EntityMover _mover;
        private EnemyRenderer _renderer;
        [SerializeField] private AttackDataSO attackData;
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _mover = entity.GetCompo<EntityMover>();
            _renderer = entity.GetCompo<EnemyRenderer>();
        }

        public async void Attack(float direction)
        {
            _renderer.FlipController(direction); //공격방향으로 회전후에

            AnimationClip clip = _renderer.GetClip(attackData.attackClip);
            Debug.Assert(clip != null, "Attack clip is null");

            _mover.CanManualMove = false;
            Vector2 movement = attackData.movement;
            movement.x *= direction; //공격방향을 설정해준다.
            float timer = 0;
            while (timer <= clip.length)
            {
                timer += Time.deltaTime;
                float percent = Mathf.Clamp01(timer / clip.length);
                float curveValue = attackData.movementCurve.Evaluate(percent);

                Vector3 position = (Vector2)_entity.transform.position + movement * (Time.deltaTime * curveValue);
                _mover.ManualMove(position);
                await Awaitable.NextFrameAsync();
            }

            _mover.CanManualMove = true;
        }
    }
}
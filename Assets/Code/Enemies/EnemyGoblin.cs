using UnityEngine;

namespace Code.Enemies
{
    public class EnemyGoblin : Enemy
    {
        [SerializeField] private float detectDistance = 7f;
        [SerializeField] private float detectAngle = 90f;
        [SerializeField] private float attackDistance = 2f;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 startPosition = transform.position + new Vector3(0, bodyOffset); 
            Gizmos.DrawLine(startPosition, startPosition + transform.right * detectDistance);
        }

        public override bool CheckPlayerInRange()
        {
            Vector3 position = transform.position + new Vector3(0, bodyOffset); //몸통의 중심점에서 측정시작.
            Collider2D collider = Physics2D.OverlapCircle(position, detectDistance, whatIsPlayer); //거리내에 플레이어가 존재하는가?

            if (collider == null) return false;
            
            //여기서 나중에 플레이어와 나 사이에 장애물이 있는지도 체크해야해.

            Vector3 direction = collider.transform.position - position; //플레이어로의 방향을 구한다.
            float angle = Vector3.Angle(direction.normalized, transform.right);

            bool isInAngle = angle < detectAngle * 0.5f;
            if(isInAngle && Target != collider.transform)
                Target = collider.transform; //타겟을 플레이어로 설정.
            
            return isInAngle;
        }

        public override bool IsTargetInAttackRange()
        {
            if (Target == null) return false;

            Vector3 distance = Target.position - transform.position;
            
            //높이는 0.5 이내이면서 가로로 공격사거리에 들어왔다면 가능하다고 return 해준다.
            const float heightLimit = 0.5f;
            return Mathf.Abs(distance.y) < heightLimit && Mathf.Abs(distance.x) < attackDistance;
        }
    }
}
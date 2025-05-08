using System;
using UnityEngine;

namespace Code.Enemies
{
    public class EnemyGoblin : Enemy
    {
        [SerializeField]private float detectDistance = 7f;
        [SerializeField] private float detectAngle = 90f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 startPosition = transform.position + new Vector3(0, bodyOffset);
            Gizmos.DrawLine(startPosition, startPosition + transform.right * detectDistance);
        }

        public override bool CheckPlayerInRange()
        {
            Vector3 position = transform.position + new Vector3(0, bodyOffset);
            Collider2D collider = Physics2D.OverlapCircle(position, detectDistance, whatIsPlayer);
            
            if(collider == null)
                return false;
            
            Debug.Log(collider.gameObject.name);
            Vector3 direction = collider.transform.position - position;
            float angle = Vector3.Angle(direction.normalized, transform.right);
            return angle < detectAngle * 0.5f;
        }
    }
}
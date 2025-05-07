using System;
using UnityEngine;

namespace Code.Enemies
{
    public class EnemyGoblin : Enemy
    {
        [SerializeField]private float detectDistance = 7f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 startPosition = transform.position + new Vector3(0, bodyOffset);
            Gizmos.DrawLine(startPosition, startPosition + transform.right * detectDistance);
        }
    }
}
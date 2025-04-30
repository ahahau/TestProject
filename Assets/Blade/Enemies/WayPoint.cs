using System;
using UnityEditor;
using UnityEngine;

namespace Blade.Enemies
{
    public class WayPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}
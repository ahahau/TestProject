using System;
using UnityEditor;
using UnityEngine;

namespace Blade.Test
{
    public class RayCasterTest : MonoBehaviour
    {
        [SerializeField] private float maxDistance = 8f;
        [SerializeField] private LayerMask whatIsEnemy;
        
        private void OnDrawGizmos()
        {
            

            RaycastHit hit;
            
            // bool isHit = Physics.Raycast(
            //     transform.position,
            //     transform.forward, 
            //     out hit, 
            //     maxDistance, whatIsEnemy);

            // bool isHit = Physics.BoxCast(transform.position, transform.lossyScale * 0.5f,
            //     transform.forward, out hit, transform.rotation, maxDistance);

            bool isHit = Physics.SphereCast(
                transform.position, transform.lossyScale.x * 0.5f, transform.forward,
                out hit, maxDistance);

            if (isHit)
            {
                Gizmos.color = Color.red;
                //Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                //Gizmos.DrawWireCube(transform.position+ transform.forward* hit.distance, transform.lossyScale);
                Gizmos.DrawWireSphere(
                    transform.position + transform.forward * hit.distance, transform.lossyScale.x * 0.5f);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * maxDistance);   
            }
            
        }
    }
}
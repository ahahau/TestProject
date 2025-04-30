using System;
using System.Collections.Generic;
using Blade.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Blade.Enemies
{
    public class NavMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float stopOffset = 0.05f; //거리에 대한 오프셋
        
        private Entity _entity;
        private const float RotateSpeed = 10f;
        public bool IsArrived => !agent.pathPending && agent.remainingDistance < agent.stoppingDistance + stopOffset;
        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            agent.speed = moveSpeed;
        }

        private void Update()
        {
            if (agent.hasPath && agent.isStopped == false && agent.path.corners.Length > 0)
            {
                LookAtTarget(agent.steeringTarget);
            }
        }

        public void LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = target - _entity.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            if (isSmooth)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation, 
                                                lookRotation, Time.deltaTime * RotateSpeed);
            }
            else
            {
                _entity.transform.rotation = lookRotation;
            }
        }

        public void SetStop(bool isStop) => agent.isStopped = isStop;
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity; 
        public void SetSpeed(float speed) => agent.speed = speed;
        public void SetDestination(Vector3 destination) => agent.SetDestination(destination);
    }
}
using Unity.Behavior;
using UnityEngine;

namespace Enemy
{
        public class EnemySkeleton : Enemy
        {
            [SerializeField] private Vector3[] positions;

            private int _currentIdx = 0;
            private NavMovement _navMovement;

            protected override void Awake()
            {
                base.Awake();
                _navMovement = GetCompo<NavMovement>();
            }

            private void Start()
            {
                _navMovement.SetDestination(positions[_currentIdx]);
            }

            private void Update()
            {
                if (_navMovement.IsArrived)
                {
                    _currentIdx = (_currentIdx + 1) % positions.Length;
                    _navMovement.SetDestination(positions[_currentIdx]);
                }
            }
        }

}
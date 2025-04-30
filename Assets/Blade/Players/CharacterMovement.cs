using Blade.Entities;
using UnityEngine;

namespace Blade.Players
{
    public class CharacterMovement : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float moveSpeed = 8f, gravity = -9.81f;
        [SerializeField] private float rotationSpeed = 8f;
        [SerializeField] private CharacterController characterController;

        public bool CanManualMovement { get; set; } = true;
        private Vector3 _autoMovement;
        public bool IsGround => characterController.isGrounded;

        private Vector3 _velocity;
        public Vector3 Velocity => _velocity;
        
        private float _verticalVelocity;
        private Vector3 _movementDirection;

        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            _movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized; 
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            Move();
        }

        private void CalculateMovement()
        {
            if (CanManualMovement)
            {
                _velocity = Quaternion.Euler(0, -45f, 0) * _movementDirection;
                _velocity *= moveSpeed * Time.fixedDeltaTime;    
            }
            else
            {
                _velocity = _autoMovement * Time.fixedDeltaTime;
            }
            
            if (_velocity.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_velocity);
                Transform parent = _entity.transform;
                parent.rotation = Quaternion.Lerp(parent.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }
        }
        
        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity < 0)
                _verticalVelocity = -0.03f; //살짝 아래로 당겨주는 힘
            else
                _verticalVelocity += gravity * Time.fixedDeltaTime;
            
            _velocity.y = _verticalVelocity;
        }
        
        private void Move()
        {
            characterController.Move(_velocity);
        }
        
        public void StopImmediately()
        {
            _movementDirection = Vector3.zero;
        }

        public void SetAutoMovement(Vector3 autoMovement) => _autoMovement = autoMovement;
        
    }
}
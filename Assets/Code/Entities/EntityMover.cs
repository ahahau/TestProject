using System;
using Code.Player;
using UnityEngine;

namespace Code.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {
        [Header("Player data")] 
        [SerializeField] private float moveSpeed = 5f;
        

        [Header("Ground check")] 
        [SerializeField] private Transform groundTrm;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private Vector2 groundCheckSize;

        
        private Rigidbody2D _rbCompo;
        private Entity _entity;
        private EntityRenderer _renderer;
        private float _movementX;
        public bool IsGrounded { get; private set; }
        public event Action<bool> OnGroundStatusChange;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _renderer = entity.GetCompo<EntityRenderer>();
            _rbCompo = entity.GetComponent<Rigidbody2D>();
        }

        public void AddForceToEntity(Vector2 force)
        {
            _rbCompo.AddForce(force, ForceMode2D.Impulse);
        }

        public void StopImmediately(bool isYAxis = false)
        {
            if (isYAxis)
                _rbCompo.linearVelocity = Vector2.zero;
            else
                _rbCompo.linearVelocityX = 0;
            _movementX = 0;
        }
        
        public void SetMovementX(float movementX) => _movementX = movementX;
        
        private void FixedUpdate()
        {
            CheckGround();
            MoveCharacter();
        }

        private void CheckGround()
        {
            bool before = IsGrounded;
            IsGrounded = Physics2D.OverlapBox(groundTrm.position, groundCheckSize, 0, whatIsGround);
            if (IsGrounded != before)
                OnGroundStatusChange?.Invoke(IsGrounded);
        }

        private void MoveCharacter()
        {
            _renderer.FlipController(_movementX);
            _rbCompo.linearVelocityX = _movementX * moveSpeed;
        }
        
        private void OnDrawGizmos()
        {
            if (groundTrm == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundTrm.position, groundCheckSize);
        }

        
    }
}
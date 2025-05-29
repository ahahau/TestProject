using System;
using Code.Player;
using UnityEngine;

namespace Code.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {
        [Header("Player data")] 
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private AnimParamSO ySpeedParam;
        [SerializeField] private AnimParamSO onGroundParam;
        [SerializeField] private bool updateYSpeedParam = true;

        [Header("Ground check")] 
        [SerializeField] private Transform groundTrm;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private Vector2 groundCheckSize;
        
        private Rigidbody2D _rbCompo;
        private Entity _entity;
        private EntityRenderer _renderer;
        private float _movementX;

        private float _moveSpeedMultiplier;
        private float _originalGravityScale;

        [field:SerializeField] public bool IsGrounded { get; private set; }
        public event Action<bool> OnGroundStatusChange; //그라운드 상태가 변경될 때마다 발행해준다.
        public event Action<Vector2> OnMoveVelocityChange; //이동속도 변경시마다 발행

        public bool CanManualMove { get; set; } = true;
        public Vector2 Velocity => _rbCompo.linearVelocity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _renderer = entity.GetCompo<EntityRenderer>(true);
            _rbCompo = entity.GetComponent<Rigidbody2D>();
            _originalGravityScale = _rbCompo.gravityScale;
            _moveSpeedMultiplier = 1f;
        }
        
        public void SetMoveSpeedMultiplier(float value) => _moveSpeedMultiplier = value;
        public void SetGravityScale(float value) => _rbCompo.gravityScale = _originalGravityScale * value;

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

        public void StopFalling() => _rbCompo.linearVelocityY = 0;
        
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
            if (CanManualMove)
            {
                _renderer.FlipController(_movementX);
                _rbCompo.linearVelocityX = _movementX * moveSpeed * _moveSpeedMultiplier;
            }
            
            if(updateYSpeedParam)
                _renderer.SetParam(ySpeedParam, _rbCompo.linearVelocityY); //현재 움직임의 Y값을 애니메이터에 전달.
            
            OnMoveVelocityChange?.Invoke(_rbCompo.linearVelocity);
        }
        
        public void SendOnGroundParam() => _renderer.SetParam(onGroundParam);
        
        private void OnDrawGizmos()
        {
            if (groundTrm == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundTrm.position, groundCheckSize);
        }

    }
}
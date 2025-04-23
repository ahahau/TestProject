using System;
using Code.Player;
using UnityEngine;

namespace Code.Entities
{
    public class EntityMover : MonoBehaviour
    {
        [Header("Player data")] 
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpPower = 4f;

        [Header("Ground check")] 
        [SerializeField] private Transform groundTrm;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private Vector2 groundCheckSize;

        [SerializeField] private AnimParamSO moveParan;
        
        private PlayerController _player;
        private Rigidbody2D _rbCompo;

        private int _jumpCount = 2; //더블점프 만들어보세요.
        
        public bool IsGrounded { get; private set; }
        
        public void Initialize(PlayerController player)
        {
            _player = player;
            _rbCompo = player.GetComponent<Rigidbody2D>();
            _player.PlayerInput.OnJumpPress += HandleJumpPress;
        }
        
        private void OnDestroy()
        {
            _player.PlayerInput.OnJumpPress -= HandleJumpPress;
        }
        
        private void HandleJumpPress()
        {
            if (IsGrounded == false && _jumpCount <= 0) return; //1번
            
            --_jumpCount;
            _rbCompo.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        
        private void FixedUpdate()
        {
            CheckGround();
            MoveCharacter();
        }

        private void CheckGround()
        {
            bool before = IsGrounded;
            IsGrounded = Physics2D.OverlapBox(groundTrm.position, groundCheckSize, 0, whatIsGround);
            if (IsGrounded && before == false)
                _jumpCount = 2;
        }

        private void MoveCharacter()
        {
            float xMove = _player.PlayerInput.InputDirection.x;
            _rbCompo.linearVelocityX = xMove * moveSpeed;
            _player.Renderer.FlipController(xMove);
            _player.Renderer.SetParam(moveParan, Mathf.Abs(xMove) > 0);
        }
        
        private void OnDrawGizmos()
        {
            if (groundTrm == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundTrm.position, groundCheckSize);
        }
    }
}
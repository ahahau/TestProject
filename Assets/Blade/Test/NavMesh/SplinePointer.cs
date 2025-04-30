using System;
using Blade.Players;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

namespace Blade.Test.NavMesh
{
    public class SplinePointer : MonoBehaviour
    {
        [SerializeField] private Transform playerTrm;
        [SerializeField] private PlayerInputSO playerInput;

        public float bendAmount = 2;

        private Spline _spline;
        private SplineInstantiate _splineInstantiate;
        private BezierKnot _playerKnot, _targetKnot;

        private void Awake()
        {
            _spline = GetComponent<SplineContainer>().Spline;
            _splineInstantiate = GetComponent<SplineInstantiate>();
            
            playerInput.OnAttackPressed += HandleClick;
        }

        private void OnDestroy()
        {
            playerInput.OnAttackPressed -= HandleClick;
        }

        private void HandleClick()
        {
            _splineInstantiate.enabled = false;
            Vector3 worldPos = playerInput.GetWorldPosition();
            FindSelectedTarget(worldPos);
        }

        private void Start()
        {
            _spline.Add(_playerKnot);
            _spline.Add(_targetKnot);
        }

        private void FindSelectedTarget(Vector3 worldPos)
        {
            _playerKnot.Position = playerTrm.position + new Vector3(0, 0.2f); //위로 살짝 올린 곳에 배치
            _targetKnot.Position = worldPos + new Vector3(0, 0.2f);

            _playerKnot.TangentOut = new float3(0, bendAmount, 1f);
            _targetKnot.TangentIn = new float3(0, bendAmount, -1f);
            
            //구조체라 변경이 안되니 대입해야 한다.
            _spline.SetKnot(0, _playerKnot);
            _spline.SetKnot(1, _targetKnot);
            
            _spline.SetTangentMode(0, TangentMode.Mirrored, BezierTangent.Out);
            _spline.SetTangentMode(1, TangentMode.Mirrored, BezierTangent.In);
            _splineInstantiate.enabled = true;
        }
    }
}
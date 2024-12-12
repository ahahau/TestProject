using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    private bool IsGround;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundCheckerTrm;
    [SerializeField] private Vector2 _checkerSize;

    private void Update()
    {
        
    }
    private void CheckGround()
    {
        Vector3 position = _groundCheckerTrm.position;
        Collider2D collider = Physics2D.OverlapBox(position, _checkerSize, 0, _whatIsGround);
        IsGround = collider != null;
    }

    private void OnDrawGizmos()
    {
        if (_groundCheckerTrm == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheckerTrm.position, _checkerSize);
        Gizmos.color = Color.white;
    }

}

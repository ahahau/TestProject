using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Test.Drag
{
    public class DragSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsUnit, whatIsGround;
        private HashSet<TestUnit> _selectedUnits;

        private Vector2 _dragStartUIPosition;
        private Vector3 _dragStartWorldPosition;

        private Collider[] _results; //오버랩 박스를 위한 컬라이더 배열

        private Vector2 _currentMousePosition;

        public Action<bool> OnMouseStatusChange;

        private void Awake()
        {
            _results = new Collider[10];
            _selectedUnits = new HashSet<TestUnit>();

            OnMouseStatusChange += HandleMouseStatusChange;
        }

        private void OnDestroy()
        {
            OnMouseStatusChange -= HandleMouseStatusChange;
        }

        private void Update()
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
                OnMouseStatusChange?.Invoke(true);
            else if(Mouse.current.leftButton.wasReleasedThisFrame)
                OnMouseStatusChange?.Invoke(false);
            
            _currentMousePosition = Mouse.current.position.ReadValue();

            if (_isMultiSelection)
            {
                Vector2 delta = _currentMousePosition - _dragStartUIPosition;
                selectionIcon.SetSize(delta);

                UpdateSelection();
            }
            
        }

        private void UpdateSelection()
        {
            GetRayCastInfo(out RaycastHit hit);
            Vector3 currentWorldPosition = hit.point;
            
            Vector3 boxSize = currentWorldPosition - _dragStartWorldPosition;
            Vector3 center = _dragStartWorldPosition + boxSize * 0.5f;
            Vector3 xAxis = Quaternion.Euler(0, -45f, 0) * Vector3.right;
            Vector3 zAxis = Quaternion.Euler(0, -45f, 0) * Vector3.forward;
            
            float xSize = Mathf.Abs(Vector3.Dot(xAxis, boxSize));
            float zSize = Mathf.Abs(Vector3.Dot(zAxis, boxSize));
            
            int cnt = Physics.OverlapBoxNonAlloc(center, 
                new Vector3(xSize, 10, zSize) * 0.5f, _results,
                Quaternion.Euler(0, -45f, 0), whatIsUnit);

            foreach (var unit in _selectedUnits)
            {
                unit.SetSelected(false);
            }
            _selectedUnits.Clear();

            for (int i = 0; i < cnt; i++)
            {
                Collider target = _results[i];
                if (target.TryGetComponent(out TestUnit unit))
                {
                    _selectedUnits.Add(unit);
                    unit.SetSelected(true);
                }
            }
        }


        private bool GetRayCastInfo(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(_currentMousePosition);
            bool isHit = Physics.Raycast(ray, out hit, Camera.main.farClipPlane, whatIsGround);
            return isHit;
        }
        
        
        [SerializeField] private SelectionIcon selectionIcon;
        private bool _isMultiSelection;
        
        private void HandleMouseStatusChange(bool isMouseDown)
        {
            if (isMouseDown)
            {
                SetMultiSelection(true);
            }
            else
            {
                SetMultiSelection(false);
            }
        }

        private void SetMultiSelection(bool isMultiSelection)
        {
            selectionIcon.SetActive(isMultiSelection);
            _isMultiSelection = isMultiSelection;

            if (isMultiSelection)
            {
                selectionIcon.SetSize(Vector2.one * 0.5f); //시작할때 아이콘 크기 최소로 설정 
                _dragStartUIPosition = _currentMousePosition;
                GetRayCastInfo(out RaycastHit hitInfo);
                
                _dragStartWorldPosition = hitInfo.point;
                
                selectionIcon.SetPosition(_dragStartUIPosition);
            }
        }

        private Vector3 _lastWorldPosition;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                if (_isMultiSelection)
                {
                    GetRayCastInfo(out RaycastHit hitInfo);
                    _lastWorldPosition = hitInfo.point;
                }
                
                Vector3 boxSize = _lastWorldPosition - _dragStartWorldPosition;
                Vector3 center = _dragStartWorldPosition + boxSize * 0.5f; //중앙지점.

                float yAngle = Camera.main.transform.localEulerAngles.y;
                Vector3 xAxis = Quaternion.Euler(0, yAngle, 0) * Vector3.right;
                Vector3 zAxis = Quaternion.Euler(0, yAngle, 0) * Vector3.forward;

                float xSize = Vector3.Dot(xAxis, boxSize);
                float zSize = Vector3.Dot(zAxis, boxSize);

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_dragStartWorldPosition, 0.3f);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_lastWorldPosition, 0.3f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(center, 0.3f);

                Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, yAngle, 0), Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(xSize, 4f, zSize));

                Gizmos.matrix = Matrix4x4.identity;
            }
        }
    }
}
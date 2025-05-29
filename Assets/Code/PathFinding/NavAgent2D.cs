using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Entities;
using DG.Tweening;
using UnityEngine;

namespace Code.PathFinding
{
    public class NavAgent2D : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private BakedDataSO bakedData;
        [SerializeField] private float jumpTime = 0.4f; //1유닛의 높이를 뛰는데 걸리는 시간
        [SerializeField] private bool drawLineDebug;
        [SerializeField] private LineRenderer lineRenderer;
        
        public bool IsPathFinding { get; private set; }
        public bool IsPathFailed { get; private set; }
        public bool IsStopped { get; set; }
        public bool IsArrived { get; private set; }
        public bool OnOffMeshLink { get; private set; }
        public Vector3 Destination { get; private set; }

        private Entity _entity;
        private List<NodeData> _path;
        private List<NodeData> _corners;
        private EntityMover _mover;
        private EntityRenderer _renderer;
        private int _currentPathIndex;
        private NodeData _currentTargetNode;
        
        public List<NodeData> Path => _path;
        public List<NodeData> Corners => _corners;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _mover = entity.GetCompo<EntityMover>();
            _renderer = entity.GetCompo<EntityRenderer>(true); //적은 따로니까 상속받은걸로 가져와
            IsPathFinding = false;
            IsPathFailed = false;
            IsStopped = false;
            IsArrived = true; //처음시작했을 때 도착상태로 둔다.
        }

        public async Task<bool> SetDestination(Vector3 position)
        {
            if (OnOffMeshLink) return true; //점프중일때는 경로 요청 무시

            IsArrived = false;
            Destination = position;
            await FindPath(position);

            if (IsPathFailed == false && _corners.Count > 0)
            {
                _currentTargetNode = _corners[0];
                return true;
            }
            else
            {
                _mover.StopImmediately(); //경로 계산에 실패하면 그 자리에 정지.
                return false;
            }
        }

        private bool CheckArrive()
        {
            const float distanceThreshold = 0.8f;
            // 스프라이트마다 기준점이 다를 수 있으므로 Y좌표는 1정도까지는 차이를 인정해준다. X는 threshold만큼 아래로 떨어져야 도착으로 인정
            bool isXArrived = Mathf.Abs(transform.position.x - _currentTargetNode.worldPosition.x) < distanceThreshold;
            bool isYArrived = Mathf.Abs(transform.position.y - _currentTargetNode.worldPosition.y) < 1f;

            return isXArrived && isYArrived;
        }

        private async Task FindPath(Vector3 destination)
        {
            IsPathFinding = true;
            IsPathFailed = false; 
            
            _path = await bakedData.GetPath(transform.position, destination);
            _corners = new List<NodeData>();
            
            //여기까지왔다면 경로설정은 끝
            IsPathFinding = false;
            IsArrived = false;

            if (_path == null || _path.Count == 0)
            {
                IsPathFailed = true;
                Debug.Log("경로를 찾을 수 없습니다.");
                return;
            }

            _currentPathIndex = 0;
            _corners.Add(_path[0]); //시작점을 코너에 넣어준다.

            for (int i = 1; i < _path.Count - 1; i++)
            {
                if (_path[i].pathLink.linkType != LinkType.Normal)
                {
                    _corners.Add(_path[i]);
                }else if (_path[i - 1].pathLink.linkType != LinkType.Normal)
                {
                    _corners.Add(_path[i]);
                }
            }
            _corners.Add(_path[^1]); //마지막 노드 더하기

            if (drawLineDebug)
            {
                Vector3[] points = _corners.Select(x => x.worldPosition).ToArray();
                lineRenderer.positionCount = points.Length;
                lineRenderer.SetPositions(points);
            }
        }

        private void Update()
        {
            if (IsStopped || IsArrived || OnOffMeshLink || IsPathFailed)
                return;

            if (_currentTargetNode == null) return;

            if (CheckArrive())
            {
                NodeData prevNode = _currentTargetNode;
                _currentTargetNode = GetNextNode();
                if (_currentTargetNode == null) return; //다음으로 가야할 노드가 없는 상태

                if (prevNode.pathLink.linkType == LinkType.Jump)
                {
                    OnOffMeshLink = true;
                    JumpProcess();
                    _mover.StopImmediately(true);
                }else if (prevNode.pathLink.linkType == LinkType.Drop)
                {
                    OnOffMeshLink = true;
                    DropProcess();
                }
            }

            Vector3 direction = _currentTargetNode.worldPosition - transform.position;
            _mover.SetMovementX(Mathf.Sign(direction.x));
        }

        private async void DropProcess()
        {
            _mover.StopImmediately(true);
            _mover.CanManualMove = false;
            _mover.SetGravityScale(0);

            Vector3 start = transform.position;
            Vector3 end = _currentTargetNode.worldPosition - new Vector3(0, 0.5f);
            _renderer.FlipController(Mathf.Sign(end.x - start.x));

            float dropPower = 2f;
            float height = Mathf.Abs(end.y - start.y);
            float realDropTime = jumpTime * height;

            await _entity.transform.DOJump(end, dropPower, 1, realDropTime)
                .SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
            
            _mover.SetGravityScale(1f);
            _mover.CanManualMove = true;
            OnOffMeshLink = false;
            Debug.Log("드랍 종료");
        }

        private async void JumpProcess()
        {
            _mover.StopImmediately(true);
            _mover.CanManualMove = false;
            _mover.SetGravityScale(0);

            Vector3 start = transform.position;
            Vector3 end = _currentTargetNode.worldPosition - new Vector3(0, 0.5f);
            
            _renderer.FlipController(Mathf.Sign(end.x - start.x)); //점프할 방향으로 바라보도록 해준다.

            float height = Mathf.Abs(end.y - start.y);
            float jumpPower = height * 0.7f; //점프해야할 높이에다가 파워값으로 적정수치를 넣어준다.
            float realJumpTime = jumpTime * height; //jump타임은 1높이일때의 시간이니 단순히 곱해주면 된다.(비례식)

            await _entity.transform.DOJump(end, jumpPower, 1, realJumpTime)
                .SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
            
            _mover.SetGravityScale(1f);
            _mover.CanManualMove = true;
            OnOffMeshLink = false;
            Debug.Log("점프 종료");
            
        }

        private NodeData GetNextNode()
        {
            _currentPathIndex++;
            if (_corners.Count <= _currentPathIndex)
            {
                IsArrived = true;
                _mover.StopImmediately();
                return null;
            }

            return _corners[_currentPathIndex];
        }
    }
}
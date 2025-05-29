using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.PathFinding
{
    [CreateAssetMenu(fileName = "BakedData", menuName = "SO/Path/BakedData", order = 0)]
    public class BakedDataSO : ScriptableObject
    {
        public List<NodeData> points = new List<NodeData>();
        private Dictionary<Vector3Int, NodeData> _pointDict;

        private void OnEnable()
        {
            _pointDict = points.ToDictionary(node => node.cellPosition);
        }

        public void ClearPoints()
        {
            points?.Clear();
        }

        public void AddPoint(Vector3 worldPosition, Vector3Int cellPosition, NodeType nodeType)
        {
            points.Add(new NodeData
            {
                worldPosition = worldPosition,
                cellPosition = cellPosition,
                nodeType = nodeType
            });
        }

        public async Task<List<NodeData>> GetPath(Vector3 startPosition, Vector3 destination)
        {
            (List<NodeData> list, bool success) = await Task.Run(() => CalulatePath(startPosition, destination));

            if (success)
                return list;
            
            return null;
        }

        private (List<NodeData>, bool) CalulatePath(Vector3 startPosition, Vector3 destination)
        {
            PriorityQueue<AstarNode> openList = new PriorityQueue<AstarNode>(); //방문가능한 노드들을 넣는 곳
            List<AstarNode> closeList = new List<AstarNode>(); // 방문한 노드들을 넣는 곳
            List<NodeData> path = new List<NodeData>(); //이건 완성된 경로를 저장하는 리스트야

            NodeData startNode = FindClosetGroundNode(startPosition);
            NodeData destinationNode = FindClosetGroundNode(destination);

            if (destination == null || startPosition == null)
                return (path, false);
            
            openList.Push(new AstarNode{nodeData = startNode, parent = null, G = 0, F = CalH(startNode, destinationNode)});
            
            bool result = false;
            while (openList.Count > 0)
            {
                AstarNode currentNode = openList.Pop();
                foreach (LinkData linkData in currentNode.nodeData.neighbors)
                {
                    AstarNode isVisited = closeList.Find(n => n.nodeData.cellPosition == linkData.endCellPosition);
                    if(isVisited != null) continue;
                    float newG = linkData.cost + currentNode.G;
                    NodeData nextNode = _pointDict[linkData.endCellPosition];

                    AstarNode nextOpenNode = new AstarNode
                    {
                        nodeData = nextNode, parent = currentNode, parentLinkData = linkData,
                        G = newG, F = newG + CalH(nextNode, destinationNode)
                    };
                    AstarNode exist = openList.Contain(nextOpenNode);
                    if (exist != null)
                    {
                        if(nextOpenNode.G < exist.G)
                        {
                            exist.G = nextOpenNode.G;
                            exist.F = nextOpenNode.F;
                            exist.parent = nextOpenNode.parent;
                            exist.parentLinkData = nextOpenNode.parentLinkData;
                        }
                    }
                    else
                    {
                        openList.Push(nextOpenNode);
                    }
                }
                closeList.Add(currentNode);
                if (currentNode.nodeData == destinationNode)
                {
                    result = true;
                    break;
                }
            }

            if (result)
            {
                AstarNode last = closeList[^1];
                while (last.parent != null)
                {
                    AstarNode prevNode = last;
                    path.Add(last.nodeData);
                    last = last.parent;
                    last.nodeData.pathLink = prevNode.parentLinkData;
                }
                
                path.Add(last.nodeData);
                path.Reverse();
            }
            return (path, result);
        }

        private float CalH(NodeData startNode, NodeData destinationNode) => Vector3Int.Distance(destinationNode.cellPosition, startNode.cellPosition);

        private NodeData FindClosetGroundNode(Vector3 startPosition)
        {
            NodeData closetNode = null;
            const float distanceThreshold = 8f; //타일이 8개 이상 벗어났으면 버려라.
            float closestDistance = distanceThreshold;
            
            foreach (NodeData node in points)
            {
                if(node.nodeType == NodeType.Drop)
                    continue;

                float distance = Vector3.Distance(startPosition, node.worldPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closetNode = node;
                }
            }

            return closetNode;
        }
    }
}
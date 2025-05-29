#if  UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.PathFinding
{
    public class PathNodeManager : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private AgentDataSO agentData;
        [SerializeField] private Tilemap targetTilemap;
        [SerializeField] private bool canIncludeOutline = true; //외곽선도 포함할꺼냐?
        [SerializeField] private BakedDataSO bakedData;

        [SerializeField] private Color dropColor, groundColor, jumpColor, edgeColor;
        
#if UNITY_EDITOR
        [ContextMenu("Bake map data")]
        private void BakeMapData()
        {
            Debug.Assert(targetTilemap != null, "targetTilemap is null");

            WritePointData();
            RecordNeighbors();
            EditorUtility.SetDirty(bakedData);
            AssetDatabase.SaveAssets();
        }

        [ContextMenu("Clear bake map data")]
        private void ClearBakeMapData()
        {
            bakedData?.ClearPoints();
        }

        private void WritePointData()
        {
            targetTilemap.CompressBounds(); //바운드를 압축해준다.
            BoundsInt bounds = targetTilemap.cellBounds; //타일맵의 범위가 된다.
            if (canIncludeOutline) //아웃라인 체크되어있다면 
            {
                bounds.xMin -= 1;
                bounds.xMin += 1;
                bounds.yMin -= 1;
                bounds.yMax += 1;
            }
            
            bakedData.ClearPoints();
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0); //타일맵 좌표를 구한다.
                    if (targetTilemap.HasTile(cellPosition) == false &&
                        targetTilemap.HasTile(cellPosition + Vector3Int.down))
                    {
                        Vector3 worldPosition = targetTilemap.GetCellCenterWorld(cellPosition);
                        bakedData.AddPoint(worldPosition, cellPosition, NodeType.Ground);
                    }
                }
            }
            
            foreach (NodeData point in bakedData.points.ToList())
            {
                Vector3Int leftCell = point.cellPosition + Vector3Int.left;
                Vector3Int rightCell = point.cellPosition + Vector3Int.right;

                bool hasBottomLeft = targetTilemap.HasTile(leftCell + Vector3Int.down);
                bool hasBottomRight = targetTilemap.HasTile(rightCell + Vector3Int.down);

                if (!hasBottomLeft)
                {
                    Vector3 worldPosition = targetTilemap.GetCellCenterWorld(leftCell);
                    bakedData.AddPoint(worldPosition, leftCell, NodeType.Drop);
                }
                
                if (!hasBottomRight)
                {
                    Vector3 worldPosition = targetTilemap.GetCellCenterWorld(rightCell);
                    bakedData.AddPoint(worldPosition, rightCell, NodeType.Drop);
                }

                if (!hasBottomLeft || !hasBottomRight)
                {
                    point.nodeType = NodeType.Edge;
                }
            }
        }
        
        private void RecordNeighbors()
        {
            Dictionary<Vector3Int, NodeData> pointDict = bakedData.points.ToDictionary(point => point.cellPosition);
            
            //드랍노드가 아닌 노드들만 찾아서 작업한다.
            bakedData.points.Where(point => point.nodeType != NodeType.Drop).ToList().ForEach(point =>
            {
                point.neighbors.Clear();

                Vector3Int leftCell = point.cellPosition + Vector3Int.left;
                if(pointDict.TryGetValue(leftCell, out NodeData leftNode))
                    point.AddNeighbor(leftNode, LinkType.Normal);

                Vector3Int rightCell = point.cellPosition + Vector3Int.right;
                if(pointDict.TryGetValue(rightCell, out NodeData rightNode))
                    point.AddNeighbor(rightNode, LinkType.Normal);
            });
            
            //드랍노드 작업을 해줘야겠지?
            bakedData.points.Where(point => point.nodeType == NodeType.Drop).ToList().ForEach(point =>
            {
                for (int i = 1; i <= agentData.dropHeight; i++)
                {
                    Vector3Int downCell = point.cellPosition + new Vector3Int(0, -i, 0); //i만큼 밑으로 내린 셀
                    if (pointDict.TryGetValue(downCell, out NodeData downNode))
                    {
                        if(downNode.nodeType == NodeType.Drop) continue; //드랍노드끼리는 연결하지 않는다.
                        
                        point.AddNeighbor(downNode, LinkType.Drop); //떨어지는 연결을 만든다.

                        if (agentData.jumpDistance > i) //해당 노드에서 Drop노드까지 점프사거리 이내라면 
                        {
                            if (pointDict.TryGetValue(point.cellPosition + Vector3Int.left, out NodeData leftNode))
                            {
                                downNode.AddNeighbor(leftNode, LinkType.Jump);    
                            }else if (pointDict.TryGetValue(point.cellPosition + Vector3Int.right,
                                          out NodeData rightNode))
                            {
                                downNode.AddNeighbor(rightNode, LinkType.Jump);
                            }
                        }
                        
                        break; //땅을 찾았다면 더이상 찾을 필요 없으니 break;
                    }
                }
            });

            bakedData.points.Where(point => point.nodeType == NodeType.Edge).ToList().ForEach(point =>
            {
                Vector3Int searchDirection = Vector3Int.zero;

                foreach (LinkData linkData in point.neighbors)
                {
                    NodeData node = pointDict[linkData.endCellPosition];
                    if (node.nodeType == NodeType.Drop) 
                        //엣지가 옆에 드롭노드가 있다. 라는 말은 그쪽방향으로 뛰어내릴 수 있다는 말이다.
                    {
                        searchDirection = linkData.endCellPosition - point.cellPosition;
                    }
                }

                int height = Mathf.FloorToInt(agentData.jumpHeight);
                for (int i = 2; i < agentData.jumpDistance; i++)
                {
                    int xOffset = searchDirection.x * i;
                    Vector3Int nextCell = point.cellPosition + new Vector3Int(xOffset, height);

                    for (int j = 0; j < height * 2; j++) //위 아래로 점프높이만큼 찾아야 한다.
                    {
                        Vector3Int checkGround = nextCell + new Vector3Int(0, -j, 0);
                        if (pointDict.TryGetValue(checkGround, out NodeData groundPoint))
                        {
                            if (groundPoint.nodeType == NodeType.Edge)
                            {
                                point.AddNeighbor(groundPoint, LinkType.Jump);
                            }
                        }
                    }
                }
            });

        }

        private void OnDrawGizmos()
        {
            if(bakedData == null || drawGizmos == false) return;

            foreach (NodeData position in bakedData.points)
            {
                Gizmos.color = position.nodeType switch
                {
                    NodeType.Ground => groundColor,
                    NodeType.Jump => jumpColor,
                    NodeType.Drop => dropColor,
                    NodeType.Edge => edgeColor,
                    _ => Color.white
                };
                Gizmos.DrawWireSphere(position.worldPosition, 0.15f);

                foreach (LinkData linkData in position.neighbors)
                {
                    Gizmos.color = linkData.linkType switch
                    {
                        LinkType.Normal => Color.white,
                        LinkType.Jump => jumpColor,
                        LinkType.Drop => dropColor,
                        _ => Color.white
                    };
                    DrawLineGizmo(linkData.start, linkData.end, linkData.linkType);
                }
            }
        }

        private void DrawLineGizmo(Vector3 start, Vector3 end, LinkType linkType)
        {
            Vector3 direction = end - start;

            Vector3 normalDir = direction.normalized;
            Vector3 arrowStart = end - normalDir * 0.25f;
            Vector3 arrowEnd = end - normalDir * 0.15f;

            const float arrowSize = 0.05f;
            Vector3 trianglePointA = arrowStart + (Quaternion.Euler(0, 0, -90f) * normalDir) * arrowSize;
            Vector3 trianglePointB = arrowStart + (Quaternion.Euler(0, 0, 90f) * normalDir) * arrowSize;
            
            Gizmos.DrawLine(start, arrowStart);
            Gizmos.DrawLine(trianglePointA, arrowEnd);
            Gizmos.DrawLine(trianglePointB, arrowEnd);
            Gizmos.DrawLine(trianglePointA, trianglePointB);
        }
#endif
    }
}
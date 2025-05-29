using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.PathFinding
{
    public enum NodeType
    {
        None = 0, Ground = 1, Jump = 2, Drop = 3, Edge = 4
    }

    public enum LinkType
    {
        Normal =0, Jump = 1, Drop = 2
    }
    
    [Serializable]
    public struct LinkData
    {
        public Vector3 start;
        public Vector3Int startCellPosition;
        public Vector3 end;
        public Vector3Int endCellPosition;

        public float cost; //비용
        public LinkType linkType;
    }
    
    [Serializable]
    public class NodeData : IComparable<NodeData>
    {
        public Vector3 worldPosition;
        public Vector3Int cellPosition;
        public NodeType nodeType;
        public List<LinkData> neighbors = new List<LinkData>(); //내가 갈 수 있는 이웃 링크들

        public LinkData pathLink; //실제 경로 설정에 사용할 링크

        public void AddNeighbor(NodeData neighbor, LinkType linkType)
        {
            LinkData newLink = new LinkData
            {
                start = worldPosition,
                startCellPosition = cellPosition,
                end = neighbor.worldPosition,
                endCellPosition = neighbor.cellPosition,
                cost = Vector3Int.Distance(cellPosition, neighbor.cellPosition),
                linkType = linkType
            };
            neighbors.Add(newLink);
        }
        
        public int CompareTo(NodeData other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            return cellPosition == other.cellPosition ? 0 : 1;
        }

        public override int GetHashCode() => cellPosition.GetHashCode();

        public override bool Equals(object obj)
        {
            NodeData other = obj as NodeData;
            if (other == null) return false;
            
            return cellPosition == other.cellPosition;
        }

        public static bool operator == (NodeData lhs, NodeData rhs)
        {
            if (lhs is null)
            {
                if (rhs is null) return true;
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(NodeData lhs, NodeData rhs) => !(lhs == rhs);
    }
}
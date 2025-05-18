using System;
using UnityEngine;

namespace Code.PathFinding
{
    public class AstarNode : IComparable<AstarNode>
    {
        public NodeData nodeData;
        public AstarNode parent;
        public LinkData parentLinkData;

        public float G;
        public float F;

        public int CompareTo(AstarNode other)
        {
            if (Mathf.Approximately(other.F, F))
                return 0;
            return other.F < F ? -1: 1;
        }

        public override bool Equals(object obj)
        {
            AstarNode astarObj = obj as AstarNode;
            if (astarObj is null)
                return false;
            return nodeData == astarObj.nodeData;
        }

        public override int GetHashCode() => nodeData.GetHashCode();

        public static bool operator ==(AstarNode lhs, AstarNode rhs)
        {
            if (lhs is null)
            {
                if(rhs is null)
                    return true;
                return false;
            }

            return lhs.Equals(rhs);
        }
        public static bool operator !=(AstarNode lhs, AstarNode rhs) => !(lhs == rhs);
        
    }
}
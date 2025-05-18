using UnityEngine;

namespace Code.PathFinding
{
    [CreateAssetMenu(fileName = "AgentData", menuName = "SO/Path/AgentData", order = 0)]
    public class AgentDataSO : ScriptableObject
    {
        public float jumpDistance; //점프가 가능한 거리
        public float jumpHeight; //점프가 가능한 높이
        public float dropHeight; //낙하 가능 높이
    }
}
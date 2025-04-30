using Unity.AI.Navigation;
using UnityEngine;

namespace Blade.Test
{
    public class TestNavBaker : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;

        public void ReBakeNavMesh()
        {
            navMeshSurface.BuildNavMesh(); //기존 메시를 클리어하고 새로 굽니다.
        }
    }
}
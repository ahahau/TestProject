using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Entities;
using UnityEngine;

namespace Code.PathFinding
{
    public class NavAgent2D : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private BakedDataSO bakedData;

        [SerializeField] private LineRenderer lineRenderer;

        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        [SerializeField] private Transform target; 
        [ContextMenu("Test nav")]
        private void TestNav()
        {
            SetDestination(target.position);
        }
        public async void SetDestination(Vector3 position)
        {
            await FindPath(position);
        }

        private async Task FindPath(Vector3 destination)
        {
            List<NodeData> path = await bakedData.GetPath(transform.position, destination);


            Vector3[] points = path.Select(x => x.worldPosition).ToArray();
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }
    }
}
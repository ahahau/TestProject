using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Blade.Test.NavMesh
{
    public class NavMeshClimb : MonoBehaviour
    {
        [SerializeField] private int offMeshArea = 4;
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private float climbSpeed = 5f;

        private void Start()
        {
            StartCoroutine(StartClimbCo());
        }

        private IEnumerator StartClimbCo()
        {
            while (true)
            {
                yield return new WaitUntil(() => IsOnClimb());
                
                yield return StartCoroutine(ClimbTo());
            }
        }

        public bool IsOnClimb()
        {
            if(navAgent.isOnOffMeshLink ) //오프메시에 있고 점핑 데이터라면
            {
                OffMeshLinkData linkData = navAgent.currentOffMeshLinkData;
                var link = linkData.owner as NavMeshLink;
                if (link != null && link.area == offMeshArea)
                {
                    return true;
                }
            }
            return false;
        }

        private IEnumerator ClimbTo()
        {
            navAgent.isStopped = true;
            OffMeshLinkData linkData = navAgent.currentOffMeshLinkData;
            Vector3 start = transform.position;
            Vector3 end = linkData.endPos;
            Vector3 yEnd = new Vector3(start.x, end.y, start.z);
            Vector3 xzEnd = new Vector3(end.x, start.y, end.z);
            
            if (yEnd.y > start.y)
            {
                yield return StartCoroutine(MoveCoroutine(start, yEnd));
                yield return StartCoroutine(MoveCoroutine(yEnd, end));
            }
            else
            {
                yield return StartCoroutine(MoveCoroutine(start, xzEnd));
                yield return StartCoroutine(MoveCoroutine(xzEnd, end));
            }
            
             
            navAgent.CompleteOffMeshLink();
            navAgent.isStopped = false;
        }

        private IEnumerator MoveCoroutine(Vector3 start, Vector3 end)
        {
            float climbTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / climbSpeed);
            float currentTime = 0;
            float percent = 0;
            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / climbTime;
                
                Vector3 newPos = Vector3.Lerp(start, end, percent);
                transform.position = newPos;

                yield return null;
            }
            
        }

    }
}
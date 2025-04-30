using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Blade.Test.NavMesh
{
    public class NavAgentJump : MonoBehaviour
    {
        [SerializeField] private int offMeshArea = 2;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float jumpSpeed = 10.0f;
        [SerializeField] private float gravity = -9.81f;

        private IEnumerator Start()  //이렇게 하면 Start가 코루틴으로 실행된다.
        {
            while (true)
            {
                yield return new WaitUntil(IsOnJumpArea);

                yield return StartCoroutine(JumpToCoroutine());
            }
        }

        private IEnumerator JumpToCoroutine()
        {
            agent.isStopped = true; //이걸 하면 에이전트가 멈춘다. (길안내가 멈춘다.)

            OffMeshLinkData linkData = agent.currentOffMeshLinkData;
            Vector3 start = transform.position;
            Vector3 end = linkData.endPos;

            float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / jumpSpeed); //시간 구하기

            float currentTime = 0;
            float percent = 0;

            float v0 = (end - start).y - gravity; //y가 가지고 있는 초기속도

            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / jumpTime;

                Vector3 pos = Vector3.Lerp(start, end, percent);

                pos.y = start.y + (v0 * percent) + (gravity * percent * percent);

                transform.position = pos;
                yield return null;
            }
            
            agent.CompleteOffMeshLink();
            agent.isStopped = false;
        }

        public bool IsOnJumpArea()
        {
            if (agent.isOnOffMeshLink) //오프메시에 있고 
            {
                OffMeshLinkData linkData = agent.currentOffMeshLinkData; //연결정보를 가져온다.
                NavMeshLink link = linkData.owner as NavMeshLink;

                if (link != null && link.area == offMeshArea)
                    return true;
            }

            return false;
        }
        
        
    }
}
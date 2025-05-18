using System;
using System.Linq.Expressions;
using UnityEngine;

namespace Code.Test
{
    public class ExpressionTest : MonoBehaviour
    {
        public Func<int, int, int> TestFunc;
        
        [ContextMenu("MakeAction")]
        private void MakeAction()
        {
            // 값 1 개가 들어오면 1개를 리턴 해주는 
            //  x + 5 더해서 리턴해주는 함수.
            ParameterExpression aParam = Expression.Parameter(typeof(int), "a");
            ParameterExpression bParam = Expression.Parameter(typeof(int), "b");
            BinaryExpression addOperation = Expression.Add(aParam, bParam);

            TestFunc = Expression.Lambda<Func<int, int, int>>(addOperation, aParam, bParam).Compile();
        }

        private int Add(int a, int b)
        {
            return a + b;
        }

        [ContextMenu("RunAction")]
        private void RunAction()
        {
            if (TestFunc == null)
            {
                Debug.Log("현재 액션이 없습니다.");
            }
            else
            {
                Debug.Log($"실행 결과는 : {TestFunc(5, 10)}");
            }
        }
    }
}
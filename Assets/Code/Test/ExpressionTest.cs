using System;
using System.Linq.Expressions;
using UnityEngine;

namespace Code.Test
{
    public class ExpressionTest : MonoBehaviour
    {
        public Func<int, int> TestFunc;
        [ContextMenu("MakeAction")]
        private void MakeAction()
        {
            // 값 1개가 들어오면 1개를 리턴 해주는
            // x + 5 더해서 리턴해주는 함수
            ConstantExpression five = Expression.Constant(5);
            ParameterExpression xParam = Expression.Parameter(typeof(int), "x");
            BinaryExpression addOperation = Expression.Add(xParam, five);
            
            TestFunc = Expression.Lambda<Func<int, int>>(addOperation, xParam).Compile();
        }

        private int AddFive(int x)
        {
            return x + 5;
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
                Debug.Log($"실행결과는 : {TestFunc(10)}");
            }
        }
    }
}
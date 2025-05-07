using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Code.Player.FSM
{
    [CreateAssetMenu(fileName = "ActionBinder", menuName = "SO/Player/ActionBinder", order = 0)]
    public class ActionBinderSO : ScriptableObject
    {
        public Type InterfaceType { get; private set; }
        public Action SubscribeAction { get; private set; }
        public Action UnSubscribeAction { get; private set; }

        public string interfaceName;
        public string inputEventName;
        public string methodName;

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        
        public void Compile(PlayerController playerController)
        {
            InterfaceType = Type.GetType(interfaceName);
            MethodInfo method = playerController.GetType().GetMethod(methodName, bindingFlags); 
            
            ConstantExpression controller = Expression.Constant(playerController);
            MemberExpression inputProp = Expression.Property(controller, "PlayerInput");
            MemberExpression actionEvent = Expression.Field(inputProp, inputEventName);
            
            // playerController의 메서드를 Delegate로 변환
            MethodCallExpression methodCall = Expression.Call(controller, method);
            LambdaExpression methodDelegate = Expression.Lambda<Action>(methodCall);
            
            // actionEvent += playerController.Method
            //BinaryExpression addAssign = Expression.AddAssign(actionEvent, methodDelegate);
            MethodInfo combineMethod = typeof(Delegate).GetMethod("Combine", new[] { typeof(Delegate), typeof(Delegate) });
            BinaryExpression addAssign = Expression.Assign(
                actionEvent,
                Expression.Convert(
                    Expression.Call(combineMethod, actionEvent, methodDelegate),
                    typeof(Action)
                )
            );
            
            

            // actionEvent -= playerController.Method
            //BinaryExpression removeAssign = Expression.SubtractAssign(actionEvent, methodDelegate);
            MethodInfo removeMethod = typeof(Delegate).GetMethod("Remove", new[] { typeof(Delegate), typeof(Delegate) });
            BinaryExpression removeAssign = Expression.Assign(
                actionEvent,
                Expression.Convert(
                    Expression.Call(removeMethod, actionEvent, methodDelegate),
                    typeof(Action)
                )
            );
            // Expression Tree 컴파일 및 실행
            SubscribeAction = Expression.Lambda<Action>(addAssign).Compile();
            UnSubscribeAction = Expression.Lambda<Action>(removeAssign).Compile();
        }
    }
}
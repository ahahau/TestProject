using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using Delegate = System.Delegate;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "ActionBinder", menuName = "SO/Player/Binder", order = 0)]
    public class ActionBinderSO : ScriptableObject
    {
        public string interfaceName;
        public string inputEventName;
        public string bindMethodName;  //입력 이벤트와 연결시킬 매서드 이름

        //인터페이스의 이름으로부터 타입을 가져와야 해.
        public Type InterfaceType { get; private set; }
        // 구독 액션을 만들어야 해
        public Action SubscribeAction { get; private set; }
        // 구독해제 액션을 만들어야 해
        public Action UnSubscribeAction { get; private set; }

        private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        
        public void Compile(PlayerController player)
        {
            InterfaceType = Type.GetType(interfaceName); //이름을 넣으면 해당 이름에 맞는 타입을 찾아주는 System함수
            MethodInfo bindMethod = player.GetType().GetMethod(bindMethodName, flags);
            
            ConstantExpression playerConstant = Expression.Constant(player);
            MemberExpression inputProperty = Expression.Property(playerConstant, "PlayerInput");

            // += 연산을 위한 매서드 대리자를 만든다.
            Delegate handler = Delegate.CreateDelegate(typeof(Action), player, bindMethod);
            ConstantExpression handlerExpression = Expression.Constant(handler);
            // player.bindMethod
            
            // += 연산 수행
            MethodInfo addMethod = typeof(PlayerInputSO).GetEvent(inputEventName).GetAddMethod();
            MethodCallExpression addBind = Expression.Call(inputProperty, addMethod, handlerExpression);
            
            // -= 연산 수행
            MethodInfo removeMethod = typeof(PlayerInputSO).GetEvent(inputEventName).GetRemoveMethod();
            MethodCallExpression removeBind = Expression.Call(inputProperty, removeMethod, handlerExpression);

            SubscribeAction = Expression.Lambda<Action>(addBind).Compile();
            UnSubscribeAction = Expression.Lambda<Action>(removeBind).Compile();
        }

    }
}
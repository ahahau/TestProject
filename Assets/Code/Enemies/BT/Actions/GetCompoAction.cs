using System;
using System.Collections.Generic;
using Code.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetCompo", story: "Get Compo from [Self]", category: "Enemy/Action", id: "96897bbc5563a85e2643da7ae50b6dbb")]
    public partial class GetCompoAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        protected override Status OnStart()
        {
            Enemy enemy = Self.Value;

            List<BlackboardVariable> variableList = enemy.BTAgent.BlackboardReference.Blackboard.Variables;
            
            foreach(BlackboardVariable variable in variableList)
            {
                //IEntityComponent을 상속받는 타입만 가져온다.
                if(typeof(IEntityComponent).IsAssignableFrom(variable.Type) == false) continue;

                //해당 타입의 컴포넌트를 가져온다.
                IEntityComponent component = enemy.GetCompo(variable.Type);
                Debug.Assert(component != default, $"Check {variable.Name} component exist on {enemy.gameObject.name}");;

                bool isSuccess = SetVariableToBT(enemy, variable.Name, component);
                Debug.Assert(isSuccess, $"Set variable {variable.Name} to BT failed");
            }
            
            return Status.Success;
        }

        private bool SetVariableToBT(Enemy enemy, string variableName, IEntityComponent component)
        {
            if (enemy.BTAgent.GetVariable(variableName, out BlackboardVariable targetVariable))
            {
                targetVariable.ObjectValue = component;
                return true;
            }
            return false;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}


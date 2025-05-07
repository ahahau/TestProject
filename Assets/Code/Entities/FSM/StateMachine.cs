using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Entities.FSM
{
    public class StateMachine
    {
        public EntityState CurrentState { get; private set; }
        
        private Dictionary<string, EntityState> _states = new Dictionary<string, EntityState>();
        public StateMachine(Entity entity, StateSO[] stateList)
        {
            foreach (StateSO state in stateList)
            {
                try
                {
                    Type type = Type.GetType(state.className); //문자열로 실제 타입을 불러오는 것
                    EntityState playerState = Activator.CreateInstance(type, entity, state.animParam) as EntityState;
                    _states.Add(state.stateName, playerState);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{state.className} loading error, check your spell : {ex.Message}");
                }
            }
        }

        public EntityState ChangeState(string newStateName)
        {
            EntityState newState = _states.GetValueOrDefault(newStateName);
            Debug.Assert(newState != default, $"newState cannot be null {newStateName}");

            EntityState oldState = CurrentState;
            
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
            
            return oldState;
        }

        public void UpdateMachine()
        {
            CurrentState?.Update();
        }
    }
}
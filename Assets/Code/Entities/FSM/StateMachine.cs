using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Entities.FSM
{
    public class StateMachine
    {
        public EntityState currentState{ get; private set; }
        private Dictionary<string, EntityState> _states = new Dictionary<string, EntityState>();
        public StateMachine(Entity entity, StateSO[] stateList)
        {
            foreach (StateSO state in stateList)
            {
                try
                {
                    Type type = Type.GetType(state.className);
                    EntityState playerState = Activator.CreateInstance(type, entity, state.animParam) as EntityState;
                    _states.Add(state.stateName, playerState);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{state.className} lodding error, chekc your spell : {ex.Message}");
                }
            }
        }

        public void ChangeState(string newStateName)
        {
            EntityState newState = _states.GetValueOrDefault(newStateName);
            Debug.Assert(newState != default, $"newState cannot be null {newStateName}");
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void UpdateMachine()
        {
            currentState?.Update();
        }
    }
}
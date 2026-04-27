using System;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop47.Scripts.Game.Fsm
{
    public class Fsm
    {
        public FsmState CurrentState { get; private set; }
        public FsmState PreviousState { get; private set; }

        public Action<FsmState> OnStateChanged;
        
        private readonly Dictionary<Type, FsmState> _states = new();

        public void AddState(FsmState state)
        {
            _states.Add(state.GetType(), state);
        }

        public void SetState<T>() where T : FsmState
        {
            var type = typeof(T);

            if (CurrentState != null && CurrentState.GetType() == type)
            {
                return;
            }

            if (_states.TryGetValue(type, out var newState))
            {
                CurrentState?.Exit();
                PreviousState = CurrentState;
                CurrentState = newState;
                CurrentState.Enter();
                
                OnStateChanged?.Invoke(CurrentState);
            }
        }

        public void LogicUpdate()
        {
            CurrentState?.LogicUpdate();
        }
        
        public void PhysicsUpdate()
        {
            CurrentState?.PhysicsUpdate();
        }

        public void LateUpdate()
        {
            CurrentState?.LateUpdate();
        }

        public void OnTriggerEnter(Collider other)
        {
            CurrentState?.OnTriggerEnter(other);
        }

        public void OnTriggerStay(Collider other)
        {
            CurrentState?.OnTriggerStay(other);
        }

        public void OnTriggerExit(Collider other)
        {
            CurrentState?.OnTriggerExit(other);
        }
        
        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
            CurrentState?.OnControllerColliderHit(hit);
        }
    }
}
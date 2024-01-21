using System;
using UnityEngine;

namespace MazurkaFSM
{
    public class StateMachine 
    {
        public StateMachine(GameObject owner)
        {
            Owner = owner;
        }
        
        public State CurrentState { get; protected set; }
        public State LastState { get; protected set; }
        public State NextState { get; protected set; }
    
        public bool IsActive { get; protected set; }
        public bool IsInPause { get; protected set; }
        public bool IsPerformingTransition { get; protected set; }
        
        public GameObject Owner { get; }
        
        public event Action<State> StateHasChanged;
        public event Action<State, State> StateHasBeenCanceled;

        
        public virtual void StartStateMachine(State initialState)
        {
            if (IsActive) return;

            CurrentState = initialState ?? throw new SystemException("You can't start FSM with a null state");
            CurrentState.EnterState();
            IsActive = true;
        }

        public virtual void PauseStateMachine()
        {
            IsInPause = true;
        }

        public virtual void ResumeStateMachine()
        {
            IsInPause = false;
        }

        public virtual void StopStateMachine(bool exitLastState = true)
        {
            if (!IsActive) return;

            if (exitLastState) CurrentState.TryExistState();
            LastState = CurrentState;
            IsActive = false;
        }
        
        
        public virtual void Update()
        {
            if (!IsActive || IsInPause) return;

            CurrentState?.LogicUpdate();
        }

        public virtual void FixedUpdate()
        {
            if (!IsActive || IsInPause) return;
            
            CurrentState?.PhysicUpdate();
        }

        public virtual void LateUpdate()
        {
            if (!IsActive || IsInPause) return;
            
            CurrentState?.PostUpdate();
        }


        public virtual bool ChangeState(State state)
        {
            if (!IsActive || IsInPause || IsPerformingTransition || (state == CurrentState && !CurrentState.CanTransitionToSelf)) return false;

            return PerformTransition(state);
        }
        
        protected virtual bool PerformTransition(State nextState)
        {
            if (nextState == null ) return false;

            NextState = nextState; //make next state accessible to be able to cancel transition to next state specifically
            IsPerformingTransition = true;
            
            if (CurrentState != null)//may not happen, but for security
            {
                //Exit current state, if the state return false the transition is cancel
                if (!CurrentState.TryExistState())
                {
                    NextState = null;
                    IsPerformingTransition = false;
                    StateHasBeenCanceled?.Invoke(CurrentState, nextState);
                    return false;
                }
                
                LastState = CurrentState; //make last state accessible to be able to work with in next state specifically
            }
            
            NextState = null;
            
            CurrentState = nextState; //Update Current State
            CurrentState.EnterState();
            StateHasChanged?.Invoke(CurrentState);
            
            IsPerformingTransition = false;
            return true;
        }
    }
}


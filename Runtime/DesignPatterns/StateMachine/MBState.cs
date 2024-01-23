using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// A monoBehaviour state, util to serialize data on the monoBehaviour to work with.
    /// </summary>
    public abstract class MBState : MonoBehaviour, IState
    {
        /// <summary>
        /// Each time state enter, startTime will be set to StateMachine Time.time;
        /// </summary>
        public float StartTime { get; protected set; }

        /// <summary>
        /// Time elapsed since enter state, in state machine delta time.
        /// </summary>
        public float TimeElapsed => Time.time - StartTime;

        public virtual bool CanTransitionToSelf { get; } = false;

        public bool IsActive { get; protected set; }
        
        /// <summary>
        /// Condition to be exited
        /// </summary>;
        protected virtual bool CanExitState => true;

        
        public void EnterState()
        {
            StartTime = Time.time;
            IsActive = true;
            OnStateEnter();
        }
        public bool TryExistState()
        {
            if (!CanExitState) return false;
            
            OnStateExit();
            IsActive = false;
            return true;
        }
        
        public abstract void LogicUpdate();
        public abstract void PostUpdate();
        public abstract void PhysicUpdate();
        
        
        protected abstract void OnStateEnter();
        protected abstract void OnStateExit();
    }
}

using UnityEngine;

namespace MazurkaFSM
{
    public abstract class State
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

        public void LogicUpdate()
        {
            OnDoCheck();
            Update();

#if DEBUG
            DebugUpdate();
#endif
        }

        public void PostUpdate()
        {
            LateUpdate();
            OnCheckTransitions();
        }

        public void PhysicUpdate()
        {
            FixedUpdate();
        }



        protected abstract void OnStateEnter();
        protected abstract void OnStateExit();
        /// <summary>
        /// Use to initialize the state each frame before LogicUpdate (useful to get, check, cast,...)
        /// </summary>
        protected virtual void OnDoCheck() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }
        protected virtual void LateUpdate() { }
        protected virtual void DebugUpdate() { }

        /// <summary>
        /// Check after LateUpdate for transition
        /// </summary>
        protected abstract void OnCheckTransitions();
    }
}

using System;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// A monoBehaviour state casting state machine reference if not assigned by hand before start
    /// </summary>
    public abstract class MBBasicState : MBState
    {
        protected StateMachine stateMachine;
        
        private void Start()
        {
            if (stateMachine != null) return;
            var fsmActor = GetComponentInParent<IStateMachineActor>();
            if(fsmActor != null) stateMachine = fsmActor.GetStateMachine;
            else throw new NullReferenceException($"Can't found the state machine for the state {name} on {gameObject.name} gameObject");
        }

        public void AssignStateMachine(StateMachine newStateMachine)
        {
            stateMachine = newStateMachine;
        }
    }
}

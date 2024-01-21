using MazurkaFSM;

namespace TheMazurkaStudio.Utilities
{
    public abstract class SimpleState : State
    {
        protected StateMachine stateMachine;
        
        protected SimpleState(StateMachine stateMachine) : base()
        {
            this.stateMachine = stateMachine;
        }
    }
}

namespace TheMazurkaStudio.Utilities
{
    public abstract class BasicState : State
    {
        protected StateMachine stateMachine;
        
        protected BasicState(StateMachine stateMachine) : base()
        {
            this.stateMachine = stateMachine;
        }
    }
}

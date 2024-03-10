namespace TheMazurkaStudio.Utilities
{
    public interface ICompletableCommand : ICommand
    {
        public void Dispose();
        public void Complete(CommandStatus finalStatus);
        
        public bool IsCompleted { get; }
    }
}


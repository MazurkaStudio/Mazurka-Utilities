namespace TheMazurkaStudio.Utilities
{
    public interface ICompletableCommand : ICommand
    {
        public void Dispose();
        public void Complete();
        
        public bool IsCompleted { get; }
    }
}


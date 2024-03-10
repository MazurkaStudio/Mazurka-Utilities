namespace TheMazurkaStudio.Utilities
{
    public interface ICommand
    {
        public void Execute();
        public void Interrupt();
    }
}

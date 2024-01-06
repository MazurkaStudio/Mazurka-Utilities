namespace TheMazurkaStudio.Utilities
{
    public interface IUndoCommand : ICommand
    {
        public void Undo();
    }
}

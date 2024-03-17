namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    public interface IInteractableCondition
    {
        public IInteractable Interactable { get; }
        public bool CanInteractCondition(IInteractionActor actor);
    }
}

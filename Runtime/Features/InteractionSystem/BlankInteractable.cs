namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    /// <summary>
    /// Simple distance based interactable that auto release interaction immediately, only use with unity events
    /// </summary>
    public class BlankInteractable : InteractableBase
    {
        protected override void OnInteract(IInteractionActor interactionObject)
        {
            //Blank interaction can not handle interaction over time because no logic is write for in the interactable
            TryReleaseInteraction();
        }

        protected override bool TryStartInteraction(IInteractionActor interactionActor)
        {
            return true;
        }

        protected override void OnInteractionHasBeenReleased() { }

        protected override void OnInteractionStateHasChanged(bool isVisible) { }
    }
}

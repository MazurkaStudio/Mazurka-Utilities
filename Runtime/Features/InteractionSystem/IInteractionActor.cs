using UnityEngine;

namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    /// <summary>
    /// Any actor that handle interaction logic
    /// </summary>
    public interface IInteractionActor 
    {
        /// <summary>
        /// Reference to actor game object (root parent of th actor if possible for cast in children when need)
        /// </summary>
        public GameObject Actor { get; }
        /// <summary>
        /// The current interaction if exist
        /// </summary>
        public IInteractable CurrentInteractable { get; }



        /// <summary>
        /// The interaction state of actor
        /// </summary>
        public bool CanInteract(IInteractable interactable);
        /// <summary>
        /// Fast way to check if interaction exist (just check if current interactable is not null)
        /// Being in interaction is not representing the interaction state itself, check InteractionHasStart (ex = creature is moving to a lever)
        /// </summary>
        public bool IsInInteraction { get; }
        /// <summary>
        /// The interaction is running, except if interaction release immediately
        /// </summary>
        public bool InteractionHasStart { get; }
        
        
        
        /// <summary>
        /// If can interact, the actor will interact with the nearest/better interactable possible
        /// </summary>
        public bool TryInteract();
        /// <summary>
        /// Try interact with a given interactable, ignore checks will ignore distance, facing dir ...
        /// </summary>
        /// <param name="interactable"></param>
        /// <param name="ignoreChecks"></param>
        /// <returns></returns>
        public bool TryInteractWith(IInteractable interactable, bool ignoreChecks);
        /// <summary>
        /// If interaction is active, release it, and allow actor to interact again
        /// </summary>
        public bool TryReleaseCurrentInteraction();
        
        
        
        /// <summary>
        /// Call by interactable when release is successful
        /// </summary>
        /// <param name="interactable"></param>
        public void ReleaseCallback(IInteractable interactable);
        /// <summary>
        /// Call by interactable when interaction successfully start
        /// </summary>
        /// <param name="interactable"></param>
        public void StartInteractionCallback(IInteractable interactable);
        
        
        
        /// <summary>
        /// Lock this interaction actor with an interactable to disable interaction system
        /// </summary>
        /// <param name="interactable"></param>
        public bool TryLock(IInteractable interactable);
        /// <summary>
        /// If interaction actor is lock with this interactable, just clear to enable again interaction system
        /// </summary>
        /// <param name="interactable"></param>
        public bool TryUnlock(IInteractable interactable);
    }
}

using System;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    /// <summary>
    /// Any interactable in the game
    /// </summary>
    public interface IInteractable
    {
        public Vector3 WorldPosition { get; }
        public float MaxDistanceToInteract { get; }
        public bool IgnoreLookAtDir { get; }
        public bool InteractionHasStart { get; }
        public GameObject InteractableObject { get; }
        
        /// <summary>
        /// Lock interaction for other interaction actor
        /// </summary>
        /// <param name="actor"></param>
        public bool TryLock(IInteractionActor actor);
        /// <summary>
        /// Unlock automatically when interaction is release, but you can cancel an interaction before if need
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public bool TryUnlock(IInteractionActor actor);
        
        public IInteractionActor CurrentInteractionActor { get; }
        public void RegisterInteractionActor(IInteractionActor actor, bool displayInteractionIcon);
        public void UnRegisterInteractionActor(IInteractionActor actor, bool displayInteractionIcon);
        
        /// <summary>
        /// Should not be call, call TryInteractWith on the interaction actor instead
        /// </summary>
        /// <param name="interactionObject"></param>
        /// <returns></returns>
        public bool TryInteract(IInteractionActor interactionObject);
        public bool TryReleaseInteraction();
        
        public bool CanInteract(IInteractionActor actor);
        public bool IsInInteraction { get; }
        
        public void AddTriggerCondition(InteractableConditionBase interactableCondition);
        public void RemoveTriggerCondition(InteractableConditionBase interactableCondition);
        
        public Action InteractionHasBeenReleased { get; set; }
    }
}

using UnityEngine;

namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    public abstract class InteractableConditionBase : MonoBehaviour, IInteractableCondition
    {
        protected IInteractable interactable;
        public IInteractable Interactable => interactable;
        
        protected virtual void OnEnable()
        {
            interactable = GetComponentInParent<IInteractable>();

            if (interactable == null) 
            {
                enabled = false;
                return;
            }
            
            interactable.AddTriggerCondition(this);
        }

        protected virtual void OnDisable()
        {
            if (interactable == null) return;
            
            interactable.RemoveTriggerCondition(this);
        }

       
        public abstract bool CanInteractCondition(IInteractionActor actor);
    }
}

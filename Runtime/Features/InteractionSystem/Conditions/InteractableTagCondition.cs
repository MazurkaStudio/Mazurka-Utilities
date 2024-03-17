using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    public class InteractableTagCondition : InteractableConditionBase
    {
        [SerializeField, BoxGroup("Tags"), TagField] private string _excludeTag;
        [SerializeField, BoxGroup("Tags"), TagField] private string _requireTag;
        
        public override bool CanInteractCondition(IInteractionActor actor)
        {
            if(!string.IsNullOrEmpty(_excludeTag) && actor.Actor.CompareTag(_excludeTag)) return false;
            if(!string.IsNullOrEmpty(_requireTag) && !actor.Actor.CompareTag(_requireTag)) return false;
            return true;
        }
    }
}

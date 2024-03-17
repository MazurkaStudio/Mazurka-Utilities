using System.Collections.Generic;
using Sirenix.OdinInspector;
using TheMazurkaStudio.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    /// <summary>
    /// Base class for any interaction actor
    /// </summary>
    public abstract class InteractionActorBase : MonoBehaviour, IInteractionActor
    {
        public GameObject Actor => interactionActor;
        public IInteractable CurrentInteractable { get; private set; }
        public virtual bool CanInteract(IInteractable interactable) => (!IsInInteraction || IsInInteraction && CurrentInteractable == interactable) && !interactionBuffer.IsInBuffer;
        public bool IsInInteraction => CurrentInteractable != null;
        public bool InteractionHasStart { get; private set; }


        [BoxGroup("Interaction Actor"), SerializeField] protected GameObject interactionActor;
        [BoxGroup("Interaction Actor"), SerializeField] protected float timeBetweenInteraction = 0.25f;
        [BoxGroup("Interaction Actor"),SerializeField] private UnityEvent<IInteractable> startInteraction;
        [BoxGroup("Interaction Actor"),SerializeField] private UnityEvent<IInteractable> releaseInteraction;
        
        [SerializeField, BoxGroup("Distance Based Settings")] protected float interactionMaxDistance = 1f;
        [SerializeField, BoxGroup("Distance Based Settings")] protected Vector3 activationOffset;
        
        [SerializeField, BoxGroup("Distance Based Cast Settings")] protected LayerMask interactionLayer;
        [SerializeField, BoxGroup("Distance Based Cast Settings")] protected bool displayInteractionIcon;
        [SerializeField, BoxGroup("Distance Based Cast Settings")] protected bool autoUpdate;
        [SerializeField, BoxGroup("Distance Based Cast Settings")] protected float updateRate = 0.25f;
        [SerializeField, BoxGroup("Distance Based Cast Settings")] protected int maxInteractions = 6;
        
        
        private BufferValue interactionBuffer;
        protected BufferValue interactionCheckBuffer;
        protected Collider2D[] castResults;
        protected IInteractable nearestValidInteractable;
        protected readonly HashSet<IInteractable> interactableContext = new();

      
        //MAIN
        public bool TryInteract()
        {
            FindNearestInteractable();
            
            if (nearestValidInteractable == null) return false;
            
            if (!CanInteract(nearestValidInteractable)) return false;
        
            if (nearestValidInteractable.CanInteract(this))
            {
                Lock(nearestValidInteractable);
                
                //Waiting for callback
                if (CurrentInteractable.TryInteract(this)) return true;
            }
            
            interactionBuffer.Trigger();
            UnLock();
            return false;
        }

        public bool TryInteractWith(IInteractable interactable, bool ignoreChecks)
        {
            if (!ignoreChecks)
            {
                if (!TestInteractable(interactable, out var dist)) return false;
            }
            if (!CanInteract(interactable)) return false;
            if (interactable == null) return false;
            if (interactable.CanInteract(this))
            {
                Lock(interactable);
                
                //Waiting for callback
                if (CurrentInteractable.TryInteract(this)) return true;
            }
            
            interactionBuffer.Trigger();
            UnLock();
            return false;
        }

        public bool TryReleaseCurrentInteraction()
        {
            if (!IsInInteraction || InteractionHasStart) return false;

            return CurrentInteractable.TryReleaseInteraction();
        }

        //Sometimes interaction will be trigger from system, not by the actor itself, so we use a callback instead
        public void StartInteractionCallback(IInteractable interaction)
        {
            TryLock(interaction);
            InteractionHasStart = true;
            interactionBuffer.Use();
            OnInteract(interaction);
            startInteraction?.Invoke(interaction);
            nearestValidInteractable = null;
            interaction.UnRegisterInteractionActor(this, displayInteractionIcon);
        }
        
        public void ReleaseCallback(IInteractable interactable)
        {
            if (interactable == CurrentInteractable)
            {
                interactable.UnRegisterInteractionActor(this, displayInteractionIcon);
                InteractionHasStart = false;
                interactionBuffer.Trigger();
                UnLock();
                releaseInteraction?.Invoke(CurrentInteractable); 
            }
        }
        
        
        public bool TryLock(IInteractable interactable)
        {
            if (CurrentInteractable != null) return false;
            if (interactable == null) return false;
          
            Lock(interactable);
            return true;
        }
        public bool TryUnlock(IInteractable interactable)
        {
            if (!IsInInteraction) return false;
            if (CurrentInteractable != interactable) return false;
            
            UnLock();
            return true;
        }
        
        //LOGIC
        protected void Lock(IInteractable interactable) => CurrentInteractable = interactable;
        protected void UnLock() => CurrentInteractable = null;
        
        /// <summary>
        /// Return any collider in range and layers
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected Collider2D[] Cast(out int size)
        {
            var fromPos = (Actor.transform.position + activationOffset);
            castResults = new Collider2D[maxInteractions];
            size = Physics2D.OverlapCircleNonAlloc(fromPos, interactionMaxDistance, castResults, interactionLayer);
            return castResults;
        }
        private void CastContext()
        {
            var fromPos = (Actor.transform.position + activationOffset);
            castResults = new Collider2D[maxInteractions];
            var size =  Physics2D.OverlapCircleNonAlloc(fromPos, interactionMaxDistance, castResults, interactionLayer);
            
            interactableContext.Clear();
            
            for (int i = 0; i < size; i++)
            {
                var other = castResults[i].GetComponentInParent<IInteractable>();
                if (other == null) continue;
                
                if (!other.CanInteract(this)) continue;
                interactableContext.Add(other);
            }
        }
        private void FindNearestInteractable()
        {
            CastContext();
            
            if (NearestInteraction( out var newInteraction))
            {
                if (nearestValidInteractable != null)
                {
                    if (nearestValidInteractable == newInteraction) return;
                    nearestValidInteractable.UnRegisterInteractionActor(this, displayInteractionIcon);
                }
                nearestValidInteractable = newInteraction;
                nearestValidInteractable?.RegisterInteractionActor(this, displayInteractionIcon);
                return;
            }

            if (nearestValidInteractable != null)
            {
                nearestValidInteractable.UnRegisterInteractionActor(this, displayInteractionIcon);
                nearestValidInteractable = null;
            }
        }
        private bool NearestInteraction(out IInteractable closestInteractable)
        {
            closestInteractable = null;
            
            var nearest = float.MaxValue;
            
            foreach (var interactable in interactableContext)
            {
                if (!TestInteractable(interactable, out var dist)) continue;
                
                if (!(dist < nearest)) continue;
                
                nearest = dist;
                closestInteractable = interactable;
            }

            return closestInteractable != null;
        }

        private bool TestInteractable(IInteractable interactable, out float dist)
        {
            var delta = interactable.WorldPosition - Actor.transform.position + activationOffset;
            dist = delta.magnitude; 
            if (interactable.MaxDistanceToInteract > 0f && dist > interactable.MaxDistanceToInteract) return false;

            var angle = Mathf.Abs(Vector2.SignedAngle(LookAtDir, delta));
            //Is not look at interaction (2D)
            if(!interactable.IgnoreLookAtDir && angle > 90f) return false;
            return true;
        }

        
        
        protected virtual void Awake()
        {
            interactionBuffer = new BufferValue(timeBetweenInteraction);
            interactionCheckBuffer = new BufferValue(updateRate);
        }

        protected virtual void Update()
        {
            if (!autoUpdate || IsInInteraction) return;
            
            if (!interactionCheckBuffer.IsInBuffer)
            {
                interactionCheckBuffer.Trigger();
                FindNearestInteractable();
            }
        }
        
        protected virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR
            
            if (!InteractionSystemDebugger.DEBUG_INTERACTION_SYSTEM) return;
            
            if (Application.isPlaying)
            {
                var color = InteractionHasStart ? Color.cyan : IsInInteraction ? Color.red : Color.blue;
                Gizmos.color = color;
                var style = new GUIStyle();
                style.normal.textColor = color;
                style.alignment = TextAnchor.MiddleCenter;
                UnityEditor.Handles.color = color;
                
                UnityEditor.Handles.Label(transform.position + Vector3.up * 1.75f,
                    $"IsInInteraction with " 
                    + (IsInInteraction 
                        ? CurrentInteractable.InteractableObject.name 
                        : "nothing"), style);
                
                
                var fromPos = transform.position + activationOffset;
                Gizmos.DrawWireSphere(fromPos, interactionMaxDistance);

                if (CurrentInteractable != null)
                {
                    var pos = CurrentInteractable.WorldPosition;
                    UnityEditor.Handles.DrawLine(fromPos + Vector3.up * 0.4f, pos, 8f);
                }
                else if(nearestValidInteractable != null)
                {
                    var pos = nearestValidInteractable.WorldPosition;
                    UnityEditor.Handles.DrawLine(fromPos + Vector3.up * 0.4f, pos, 8f);
                }
              
            }
            else
            {
                Gizmos.color = Color.blue;
                var fromPos = transform.position + activationOffset;
                Gizmos.DrawWireSphere(fromPos, interactionMaxDistance);
            }
#endif
        }
        
        
        protected abstract Vector2 LookAtDir { get; }
        protected abstract void OnInteract(IInteractable interactable);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField, BoxGroup("Interactable Parameters")] protected bool enableByDefault = true;
        [SerializeField, BoxGroup("Interactable Parameters")] protected bool useOnce;
        [FormerlySerializedAs("isImmediate")] [SerializeField, BoxGroup("Interactable Parameters")] protected bool shouldReleaseImmediately;
        [SerializeField, BoxGroup("Interactable Parameters")] protected bool needHold;
        [SerializeField, ShowIf("needHold"), BoxGroup("Interactable Parameters")] protected float holdDuration;
        [field: SerializeField, BoxGroup("Interactable Parameters")] public bool IgnoreLookAtDir { get; private set; }
        public GameObject InteractableObject => gameObject;
        
        [FormerlySerializedAs("activationOffset"), SerializeField, BoxGroup("Interactable Parameters")] protected Vector3 positionOffset;
        [FormerlySerializedAs("activationDistance"), SerializeField, BoxGroup("Interactable Parameters")] protected float maxDistanceToInteract = -1f;
        [SerializeField] private bool useInteractionIcon = true;
        [SerializeField, ShowIf("useInteractionIcon")] private GameObject interactionImage;
        
        [FormerlySerializedAs("onInteract")] [SerializeField, FoldoutGroup("Callbacks")] protected UnityEvent startInteraction;
        [FormerlySerializedAs("onEnableInteract")] [SerializeField, FoldoutGroup("Callbacks")] protected UnityEvent showInteraction;
        [FormerlySerializedAs("onDisableInteract")] [SerializeField, FoldoutGroup("Callbacks")] protected UnityEvent hideInteraction;

        public event Action<IInteractable> InteractCallback;

        public Vector3 WorldPosition => transform.position + positionOffset;

        public float MaxDistanceToInteract => maxDistanceToInteract;
        
        private readonly HashSet<InteractableConditionBase> conditions = new();
        
        public void AddTriggerCondition(InteractableConditionBase interactableCondition) => conditions.Add(interactableCondition);
        public void RemoveTriggerCondition(InteractableConditionBase interactableCondition) =>  conditions.Remove(interactableCondition);
        public Action InteractionHasBeenReleased { get; set; }

        public IInteractionActor CurrentInteractionActor { get; private set; }

        private readonly HashSet<IInteractionActor> currentPotentialInteractionActors = new(); //Handle all the actor with this interactable as nearest 
        private readonly HashSet<IInteractionActor> currentPotentialDisplayInteractionActors = new(); //Handle the actor who need display interaction icon

        private bool isEnable = true;
        private bool isVisible;
        private bool isReleasing;
        private bool used;

        public virtual bool CanInteract(IInteractionActor actor) 
            => isEnable && (!useOnce || !used)
               && (!IsInInteraction || IsInInteraction && CurrentInteractionActor == actor) 
               && conditions.All(condition => condition.CanInteractCondition(actor));
        
        public bool IsInInteraction => CurrentInteractionActor != null; //is lock with an interaction actor
        public bool InteractionHasStart { get; private set; } //is running the interaction (if is not immediate)
        

        protected virtual  void OnEnable()
        {
            isVisible = false;
            isEnable = enableByDefault;
            if(useInteractionIcon) interactionImage.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            if(IsInInteraction) ClearInteractable();
        }


        #region Hold

        protected bool _isHolding;
        
        protected virtual void StartHold()
        {
            _holdCoroutine = StartCoroutine(HoldCoroutine());
        }

        protected virtual void UpdateHold(float relative) { }
        
        protected virtual void CancelHold()
        {
            _isHolding = false;
            //InputsManager.OnReleaseInteract -= CancelHold;
            CurrentInteractionActor = null;
            if(_holdCoroutine != null) StopCoroutine(_holdCoroutine);
        }
        
        private Coroutine _holdCoroutine;
    
        private IEnumerator HoldCoroutine()
        {
            _isHolding = true;
            var elapsed = 0f;
            //InputsManager.OnReleaseInteract += CancelHold;

            if (holdDuration <= 0f)
            {
                yield break;
            }
            
            while (elapsed < holdDuration)
            {
                var relative = elapsed / holdDuration;
                UpdateHold(relative);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            _isHolding = false;
            //InputsManager.OnReleaseInteract -= CancelHold;
            
            StartInteraction();
        }

        #endregion

        
        public void RegisterInteractionActor(IInteractionActor actor, bool displayInteractionIcon)
        {
            if (actor == null) return;
            if (currentPotentialInteractionActors.Contains(actor)) return;
            
            currentPotentialInteractionActors.Add(actor);
            
            if (displayInteractionIcon)
            {
                currentPotentialDisplayInteractionActors.Add(actor);
                
                if (!isVisible && currentPotentialDisplayInteractionActors.Count > 0)
                {
                    isVisible = true;
                    if(useInteractionIcon && interactionImage != null) interactionImage.SetActive(true);
                    OnInteractionStateHasChanged(true);
                    showInteraction?.Invoke();
                }
            }
        }
        public void UnRegisterInteractionActor(IInteractionActor actor, bool displayInteractionIcon)
        {
            Debug.Log("Unregister");
            if (actor == null) return;

            if (!currentPotentialInteractionActors.Contains(actor)) return;
            
            currentPotentialInteractionActors.Remove(actor);

            if (displayInteractionIcon)
            {  Debug.Log("Try hide " + currentPotentialDisplayInteractionActors.Count);
                currentPotentialDisplayInteractionActors.Remove(actor);
                
                //Todo : dont pass the actor, we dont need to know
                if (currentPotentialDisplayInteractionActors.Count <= 0)
                {
                    Debug.Log("Hide");
                    isVisible = false;
                    if(useInteractionIcon && interactionImage != null) interactionImage.SetActive(false);
                    OnInteractionStateHasChanged(false);
                    hideInteraction?.Invoke();
                }
            }
        }
        
        
        public bool TryInteract(IInteractionActor interactionActor)
        {
            if (!CanInteract(interactionActor)) return false;
            if (!TryStartInteraction(interactionActor)) return false;

            CurrentInteractionActor = interactionActor; //Lock
            Debug.Log("START INTERACTION");
            if (needHold)
            {
                StartHold();
                return true;
            }
            
            StartInteraction();
            return true;
        }
        public bool TryReleaseInteraction()
        {
            if (!IsInInteraction || isReleasing) return false;
            
            ClearInteractable();
            
            InteractionHasBeenReleased?.Invoke();
            OnInteractionHasBeenReleased();
            return true;
        }

        
        public bool TryLock(IInteractionActor actor)
        {
            if (IsInInteraction) return false;
            if (actor == null) return false;

            Lock(actor);
            return true;
        }
        public bool TryUnlock(IInteractionActor actor)
        {
            if (!IsInInteraction) return false;
            if (actor != CurrentInteractionActor) return false;
           
            Unlock();
            return true;
        }

        
        private void StartInteraction()
        {
            if (shouldReleaseImmediately) StartCoroutine(ReleaseImmediately());
            CurrentInteractionActor.StartInteractionCallback(this);
            InteractionHasStart = true;
            startInteraction?.Invoke();
            InteractCallback?.Invoke(this);
            OnInteract(CurrentInteractionActor);
            used = true;
        }
        private IEnumerator ReleaseImmediately()
        {
            isReleasing = true;
            yield return new WaitForEndOfFrame();
            isReleasing = false;
            
            TryReleaseInteraction();
        }
        private void ClearInteractable()
        {
            //InputsManager.OnInteract -= CancelHold;
       
            if (CurrentInteractionActor != null)
            {
                CurrentInteractionActor.ReleaseCallback(this);
            }
            
            CurrentInteractionActor = null; //unlock
            InteractionHasStart = false;
            isReleasing = false;
        }
      

        
        
        
        protected abstract void OnInteract(IInteractionActor interactionActor);
        /// <summary>
        /// Try interaction in heritage before success
        /// </summary>
        /// <param name="interactionActor"></param>
        /// <returns></returns>
        protected abstract bool TryStartInteraction(IInteractionActor interactionActor);
        protected abstract void OnInteractionHasBeenReleased();
        protected abstract void OnInteractionStateHasChanged(bool isVisible);

        
        
        public void RegisterInteractionCallback(Action<IInteractable> callback) =>  InteractCallback += callback;
        public void UnregisterInteractionCallback(Action<IInteractable>  callback) =>  InteractCallback -= callback;

        public void Enable(bool value) => isEnable = value;


        private void Lock(IInteractionActor actor) => CurrentInteractionActor = actor;
        private void Unlock() => CurrentInteractionActor = null;
        
        
        protected virtual void OnDrawGizmosSelected()
        {
          

#if UNITY_EDITOR

            if (!InteractionSystemDebugger.DEBUG_INTERACTION_SYSTEM) return;
            
            if (Application.isPlaying)
            {
                Gizmos.color= Color.white;
                foreach (var actor in currentPotentialInteractionActors)
                {
                    Gizmos.DrawLine(actor.Actor.transform.position + Vector3.up * 0.4f, WorldPosition);
                }
                
                var color = (!isEnable || useOnce && used) ? Color.black 
                    : InteractionHasStart ? Color.cyan 
                    : IsInInteraction ? Color.red : Color.green;
                
                Gizmos.color =color;
                var style = new GUIStyle();
                style.normal.textColor =color;
                style.alignment = TextAnchor.MiddleCenter;
                
                
                UnityEditor.Handles.Label(WorldPosition + Vector3.up * 1.5f,
                    $"IsInInteraction with " 
                    + (IsInInteraction 
                        ? CurrentInteractionActor.Actor.GetComponentInParent<Rigidbody2D>().gameObject.name 
                        : "nothing"), style);
                
                
                Gizmos.DrawWireSphere(WorldPosition, maxDistanceToInteract);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(WorldPosition, maxDistanceToInteract);
            }
#endif
        }
    }
}

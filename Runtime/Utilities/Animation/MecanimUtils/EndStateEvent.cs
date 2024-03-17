using UnityEngine;
using UnityEngine.Events;

namespace TheMazurkaStudio.Utilities.Animation
{
    public class EndStateEvent : StateMachineBehaviour
    {
        [SerializeField] private UnityEvent eventSend;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            
            eventSend?.Invoke();
        }
    }
}
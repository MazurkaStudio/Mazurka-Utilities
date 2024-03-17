using UnityEngine;

namespace TheMazurkaStudio.Utilities.Animation
{
    public class EndStateEventSender : StateMachineBehaviour
    {
        [SerializeField] private string eventName;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            
            animator.GetComponent<AnimatorHelper>()?.RaiseEvent(eventName);
        }
    }
}

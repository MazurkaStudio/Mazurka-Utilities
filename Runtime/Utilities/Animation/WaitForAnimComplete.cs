using UnityEngine;

namespace TheMazurkaStudio.Utilities.Animation
{
    public class WaitForAnimCompleted : CustomYieldInstruction
    {
        private readonly string animationName;

        private readonly Animator animator;
        private readonly int layerIndex;


        private AnimatorStateInfo StateInfo => animator.GetCurrentAnimatorStateInfo(layerIndex);

        private bool CorrectAnimationIsPlaying => StateInfo.IsName(animationName);

        private bool AnimationIsDone => StateInfo.normalizedTime >= 1;

        public override bool keepWaiting => CorrectAnimationIsPlaying && !AnimationIsDone;
 

        public WaitForAnimCompleted(Animator animator, string animationName, int layerIndex = 0)
        {
            this.animator = animator;
            this.layerIndex = layerIndex;
            this.animationName = animationName;
        }
    }
    
    public class WaitForAnimHashCompleted : CustomYieldInstruction
    {
        private readonly int hash;

        private readonly Animator animator;
        private readonly int layerIndex;


        private AnimatorStateInfo StateInfo => animator.GetCurrentAnimatorStateInfo(layerIndex);

        private bool CorrectAnimationIsPlaying => StateInfo.shortNameHash == hash;

        private bool AnimationIsDone => StateInfo.normalizedTime >= 1;

        public override bool keepWaiting => CorrectAnimationIsPlaying && !AnimationIsDone;


        public WaitForAnimHashCompleted(Animator animator, int hash, int layerIndex = 0)
        {
            this.animator = animator;
            this.layerIndex = layerIndex;
            this.hash = hash;
        }
    }
    
    public class WaitForAnimationStarted : CustomYieldInstruction
    {
        private readonly string animationName;
        private readonly Animator animator;
        private readonly int layerIndex;


        private AnimatorStateInfo StateInfo => animator.GetCurrentAnimatorStateInfo(layerIndex);

        private bool CorrectAnimationIsPlaying => StateInfo.IsName(animationName);

        private bool AnimationStarted => StateInfo.normalizedTime > 0;

        public override bool keepWaiting => !CorrectAnimationIsPlaying || !AnimationStarted;

        
        public WaitForAnimationStarted(Animator animator, string animationName, int layerIndex = 0)
        {
            this.animator = animator;
            this.layerIndex = layerIndex;
            this.animationName = animationName;
        }
    }
}

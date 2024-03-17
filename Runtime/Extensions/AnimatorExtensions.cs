using UnityEngine;

namespace TheMazurkaStudio.Utilities.Animation
{
    public static class AnimatorExtensions
    {
        public static WaitForAnimCompleted PlayAndWait(this Animator animator, string animationName, int layer = 0)
        {
            animator.Play(animationName, layer);
            return new WaitForAnimCompleted(animator, animationName, layer);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace TheMazurkaStudio.Utilities.Animation
{
	/// <summary>
	///  Use to work with Animator and events. 
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class AnimatorHelper : MonoBehaviour
	{
		public event Action<string> AnimEventWasRaised;
		public event Action FootStepEvent;
		
		
		[SerializeField] protected string fallbackAnim = "Locomotion";
		[SerializeField] protected bool randomizeInitialStateTimeOffset;
		[SerializeField] protected UnityEvent<string> _animEventWasRaised;
		
		public Animator Animator { get; private set; }

		protected virtual void Awake()
		{
			Animator = GetComponent<Animator>();
			if(randomizeInitialStateTimeOffset) Animator.Play(0,-1, Random.value);
		}

		#region Parameters

		public void SetFloat(string floatName, float value) => Animator.SetFloat(floatName, value);
		public void SetFloat(int hash, float value) => Animator.SetFloat(hash, value);
		
		public void SetInteger(string intName, int value) => Animator.SetInteger(intName, value);
		public void SetInteger(int hash, int value) => Animator.SetInteger(hash, value);
		
		public void SetBoolToTrue(string boolName) => Animator.SetBool(boolName, true);
		public void SetBoolToFalse(string boolName) => Animator.SetBool(boolName, false);
		public void SetBool(string boolName, bool value) => Animator.SetBool(boolName, value);
		public void SetBool(int hash, bool value) => Animator.SetBool(hash, value);
		
		public void SetTrigger(string triggerName) => Animator.SetTrigger(triggerName);
		public void SetTrigger(int hash) => Animator.SetTrigger(hash);
		
		public void ResetTrigger(string triggerName) => Animator.SetTrigger(triggerName);
		public void ResetTrigger(int hash) => Animator.SetTrigger(hash);

		#endregion

		
		#region Play Animation

		public void PlayAnim(string animName, int layer = 0) => Animator.Play(animName, layer);
		public void PlayAnim(int hash, int layer = 0) => Animator.Play(hash, layer);
		
		public void PlayAnim(string animName, Action animEndCallback, int layer = 0) => StartCoroutine(WaitForAnimCompleted(animEndCallback, animName, layer));
		public void PlayAnim(int hash, Action animEndCallback, int layer = 0) => StartCoroutine(WaitForAnimCompleted(animEndCallback, hash, layer));

		/// <summary>
		/// Return to the base state of the animator (idle, locomotion ...)
		/// </summary>
		public void Fallback() => PlayAnim(fallbackAnim);
		
		#endregion

		
		#region Anim Events

		public virtual void RaiseEvent(string eventID)
		{
			_animEventWasRaised?.Invoke(eventID);
			AnimEventWasRaised?.Invoke(eventID);
		}

		public virtual void Footstep()
		{
			FootStepEvent?.Invoke();
		}

		#endregion


		#region Helpers

		private IEnumerator WaitForAnimCompleted(Action callback, string animName, int layer = 0)
		{
			Animator.Play(animName, layer);
			//Wait the end of frame to start tracking anim because coroutine start before anim update
			yield return new WaitForEndOfFrame(); 
			yield return new WaitForAnimCompleted(Animator, animName, layer); 
			callback?.Invoke();
		}
		
		private IEnumerator WaitForAnimCompleted(Action callback, int animHash, int layer = 0)
		{
			Animator.Play(animHash, layer);
			//Wait the end of frame to start tracking anim because coroutine start before anim update
			yield return new WaitForEndOfFrame(); 
			yield return new WaitForAnimHashCompleted(Animator, animHash, layer); 
			callback?.Invoke();
		}

		public bool IsAnimPlaying(string animNameValue) => Animator.GetCurrentAnimatorStateInfo(0).IsName(animNameValue);
		public bool IsAnimPlaying(int hash) => Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hash;

		#endregion
	}
}


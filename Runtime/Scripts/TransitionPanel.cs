using System;
using UnityEngine;

using jmayberry.CustomAttributes;
using System.Collections;

namespace jmayberry.SceneTransitions {

	[RequireComponent(typeof(Animator))]
	public class TransitionPanel : MonoBehaviour {
		protected Animator transitionAnimator;
		protected Action currentCallback;

		protected virtual void Awake() {
			this.transitionAnimator = GetComponent<Animator>();
		}

		public virtual IEnumerator FadeOut() {
			bool isFinished = false;
			this.FadeOut(() => isFinished = true);

			while (!isFinished) {
				yield return null;
			}
		}

		public virtual void FadeOut(Action callWhenFadeFinished) {
			currentCallback = callWhenFadeFinished;
			this.transitionAnimator.SetTrigger("fadeOut");
		}

		public virtual IEnumerator FadeIn() {
			bool isFinished = false;
			this.FadeIn(() => isFinished = true);

			while (!isFinished) {
				yield return null;
			}
		}

		public virtual void FadeIn(Action callWhenFadeFinished) {
			currentCallback = callWhenFadeFinished;
			this.transitionAnimator.SetTrigger("fadeIn");
		}

		// Called by TransitionPanel
		public virtual void FadeFinished() {
			if (this.currentCallback != null) {
				this.currentCallback.Invoke();
				this.currentCallback = null;
			}
		}
	}
}

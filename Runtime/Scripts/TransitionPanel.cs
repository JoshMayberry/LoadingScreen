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

        public virtual IEnumerator DoFade() {
            bool isFinished = false;
            this.DoFade(() => isFinished = true);

            while (!isFinished) {
                yield return null;
            }
        }

        public virtual void DoFade(Action callWhenFadeFinished) {
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

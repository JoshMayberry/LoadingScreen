using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using jmayberry.CustomAttributes;

namespace jmayberry.SceneTransitions {

	// Use [Unity Loading Screen - Easy Tutorial](https://www.youtube.com/watch?v=wvXDCPLO7T0)
	public class SceneTransitionManager : MonoBehaviour {
		public Animator transitionAnimator;
		public GameObject LoadingScreenContainer;
		public Image LoadingBarFill;

		public Func<bool>[] PreloadFunctions;

		[Readonly] public string sceneName;

        public static SceneTransitionManager instance { get; private set; }
		private void Awake() {
			if (instance != null) {
				Debug.LogError("Found more than one SceneTransitionManager in the scene.");
				Destroy(this.gameObject);
				return;
			}

			instance = this;
		}

        public virtual void FadeInScene(string sceneName) {
			this.sceneName = sceneName;

			if (this.transitionAnimator != null) {
				this.transitionAnimator.SetTrigger("fadeIn");
				return;
			}

			LoadScene();
		}

        // Called by animation
        public virtual void LoadScene() {
			StartCoroutine(LoadSceneAsync(sceneName));
		}

		protected virtual IEnumerator LoadSceneAsync(string sceneName) {
			// TODO: Inject functions that can be loaded?

            if ((LoadingScreenContainer == null) || (LoadingBarFill == null)) {
                SceneManager.LoadScene(this.sceneName);
                yield break;
            }

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

			LoadingScreenContainer.SetActive(true);

			while (!operation.isDone) {
				float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
				LoadingBarFill.fillAmount = progressValue;

				yield return null;
			}
		}
	}
}
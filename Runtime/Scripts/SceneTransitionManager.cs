using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using jmayberry.CustomAttributes;
using TMPro;
using System.Linq;
using System.Threading;

namespace jmayberry.SceneTransitions {
	// Use [Unity Loading Screen - Easy Tutorial](https://www.youtube.com/watch?v=wvXDCPLO7T0)
	public class SceneTransitionManager : MonoBehaviour {
		[Header("Setup")]
		public LoadingPanel loadingPanel;
		public TransitionPanel transitionPanel;
		public Canvas sceneTransitionCanvas;
		[Required] public string rootScene; // The main scene after the preload scene this one is in

		[Header("Tweak")]
		[SerializeField] internal protected bool loadingPanelOnlyForPreload = true;
		[SerializeField] internal protected bool persistOnLoad = true;

		[Header("Debug")]
		[Readonly] public string sceneName;
		public List<LoadOperation> preloadOperations = new List<LoadOperation>();

		public static SceneTransitionManager instance { get; private set; }
		protected virtual void Awake() {
			if (instance != null) {
				Debug.LogError("Found more than one SceneTransitionManager in the scene.");
				Destroy(this.gameObject);
				return;
			}

			instance = this;
			if (persistOnLoad) {
				DontDestroyOnLoad(gameObject);
				if (sceneTransitionCanvas != null) {
					DontDestroyOnLoad(sceneTransitionCanvas.gameObject);
				}
				else if ((loadingPanel != null) || (transitionPanel != null)) {
					Debug.LogError("*sceneTransitionCanvas* must be given if *persistOnLoad* is true and either *loadingPanel* or *transitionPanel* are given");
					loadingPanel = null;
					transitionPanel = null;
				}
			}
		}

		public virtual void LoadRoot() {
			this.LoadScene(rootScene);
		}

		public virtual void LoadScene(string sceneName=null) {
			StartCoroutine(LoadSceneAsync(sceneName));
		}

		public virtual IEnumerator LoadSceneAsync(string sceneName=null) {
			if (sceneName == null) {
				sceneName = this.sceneName;
			}

			if ((loadingPanelOnlyForPreload && (preloadOperations.Count == 0)) || !loadingPanel.HasLoadingBar()) {
				if (transitionPanel == null) {
					SceneManager.LoadScene(sceneName);
					yield break;
				}

				transitionPanel.FadeIn(() => {
					SceneManager.LoadScene(sceneName);
					if (persistOnLoad) {
						transitionPanel.FadeOut(null);
					}
				});
				yield break;
			}

			yield return loadingPanel.FadeIn();

			var loadSceneOperation = new LoadSceneOperation(sceneName);
			float totalWeight = preloadOperations.Sum(op => op.weight) + loadSceneOperation.weight;
			float cumulativeWeight = 0;

			List<LoadOperation> toRemove = new List<LoadOperation>();
			foreach (var item in preloadOperations) {
				yield return RunOperation(item, cumulativeWeight / totalWeight, totalWeight);
				cumulativeWeight += item.weight;

				if (item.isOneShot) {
					toRemove.Add(item);
				}
			}

			yield return RunOperation(loadSceneOperation, cumulativeWeight / totalWeight, totalWeight);

			if (persistOnLoad) {
				foreach (LoadOperation item in toRemove) {
					preloadOperations.Remove(item);
				}

				if (loadingPanel != null) {
					yield return loadingPanel.FadeOut();
				}

				if (transitionPanel != null) {
					yield return transitionPanel.FadeOut();
				}
			}
		}

		protected virtual IEnumerator RunOperation(LoadOperation loadOperation, float startProgress, float totalWeight) {
			loadingPanel.SetText((loadOperation.description != "") ? loadOperation.description : "Loading...");


			bool isFinished = false;
			Coroutine coroutine = StartCoroutine(loadOperation.Run(() => isFinished = true));

			float weight = (loadOperation.weight / totalWeight);
			while (!isFinished) {
				loadingPanel.SetFill(startProgress + Mathf.Clamp01(loadOperation.progress) * weight);
				yield return null;
			}

			if (!isFinished) {
				Debug.LogWarning("Coroutine did not finish when progress was 1");
			}
		}
	}

	public class LoadSceneOperation : LoadOperation {
		private string sceneToLoad;

		public LoadSceneOperation(string sceneName) {
			this.sceneToLoad = sceneName;
			this.description = "Loading Scene";
			this.weight = 0.1f;
		}

		public override IEnumerator Run(Action callWhenFinished) {
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
			operation.allowSceneActivation = false;

			while (operation.progress < 0.9f) {
				progress = Mathf.Clamp01(operation.progress / 0.9f);
				yield return null;
			}

			if (SceneTransitionManager.instance.transitionPanel != null) {
				yield return SceneTransitionManager.instance.transitionPanel.FadeIn();
			}

			operation.allowSceneActivation = true;
			while (!operation.isDone) {
				progress = Mathf.Clamp01(operation.progress / 0.9f);
				yield return null;
			}

			progress = 1f;
			callWhenFinished();
		}
	}
}
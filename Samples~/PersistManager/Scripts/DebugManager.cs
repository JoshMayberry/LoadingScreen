
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.SceneTransitions;
using UnityEngine.SceneManagement;

namespace jmayberry.SceneTransitions.Samples.PersistManager {
	public class DebugManager : MonoBehaviour {
		void Start() {
			if (SceneManager.GetActiveScene().name == "PersistIntro") {
				SceneTransitionManager.instance.preloadOperations.Add(new MockLoadOperation());
			}
		}

		public void OnLoadNextScene() {
			SceneTransitionManager.instance.LoadScene(SceneManager.GetActiveScene().name == "PersistScene1" ? "PersistScene2" : "PersistScene1");
		}
	}
}
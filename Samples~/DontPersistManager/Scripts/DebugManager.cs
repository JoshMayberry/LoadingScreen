using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.SceneTransitions;
using UnityEngine.SceneManagement;

namespace jmayberry.SceneTransitions.Samples.DontPersistManager {
	public class DebugManager : MonoBehaviour {
		void Start() {
			SceneTransitionManager.instance.preloadOperations.Add(new MockLoadOperation());
		}

		public void OnLoadNextScene() {
			SceneTransitionManager.instance.LoadScene(SceneManager.GetActiveScene().name == "DontPersistScene1" ? "DontPersistScene2" : "DontPersistScene1");
		}
	}
}

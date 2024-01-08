using UnityEngine;

namespace jmayberry.SceneTransitions {
	public class TransitionPanel : MonoBehaviour {
		// Called by animation
		public void GoToScene() {
			SceneTransitionManager.instance.LoadScene();
        }
	}
}

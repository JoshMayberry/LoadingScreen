using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.SceneTransitions;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour {
    void Start() {
        SceneTransitionManager.instance.preloadOperations.Add(new MockLoadOperation());
    }

    public void OnLoadNextScene() {
        SceneTransitionManager.instance.LoadScene(SceneManager.GetActiveScene().name == "Scene1" ? "Scene2" : "Scene1");
    }
}

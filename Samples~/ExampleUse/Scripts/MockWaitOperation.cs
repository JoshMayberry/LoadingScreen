using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.SceneTransitions;
using System;

public class MockLoadOperation : LoadOperation {
    public MockLoadOperation() {
        this.description = "Testing";
    }

    public override IEnumerator Run(Action callWhenFinished) {
        // Simulate a task that takes a few seconds to complete
        for (int i = 0; i < 4; i++) {
            yield return new WaitForSeconds(1); // Wait for 1 second
            progress += 0.25f; // Increment progress by 25% each second
        }

        callWhenFinished();
    }
}

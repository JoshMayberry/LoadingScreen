using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jmayberry.SceneTransitions {
	[Serializable]
	public abstract class LoadOperation {
		public bool isOneShot;
		public string description;
		public float weight = 1; // How much of the progress bar it fills up
		[Range(0,1)] public float progress;

		public abstract IEnumerator Run(Action callWhenFinished);
	}
}

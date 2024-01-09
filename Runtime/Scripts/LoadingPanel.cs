using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using jmayberry.CustomAttributes;
using TMPro;
using System.Linq;

namespace jmayberry.SceneTransitions {
	public class LoadingPanel : MonoBehaviour {
		[Header("Setup")]
		[SerializeField] protected GameObject LoadingScreenContainer;
		[SerializeField] protected Image LoadingBackground;
		[SerializeField] protected Image LoadingBarBorder;
		[SerializeField] protected Image LoadingBarFill;
		[SerializeField] protected TMP_Text LoadingText;

		[Header("Tweak")]
		[SerializeField] protected float fadeInLoadTime = 1f;

        private void Start() {
			SetAlpha(this.LoadingBackground, 0);
			SetAlpha(this.LoadingBarBorder, 0);
			SetAlpha(this.LoadingBarFill, 0);
			SetAlpha(this.LoadingText, 0);
            LoadingScreenContainer.SetActive(false);
        }

        public bool HasLoadingBar() {
			return (LoadingScreenContainer != null) && (LoadingBackground != null) && (LoadingBarFill != null);
        }

        internal IEnumerator Show() {
            LoadingBarFill.fillAmount = 0;
            LoadingScreenContainer.SetActive(true);

            yield return LerpOpacity(0, 1);
        }

        internal IEnumerator Hide() {
            yield return LerpOpacity(1, 0);
            LoadingScreenContainer.SetActive(false);
        }

        protected virtual IEnumerator LerpOpacity(float initialOpacity, float targetOpacity) {
			yield return doOverTime(this.fadeInLoadTime, (float progress) => {
				float currentAlpha = Mathf.Lerp(initialOpacity, targetOpacity, progress);
				SetAlpha(this.LoadingBackground, currentAlpha);
				SetAlpha(this.LoadingBarBorder, currentAlpha);
				SetAlpha(this.LoadingBarFill, currentAlpha);
				SetAlpha(this.LoadingText, currentAlpha);
			});
		}

		private void SetAlpha(Graphic graphic, float alpha) {
			if (graphic == null) {
				return;
			}

			Color color = graphic.color;
			color.a = alpha;
			graphic.color = color;
		}

		protected virtual IEnumerator doOverTime(float duration, Action<float> OnProgressMade) {
			if (duration <= 0) {
				OnProgressMade(1);
				yield break;
			}

			float startTime = Time.time;
			float progress = 0;
			while (progress < 1) {
				float timeElapsed = Time.time - startTime;
				progress = Mathf.Clamp01(timeElapsed / duration);
				OnProgressMade(progress);
				yield return null;
			}
		}

        internal void SetText(string value) {
            if (LoadingText != null) {
                LoadingText.text = value;
            }
        }

        internal void SetFill(float value) {
			if (LoadingBarFill != null) {
                LoadingBarFill.fillAmount = value;
            }
        }
    }
}

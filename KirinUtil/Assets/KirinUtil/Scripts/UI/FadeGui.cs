using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil {
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeGui : MonoBehaviour {

        //private FadeType fadeType;
        private float fadeAlpha;

        #region FadeOutGUI
        public void FadeOut(float time, float delay, float alpha, iTween.EaseType easetype = iTween.EaseType.easeOutCubic) {

            gameObject.SetActive(true);
            //fadeType = FadeType.FadeOut;
            fadeAlpha = alpha;

            iTween.ValueTo(gameObject,
                iTween.Hash(
                    "from", 1,
                    "to", fadeAlpha,
                    "time", time,
                    "delay", delay,
                    "easetype", easetype,
                    "onUpdate", "FadeOutGUIUpdate",
                    "oncomplete", "FadeOutGUIComplete"
                )
            );

        }

        private void FadeOutGUIUpdate(float fade) {
            gameObject.GetComponent<CanvasGroup>().alpha = fade;
        }

        private void FadeOutGUIComplete() {
            iTween.Stop(gameObject);
            gameObject.SetActive(false);
            gameObject.GetComponent<CanvasGroup>().alpha = fadeAlpha;
        }
        #endregion

        #region FadeInGUI
        public void FadeIn(float time, float delay, float alpha = 1, iTween.EaseType easetype = iTween.EaseType.easeOutCubic) {

            gameObject.SetActive(true);
            gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
            //fadeType = FadeType.FadeIn;
            fadeAlpha = alpha;

            iTween.ValueTo(gameObject,
                iTween.Hash(
                    "from", 0.0f,
                    "to", fadeAlpha,
                    "time", time,
                    "delay", delay,
                    "easetype", "easeOutCubic",
                    "onUpdate", "FadeInGUIUpdate"
                )
            );

        }

        private void FadeInGUIUpdate(float fade) {
            gameObject.GetComponent<CanvasGroup>().alpha = fade;
        }

        private void FadeInGUIComplete() {
            iTween.Stop(gameObject);
            gameObject.GetComponent<CanvasGroup>().alpha = fadeAlpha;
        }

        #endregion

        private void OnDisable() {
            iTween.Stop(gameObject);
            gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        }

        private void OnDestroy() {
            iTween.Stop(gameObject);
        }
    }
}

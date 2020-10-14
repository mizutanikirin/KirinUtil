using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeGui : MonoBehaviour
    {

        //private FadeType fadeType;

        #region FadeOutGUI
        public void FadeOut(float time, float delay, iTween.EaseType easetype = iTween.EaseType.easeOutCubic)
        {

            iTween.Stop(gameObject, "value");
            gameObject.SetActive(true);
            //fadeType = FadeType.FadeOut;

            iTween.ValueTo(gameObject,
                iTween.Hash(
                    "from", 1f,
                    "to", 0f,
                    "time", time,
                    "delay", delay,
                    "easetype", easetype,
                    "onUpdate", "FadeOutGUIUpdate",
                    "oncomplete", "FadeOutGUIComplete"
                )
            );

        }

        private void FadeOutGUIUpdate(float fade)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = fade;
        }

        private void FadeOutGUIComplete()
        {
            iTween.Stop(gameObject);
            gameObject.SetActive(false);
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }
        #endregion

        #region FadeInGUI
        public void FadeIn(float time, float delay, iTween.EaseType easetype = iTween.EaseType.easeOutCubic)
        {

            iTween.Stop(gameObject, "value");
            gameObject.SetActive(true);
            gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
            //fadeType = FadeType.FadeIn;

            iTween.ValueTo(gameObject,
                iTween.Hash(
                    "from", 0.0f,
                    "to", 1f,
                    "time", time,
                    "delay", delay,
                    "easetype", "easeOutCubic",
                    "onUpdate", "FadeInGUIUpdate"
                )
            );

        }

        private void FadeInGUIUpdate(float fade)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = fade;
        }


        #endregion

        private void OnDisable()
        {
            iTween.Stop(gameObject);
            gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        }

        private void OnDestroy()
        {
            iTween.Stop(gameObject);
        }
    }
}

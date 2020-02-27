using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace KirinUtil {
    public class TextToSlider:MonoBehaviour {

        private string _text;


        public void EndEdit(string text) {
            this._text = text;
        }

        public void ValueToSlider(Slider slider) {
            slider.value = float.Parse(this._text);
        }

    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace KirinUtil {
    public class SliderToText:MonoBehaviour {

        private float _sliderValue;

        public float sliderValue {
            get {
                return this._sliderValue;
            }
            set {
                this._sliderValue = value;
            }
        }

        public void ValueToText(InputField input) {
            input.text = this._sliderValue.ToString();
        }

    }
}

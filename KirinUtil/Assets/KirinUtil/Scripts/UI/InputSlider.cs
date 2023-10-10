using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace KirinUtil {
    public class InputSlider:MonoBehaviour {

        [System.NonSerialized] public float value;
        private InputField input;
        private Slider slider;

        private void Start() {
            input = gameObject.GetComponentInChildren<InputField>();
            slider = gameObject.GetComponent<Slider>();
            input.text = slider.value.ToString();
            value = slider.value;
        }

        public void InputEndEdit(string valueStr) {
            if (valueStr == "") return;
            
            slider.value = float.Parse(valueStr);
            value = slider.value;
        }

        public void MoveSlider(float sliderValue) {
            input.text = sliderValue.ToString();
            value = sliderValue;
        }

    }
}
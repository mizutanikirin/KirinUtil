using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace KirinUtil {
    public class ToggleBtn:MonoBehaviour {

        public Sprite onTexture;
        public Sprite offTexture;

        public bool isON = false;

        [SerializeField]
        private UnityEvent OnEvent = new UnityEvent();

        [SerializeField]
        private UnityEvent OffEvent = new UnityEvent();

        // Use this for initialization
        void Awake() {
            SetToggle(isON);
        }

        void OnEnable() {
            OnEvent.AddListener(ChangeOn);
            OffEvent.AddListener(ChangeOff);
        }

        void OnDisable() {
            OnEvent.RemoveListener(ChangeOn);
            OffEvent.RemoveListener(ChangeOff);
        }

        void ChangeOn() {
            Debug.LogFormat("ChangeOn");
        }

        void ChangeOff() {
            Debug.LogFormat("ChangeOff");
        }

        public void Switch() {
            isON = !isON;
            SetToggle(isON);
        }

        public void SetToggle(bool thisON) {
            if (thisON)
                gameObject.GetComponent<Image>().sprite = onTexture;
            else
                gameObject.GetComponent<Image>().sprite = offTexture;

            isON = thisON;

            if (isON)
                OnEvent.Invoke();
            else
                OffEvent.Invoke();
        }
    }
}
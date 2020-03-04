using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace KirinUtil {

    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class ToggleButton:MonoBehaviour {

        public Texture2D onTexture;
        public Texture2D offTexture;
        private Sprite onSprite = null;
        private Sprite offSprite = null;

        private bool isON = false;

        [SerializeField]
        private UnityEvent OnEvent = new UnityEvent();

        [SerializeField]
        private UnityEvent OffEvent = new UnityEvent();


        private void Start() {
            onSprite = Sprite.Create(onTexture, new Rect(0.0f, 0.0f, onTexture.width, onTexture.height), Vector2.zero);
            Util.image.UnloadTexture(onTexture); 
            offSprite = Sprite.Create(offTexture, new Rect(0.0f, 0.0f, offTexture.width, offTexture.height), Vector2.zero);
            Util.image.UnloadTexture(offTexture);

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
            //print("ChangeOn");
        }

        void ChangeOff() {
            //print("ChangeOff");
        }

        public void Switch() {
            isON = !isON;
            SetToggle(isON);
        }

        public void SetToggle(bool thisON) {
            isON = thisON;

            if (isON) {
                gameObject.GetComponent<Image>().sprite = onSprite;
                OnEvent.Invoke();
            } else {
                gameObject.GetComponent<Image>().sprite = offSprite;
                OffEvent.Invoke();
            }
        }

        public bool IsOn() {
            return isON;
        }
    }
}
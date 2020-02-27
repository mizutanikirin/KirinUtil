using UnityEngine;
using System.Collections;


namespace KirinUtil {
    public class StartMultiDisplay:MonoBehaviour {

        public int maxDisplayCount = 2;

        void Start() {
            for (int i = 0; i < maxDisplayCount && i < Display.displays.Length; i++) {
                Display.displays[i].Activate();
            }
        }
    }
}
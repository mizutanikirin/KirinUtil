//--------------------------------------------------------------------------
//  DropDownのTemplateにアタッチして使う
//  デフォのDropDownだとスクロールの初期位置が一番上に行くが、
//  これを使うと選択した位置にスクロールしている。
//--------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KirinUtil {
    public class SetDropDownScrollPosition : MonoBehaviour {

        private ScrollRect sr;

        public void Start() {
            sr = gameObject.GetComponent<ScrollRect>();

            GameObject parentObj = gameObject.transform.parent.gameObject;
            Dropdown dropdown = parentObj.GetComponent<Dropdown>();

            float scrollPosition = 1 - dropdown.value / (float)dropdown.options.Count;
            sr.normalizedPosition = new Vector2(0f, scrollPosition);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {
    public class DialogManager : MonoBehaviour {

        public GameObject dialogUIPrefab;
        public GameObject parentObj;
        public bool bgOn;
        public float toastTime;

        public enum ButtonType {
            None, YesNo, OK
        }

        private List<string> idList = new List<string>();
        private List<GameObject> dialogUIList = new List<GameObject>();
        private List<GameObject> yesBtnList = new List<GameObject>();
        private List<GameObject> noBtnList = new List<GameObject>();
        private List<GameObject> okBtnList = new List<GameObject>();


        //----------------------------------
        //  event
        //----------------------------------
        #region event
        [System.Serializable] public class YesClickEvent : UnityEngine.Events.UnityEvent<string> { }
        [System.Serializable] public class NoClickEvent : UnityEngine.Events.UnityEvent<string> { }
        [System.Serializable] public class OkClickEvent : UnityEngine.Events.UnityEvent<string> { }

        [SerializeField] private YesClickEvent yesClickEvent = new YesClickEvent();
        [SerializeField] private NoClickEvent noClickEvent = new NoClickEvent();
        [SerializeField] private OkClickEvent okClickEvent = new OkClickEvent();

        void OnEnable() {
            yesClickEvent.AddListener(YesClick);
            noClickEvent.AddListener(NoClick);
            okClickEvent.AddListener(OkClick);
        }

        void OnDisable() {
            yesClickEvent.RemoveListener(YesClick);
            noClickEvent.RemoveListener(NoClick);
            okClickEvent.RemoveListener(OkClick);
        }

        void YesClick(string id) {
        }

        void NoClick(string id) {
        }

        void OkClick(string id) {
        }
        #endregion

        //----------------------------------
        //  Dialog作成
        //----------------------------------
        // YesNoボタンがあるDialog
        public void Popup(string id, Vector2 pos, string message, ButtonType btnType) {
            GameObject dialogUI = Util.media.CreateUIObj(dialogUIPrefab, parentObj, "DialogUI", Vector3.zero, Vector3.zero, Vector3.one);

            // 位置調整
            GameObject dialogWindowObj = dialogUI.transform.Find("dialogWindow").gameObject;
            dialogWindowObj.transform.localPosition = new Vector3(pos.x, pos.y, 0);

            // アニメーション
            Util.Scale(dialogWindowObj, 0.01f);
            iTween.ScaleTo(dialogWindowObj,
                iTween.Hash(
                    "x", 1,
                    "y", 1,
                    "z", 1,
                    "time", 0.5f,
                    "islocal", true,
                    "EaseType", iTween.EaseType.easeOutBounce
                )
            );

            // bg
            Transform bgTrf = dialogUI.transform.Find("bg");
            if (bgTrf != null) {
                GameObject bgObj = bgTrf.gameObject;
                if (bgOn) {
                    Util.media.SetUISize(bgObj, new Vector2(Screen.width, Screen.height));
                    Util.media.FadeInUI(bgObj, 0.5f, 0);
                } else {
                    bgObj.SetActive(false);
                }
            }

            // ボタンイベント
            GameObject NoneUI = dialogWindowObj.transform.Find("typeNone").gameObject;
            GameObject OkUI = dialogWindowObj.transform.Find("typeOk").gameObject;
            GameObject YesNoUI = dialogWindowObj.transform.Find("typeYesNo").gameObject;

            Transform okTrf = OkUI.transform.Find("okBtn");
            Transform yesTrf = YesNoUI.transform.Find("yesBtn");
            Transform noTrf = YesNoUI.transform.Find("noBtn");

            if (btnType == ButtonType.None) {
                NoneUI.SetActive(true);
                OkUI.SetActive(false);
                YesNoUI.SetActive(false);
            } else if (btnType == ButtonType.OK) {
                NoneUI.SetActive(false);
                OkUI.SetActive(true);
                YesNoUI.SetActive(false);

                if (okTrf != null) okTrf.gameObject.GetComponent<Button>().onClick.AddListener(() => OkBtnClick(id));
            } else {
                NoneUI.SetActive(false);
                OkUI.SetActive(false);
                YesNoUI.SetActive(true);

                if (yesTrf != null) yesTrf.gameObject.GetComponent<Button>().onClick.AddListener(() => YesBtnClick(id));
                if (noTrf != null) noTrf.gameObject.GetComponent<Button>().onClick.AddListener(() => NoBtnClick(id));
            }

            Transform closeTrf = dialogWindowObj.transform.Find("closeBtn");
            if (closeTrf != null) {
                if (btnType == ButtonType.None) closeTrf.gameObject.SetActive(false);
                else closeTrf.gameObject.SetActive(true);
                closeTrf.gameObject.GetComponent<Button>().onClick.AddListener(() => CloseBtnClick(id));
            }

            // message
            Transform messageTrf;
            if (btnType == ButtonType.None) messageTrf = NoneUI.transform.Find("messageText");
            else if (btnType == ButtonType.OK) messageTrf = OkUI.transform.Find("messageText");
            else messageTrf = YesNoUI.transform.Find("messageText");
            if (messageTrf != null) messageTrf.gameObject.GetComponent<Text>().text = message;

            // listに登録
            idList.Add(id);
            dialogUIList.Add(dialogUI);
            if(yesTrf != null) yesBtnList.Add(yesTrf.gameObject);
            else yesBtnList.Add(null);
            if (noTrf != null) noBtnList.Add(noTrf.gameObject);
            else noBtnList.Add(null);
            if (okTrf != null) okBtnList.Add(okTrf.gameObject);
            else noBtnList.Add(null);

            // none
            if (btnType == ButtonType.None) StartCoroutine(NoneWait(id));
        }

        private IEnumerator NoneWait(string id) {
            yield return new WaitForSeconds(toastTime);
            CloseDialog(id);
        }

        //----------------------------------
        //  btn
        //----------------------------------
        #region btn
        private void YesBtnClick(string id) {
            yesClickEvent.Invoke(id);
            CloseDialog(id);
        }

        private void NoBtnClick(string id) {
            noClickEvent.Invoke(id);
            CloseDialog(id);
        }

        private void OkBtnClick(string id) {
            okClickEvent.Invoke(id);
            CloseDialog(id);
        }

        private void CloseBtnClick(string id) {
            CloseDialog(id);
        }
        #endregion

        //----------------------------------
        //  function
        //----------------------------------
        private void CloseDialog(string id) {
            int listNum = Util.GetListNum(idList, id);
            Destroy(dialogUIList[listNum]);

            idList.RemoveAt(listNum);
            dialogUIList.RemoveAt(listNum);
            yesBtnList.RemoveAt(listNum);
            noBtnList.RemoveAt(listNum);
            okBtnList.RemoveAt(listNum);
        }
    }
}

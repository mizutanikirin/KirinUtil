using KirinUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {

    public class BalloonMessageManager : MonoBehaviour {

        public GameObject balloonPrefab;
        public Vector2 balloonMargin;

        [System.SerializableAttribute]
        public class InfoList {
            public List<Info> info = new List<Info>();

            public InfoList(List<Info> list) {
                info = list;
            }
        }
        [SerializeField]
        private List<InfoList> infoList = new List<InfoList>();

        public enum playMode {
            Start, Playing, End
        }
        private List<int> idList;
        private List<List<GameObject>> balloonObjList;
        private List<List<Vector3>> initPosList;
        private List<List<float>> startTimeList;
        private List<List<float>> timeList;
        private List<List<Timer>> timerList;
        private List<List<playMode>> isPlayModeList;
        private List<List<string>> soundFileList;
        private List<bool> isPlayList;
        private List<float> playTimeList;
        private List<List<int>> typeList;

        private List<List<Coroutine>> messageDisplayCoroutine;
        private List<List<Coroutine>> flashingMessageCoroutine;
        private bool inited = false;

        [Serializable]
        public class Info {
            public Type type;
            public Alignment alignment;
            public int fontSize;
            public Vector2 pos;
            public float startTime;
            public float time;
            public string soundPath;
            public string message;
        }

        public enum Type {
            TopLeft = 0, TopCenter = 1, TopRight = 2, 
            RightTop = 3, RightCenter = 4, RightBottom = 5,
            BottomRight = 6, BottomCenter = 7, BottomLeft = 8,
            LeftBottom = 9, LeftCenter = 10, LeftTop = 11
        }

        public enum Alignment {
            Left, Center
        }

        // Start is called before the first frame update
        void Start() {
            if(!inited) Init();
        }

        private void Init() {
            idList = new List<int>();
            isPlayList = new List<bool>();
            balloonObjList = new List<List<GameObject>>();
            initPosList = new List<List<Vector3>>();
            startTimeList = new List<List<float>>();
            timeList = new List<List<float>>();
            timerList = new List<List<Timer>>();
            playTimeList = new List<float>();
            isPlayModeList = new List<List<playMode>>();
            typeList = new List<List<int>>();
            soundFileList = new List<List<string>>();

            messageDisplayCoroutine = new List<List<Coroutine>>();
            flashingMessageCoroutine = new List<List<Coroutine>>();

            inited = true;
        }

        //----------------------------------
        //  init
        //----------------------------------
        public void Create(GameObject parentObj, int id) {
            // id登録
            if (!inited) Init();
            idList.Add(id);
            isPlayList.Add(false);
            playTimeList.Add(0);

            List<Vector3> thisInitPosList = new List<Vector3>();
            List<float> thisStartTimeList = new List<float>();
            List<float> thisTimeList = new List<float>();
            List<Timer> thisTimerList = new List<Timer>();
            List<GameObject> thisObjList = new List<GameObject>();
            List<playMode> thisPlayList = new List<playMode>();
            List<int> thisTypeList = new List<int>();
            List<Coroutine> thisCoroutine0 = new List<Coroutine>();
            List<Coroutine> thisCoroutine1 = new List<Coroutine>();
            List<string> thisSoundFileList = new List<string>();

            for (int i = 0; i < infoList[id].info.Count; i++) {
                // 情報を取得/登録
                //List<string> infoValue = GetInfoValue(infoList[i]);
                int type = (int)infoList[id].info[i].type;
                int alignment = (int)infoList[id].info[i].alignment;
                int fontSize = infoList[id].info[i].fontSize;
                Vector3 pos = infoList[id].info[i].pos;
                float startTime = infoList[id].info[i].startTime;
                float time = infoList[id].info[i].time;
                string soundFilePath;
                if (infoList[id].info[i].soundPath == "") soundFilePath = "";
                else soundFilePath = Application.dataPath + infoList[id].info[i].soundPath;
                string message = Util.GetLineText(infoList[id].info[i].message);

                thisInitPosList.Add(pos);
                thisStartTimeList.Add(startTime);
                thisTimeList.Add(time);
                thisPlayList.Add(playMode.Start);
                thisCoroutine0.Add(null);
                thisCoroutine1.Add(null);
                thisTypeList.Add(type);

                Timer timer = new Timer();
                timer.LimitTime = time;
                thisTimerList.Add(timer);
                thisSoundFileList.Add(soundFilePath);

                // オブジェクト作成
                GameObject thisObj = Util.media.CreateUIObj(balloonPrefab, parentObj, "balloon" + i, pos, Vector3.zero, Vector3.one);
                GameObject thiBgObj = thisObj.transform.Find("bg").gameObject;
                Text thisText = thisObj.GetComponentInChildren<Text>();
                if (alignment == 1) thisText.alignment = TextAnchor.MiddleCenter;
                else thisText.alignment = TextAnchor.MiddleLeft;
                thisText.fontSize = fontSize;
                thisText.text = message;
                thisText.GetComponent<RectTransform>().sizeDelta = new Vector2(thisText.preferredWidth, thisText.preferredHeight);
                thisObj.SetActive(false);
                thisObjList.Add(thisObj);

                // 大きさ調整
                Vector2 bgSize = new Vector2(thisText.preferredWidth + balloonMargin.x, thisText.preferredHeight + balloonMargin.y);
                thiBgObj.GetComponent<RectTransform>().sizeDelta = bgSize;


                // しっぽ表示/位置調整
                List<GameObject> tailObjList = Util.media.GetAllGameObject(thisObj.transform.Find("typeUI").gameObject);
                for (int j = 0; j < tailObjList.Count; j++) {
                    tailObjList[j].SetActive(false);
                }
                if (type == 0 || type == 1 || type == 2) {
                    // 上
                    tailObjList[0].SetActive(true);

                    // 位置
                    if (type == 0) Util.LocalPosX(tailObjList[0], -thisText.preferredWidth * 0.5f);
                    else if (type == 2) Util.LocalPosX(tailObjList[0], thisText.preferredWidth * 0.5f);

                    Util.LocalPosY(tailObjList[0], bgSize.y * 0.5f + tailObjList[0].GetComponent<RectTransform>().sizeDelta.y * 0.5f);  // - 8fはドロップシャドウの幅
                } else if (type == 3 || type == 4 || type == 5) {
                    // 右
                    tailObjList[1].SetActive(true);
                    if (type == 3) Util.LocalPosY(tailObjList[1], thisText.preferredHeight * 0.5f);
                    else if (type == 5) Util.LocalPosY(tailObjList[1], -thisText.preferredHeight * 0.5f);

                    Util.LocalPosX(tailObjList[1], bgSize.x * 0.5f + tailObjList[1].GetComponent<RectTransform>().sizeDelta.x * 0.5f);
                } else if (type == 6 || type == 7 || type == 8) {
                    // 下
                    tailObjList[2].SetActive(true);
                    if (type == 6) Util.LocalPosX(tailObjList[2], thisText.preferredWidth * 0.5f);
                    else if (type == 8) Util.LocalPosX(tailObjList[2], -thisText.preferredWidth * 0.5f);

                    Util.LocalPosY(tailObjList[2], -bgSize.y * 0.5f - tailObjList[2].GetComponent<RectTransform>().sizeDelta.y * 0.5f);
                } else if (type == 9 || type == 10 || type == 11) {
                    // 左
                    tailObjList[3].SetActive(true);
                    if (type == 9) Util.LocalPosY(tailObjList[3], -thisText.preferredHeight * 0.5f);
                    else if (type == 11) Util.LocalPosY(tailObjList[3], thisText.preferredHeight * 0.5f);

                    Util.LocalPosX(tailObjList[3], -bgSize.x * 0.5f - tailObjList[3].GetComponent<RectTransform>().sizeDelta.x * 0.5f);
                }
            }

            // 情報を登録
            initPosList.Add(thisInitPosList);
            startTimeList.Add(thisStartTimeList);
            timeList.Add(thisTimeList);
            timerList.Add(thisTimerList);
            balloonObjList.Add(thisObjList);
            isPlayModeList.Add(thisPlayList);
            typeList.Add(thisTypeList);
            messageDisplayCoroutine.Add(thisCoroutine0);
            flashingMessageCoroutine.Add(thisCoroutine1);
            soundFileList.Add(thisSoundFileList);

        }

        //----------------------------------
        //  コントロール
        //----------------------------------
        public void PlayAll(int id) {

            isPlayList[id] = true;

            // init
            playTimeList[id] = 0;
            for (int i = 0; i < balloonObjList[id].Count; i++) {
                balloonObjList[id][i].SetActive(false);
                balloonObjList[id][i].transform.localPosition = initPosList[id][i];
                timerList[id][i] = new Timer();
                timerList[id][i].LimitTime = timeList[id][i];
                isPlayModeList[id][i] = playMode.Start;
            }
        }


        public void StopAll(int id) {
            StopMessage(id);
            isPlayList[id] = false;
            for (int i = 0; i < balloonObjList[id].Count; i++) {
                balloonObjList[id][i].SetActive(false);
            }
        }

        public void Play(int id, int balloonNum) {
            if (isPlayList[id]) return;
            PlayOneBalloon(id, balloonNum);
        }

        public void Stop(int id, int balloonNum) {
            if (isPlayList[id]) return;
            StopOneBalloon(id, balloonNum);
        }

        // Update is called once per frame
        void Update() {
            for (int i = 0; i < idList.Count; i++) {
                if (isPlayList[i]) {
                    TimerUpdate(i);
                }
            }
        }

        private void TimerUpdate(int idListNum) {
            playTimeList[idListNum] += Time.deltaTime;
            //print("---------------TimerUpdate: " + playTimeList[idListNum]);

            for (int i = 0; i < timerList[idListNum].Count; i++) {
                if (playTimeList[idListNum] > startTimeList[idListNum][i]) {

                    // 再生スタート
                    if (isPlayModeList[idListNum][i] == playMode.Start) {
                        isPlayModeList[idListNum][i] = playMode.Playing;
                        PlayOneBalloon(idListNum, i);
                    }

                    // 再生中
                    /*if (isPlayModeList[idListNum][i] == playMode.Playing) {
                        if (timerList[idListNum][i].Update()) {
                            // 再生終了
                            isPlayModeList[idListNum][i] = playMode.End;
                            //StopOneBalloon(idListNum, i);
                        }
                    }*/

                }
            }
        }

        private void PlayOneBalloon(int idListNum, int balloonNum) {
            balloonObjList[idListNum][balloonNum].SetActive(true);

            int type = typeList[idListNum][balloonNum];
            string direction;
            if (type == 0 || type == 1 || type == 2) direction = "up";
            else if (type == 3 || type == 4 || type == 5) direction = "right";
            else if (type == 6 || type == 7 || type == 8) direction = "down";
            else direction = "left";

            MessageDisplay(balloonObjList[idListNum][balloonNum], direction, idListNum, balloonNum, timeList[idListNum][balloonNum]);
            Util.sound.LoadAndPlay(soundFileList[idListNum][balloonNum], 1, "balloon_" + idListNum + "_" + balloonNum);
        }

        private void StopOneBalloon(int idListNum, int balloonNum) {
            balloonObjList[idListNum][balloonNum].SetActive(false);
        }

        //----------------------------------
        //  message
        //----------------------------------
        #region message1
        private void MessageDisplay(GameObject obj, string direction, int idListNum, int balloonNum, float time = -1) {
            messageDisplayCoroutine[idListNum][balloonNum] = StartCoroutine(MessageDisplayWait(obj, direction, time, idListNum, balloonNum));
        }

        private void StopMessage(int idListNum) {
            //print(idListNum + ": " + messageDisplayCoroutine.Count);

            for (int i = 0; i < messageDisplayCoroutine[idListNum].Count; i++) {
                Util.sound.StopOneShotSound("balloon_" + idListNum + "_" + i);

                if (messageDisplayCoroutine[idListNum][i] != null) StopCoroutine(messageDisplayCoroutine[idListNum][i]);
                if (flashingMessageCoroutine[idListNum][i] != null) StopCoroutine(flashingMessageCoroutine[idListNum][i]);
                messageDisplayCoroutine[idListNum][i] = null;
                flashingMessageCoroutine[idListNum][i] = null;
            }
        }

        private IEnumerator MessageDisplayWait(GameObject obj, string direction, float time, int idListNum, int balloonNum) {
            Direction direct0;
            Direction direct1;
            if (direction == "up") {
                direct0 = Direction.Up;
                direct1 = Direction.Down;
            } else if (direction == "right") {
                direct0 = Direction.Right;
                direct1 = Direction.Left;
            } else if (direction == "down") {
                direct0 = Direction.Down;
                direct1 = Direction.Up;
            } else {
                direct0 = Direction.Left;
                direct1 = Direction.Right;
            }

            Util.media.UIDisplay(obj, FadeType.FadeIn, direct0);

            //yield return new WaitForSeconds(2.0f); // TODO: 必要ないかも？

            if (time != -1) {
                yield return new WaitForSeconds(time);
                Util.media.UIDisplay(obj, FadeType.FadeOut, direct1);
            }
        }

        #endregion

        //----------------------------------
        //  function
        //----------------------------------
        /*private List<string> GetInfoValue(string infoStr) {
            List<string> infoValue = new List<string>();

            string[] infoAry = infoStr.Split(","[0]);
            for (int i = 0; i < infoAry.Length; i++) {
                infoValue.Add(infoAry[i]);
            }

            return infoValue;
        }*/
    }
}

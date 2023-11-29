using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {
    public class CountDown : MonoBehaviour {

        private List<CountData> countDatas = new List<CountData>();

        [System.Serializable]
        public class CountData
        {
            public string id;
            public bool timerFin;
            public float timerTime;
            public int limitTime;
            public Text timerText;
            public TextMeshProUGUI timerTMP;
            public int soundSENum;
            public int soundPreNum;
            public bool zeroSoundPlay;
            public AddText addText;
            public int currentTime;
        }


        [System.Serializable]
        public class AddText
        {
            public string before;
            public string after;
        }

        //----------------------------------
        //  SetCountDown
        //----------------------------------

        #region カウントダウンのみ
        public void Set(int thisLimitTime, string id="Main")
        {
            CountData data = new CountData();
            data.id = id;
            data.timerText = null;
            data.timerTMP = null;
            data.soundSENum = -1;
            data.soundPreNum = -1;
            data.zeroSoundPlay = false;

            data.addText = new AddText();
            data.addText.before = "";
            data.addText.after = "";

            SetupData(data, thisLimitTime);
        }
        #endregion

        #region カウントダウン + text
        public void Set(
            int thisLimitTime, Text thisTimerText, string beforeText = "", 
            string afterText = "", string id = "Main")
        {
            CountData data = new CountData();
            data.id = id;
            data.timerText = thisTimerText;
            data.timerTMP = null;
            data.soundSENum = -1;
            data.soundPreNum = -1;
            data.zeroSoundPlay = false;

            data.addText = new AddText();
            data.addText.before = beforeText;
            data.addText.after = afterText;

            SetupData(data, thisLimitTime);
        }

        public void Set(
            int thisLimitTime, TextMeshProUGUI thisTimerText, string beforeText = "", 
            string afterText = "", string id = "Main")
        {
            CountData data = new CountData();
            data.id = id;
            data.timerText = null;
            data.timerTMP = thisTimerText;
            data.soundSENum = -1;
            data.soundPreNum = -1;
            data.zeroSoundPlay = false;

            data.addText = new AddText();
            data.addText.before = beforeText;
            data.addText.after = afterText;

            SetupData(data, thisLimitTime);
        }
        #endregion

        #region カウントダウン + text + 音

        #region カウントのたびに音を鳴らす場合(Text)
        // 追加テキストなし
        public void Set(
            int thisLimitTime, Text thisTimerText, bool thisZeroSoundPlay, int thisSoundSENum,
            string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, "", "");
            data.timerText = thisTimerText;
            data.timerTMP = null;
            data.soundPreNum = thisLimitTime + 1;

            SetupData(data, thisLimitTime);
        }

        // 追加テキストあり
        public void Set(
            int thisLimitTime, Text thisTimerText, bool thisZeroSoundPlay, int thisSoundSENum,
            string beforeText, string afterText, string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, beforeText, afterText);
            data.timerText = thisTimerText;
            data.timerTMP = null;
            data.soundPreNum = thisLimitTime + 1;

            SetupData(data, thisLimitTime);
        }
        #endregion

        #region thisSoundPreNum秒前から音を鳴らす場合(Text)
        // 追加テキストなし
        public void Set(
            int thisLimitTime, Text thisTimerText, bool thisZeroSoundPlay,
            int thisSoundSENum, int thisSoundPreNum, string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, "", "");
            data.timerText = thisTimerText;
            data.timerTMP = null;
            data.soundPreNum = thisSoundPreNum;

            SetupData(data, thisLimitTime);
        }

        // 追加テキストあり
        public void Set(
            int thisLimitTime, Text thisTimerText, bool thisZeroSoundPlay,
            int thisSoundSENum, int thisSoundPreNum,
            string beforeText, string afterText, string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, beforeText, afterText);
            data.timerText = thisTimerText;
            data.timerTMP = null;
            data.soundPreNum = thisSoundPreNum;

            SetupData(data, thisLimitTime);
        }
        #endregion

        #region カウントのたびに音を鳴らす場合(TextMeshPro)
        // 追加テキストなし
        public void Set(
            int thisLimitTime, TextMeshProUGUI thisTimerText, bool thisZeroSoundPlay,
            int thisSoundSENum, string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, "", "");
            data.timerText = null;
            data.timerTMP = thisTimerText;
            data.soundPreNum = thisLimitTime + 1;

            SetupData(data, thisLimitTime);
        }

        // 追加テキストあり
        public void Set(
            int thisLimitTime, TextMeshProUGUI thisTimerText, bool thisZeroSoundPlay,
            int thisSoundSENum, string beforeText, string afterText, string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, beforeText, afterText);
            data.timerText = null;
            data.timerTMP = thisTimerText;
            data.soundPreNum = thisLimitTime + 1;

            SetupData(data, thisLimitTime);
        }
        #endregion

        #region thisSoundPreNum秒前から音を鳴らす場合(TextMeshPro)
        // 追加テキストなし
        public void Set(
            int thisLimitTime, TextMeshProUGUI thisTimerText, bool thisZeroSoundPlay,
            int thisSoundSENum, int thisSoundPreNum, string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, "", "");
            data.timerText = null;
            data.timerTMP = thisTimerText;
            data.soundPreNum = thisSoundPreNum;

            SetupData(data, thisLimitTime);
        }

        // 追加テキストあり
        public void Set(
            int thisLimitTime, TextMeshProUGUI thisTimerText, bool thisZeroSoundPlay,
            int thisSoundSENum, int thisSoundPreNum, string beforeText, string afterText, 
            string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, beforeText, afterText);
            data.timerText = null;
            data.timerTMP = thisTimerText;
            data.soundPreNum = thisSoundPreNum;

            SetupData(data, thisLimitTime);
        }

        #endregion

        #endregion

        #region カウントダウン + 音
        public void Set(
            int thisLimitTime, bool thisZeroSoundPlay, int thisSoundSENum, 
            string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, "", "");
            data.timerText = null;
            data.timerTMP = null;
            data.soundPreNum = thisLimitTime + 1;

            SetupData(data, thisLimitTime);
        }

        public void Set(
            int thisLimitTime, bool thisZeroSoundPlay, int thisSoundSENum, 
            int thisSoundPreNum, string id = "Main")
        {
            CountData data = GetSoundCountData(id, thisSoundSENum, thisZeroSoundPlay, "", "");
            data.timerText = null;
            data.timerTMP = null;
            data.soundPreNum = thisSoundPreNum;

            SetupData(data, thisLimitTime);
        }
        #endregion

        #region Common
        // 共通の設定
        private void SetupData(CountData data, int thisLimitTime)
        {
            data.timerFin = false;
            data.limitTime = thisLimitTime + 1;
            data.currentTime = data.limitTime;
            data.timerTime = 0;

            if (data.timerText != null) data.timerText.text = "";
            if (data.timerTMP != null) data.timerTMP.text = "";

            countDatas.Add(data);
        }

        // 音付きのCountDataの共通設定
        private CountData GetSoundCountData(
            string id, int thisSoundSENum, bool thisZeroSoundPlay, 
            string beforeText, string afterText)
        {
            CountData data = new CountData();
            data.id = id;
            data.soundSENum = thisSoundSENum;
            data.zeroSoundPlay = thisZeroSoundPlay;

            data.addText = new AddText();
            data.addText.before = beforeText;
            data.addText.after = afterText;

            return data;
        }
        #endregion


        //----------------------------------
        //  update
        //----------------------------------
        #region カウントダウン
        public bool Update2(string id = "Main") {

            CountData data = GetCountData(id);
            if(data == null) return false;

            TimerUpdate(data);

            int preTime = data.currentTime;
            data.currentTime = (int)(data.limitTime - data.timerTime);

            // text
            if (data.timerText != null) data.timerText.text = TextContent(data);
            if (data.timerTMP != null) data.timerTMP.text = TextContent(data);

            // sound
            if (data.soundSENum >= 0)
            {
                print(data.currentTime + ", " + data.soundPreNum);
                if (preTime != data.currentTime && data.currentTime <= data.soundPreNum) {

                    if (data.currentTime == 0) {
                        if (data.zeroSoundPlay) Util.sound.PlaySE(data.soundSENum);
                    } else {
                        Util.sound.PlaySE(data.soundSENum);
                    }

                    print(data.currentTime);

                }
            }

            if (data.currentTime > 0) return false;
            else return true;
        }

        private CountData GetCountData(string id)
        {
            if(countDatas == null) return null;

            for (int i = 0; i < countDatas.Count; i++)
            {
                if (countDatas[i].id == id) return countDatas[i];
            }

            return null;
        }

        private void TimerUpdate(CountData data)
        {
            if (data.timerFin) return;

            data.timerTime += Time.deltaTime;
            if (data.timerTime >= data.limitTime)
            {
                data.timerTime = data.limitTime;
                data.timerFin = true;
            }
        }
        #endregion


        //----------------------------------
        //  function
        //----------------------------------
        // 現在の時間取得
        public int CurrentTime(string id = "Main")
        {
            CountData data = GetCountData(id);
            if (data == null) return 0;

            return data.currentTime;
        }

        private string TextContent(CountData data)
        {
            string countStr = "";

            if (data.addText.before != null) countStr += data.addText.before;
            countStr += data.currentTime.ToString();
            if (data.addText.after != null) countStr += data.addText.after;

            return countStr;
        }

    }

}

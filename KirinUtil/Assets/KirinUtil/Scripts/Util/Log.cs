using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {
    public class Log : MonoBehaviour {

        [Header("[Setting]")]
        public Text logText;
        public string saveDir = "/../../AppData/Log/";
        public string saveFileName = "log_";
        private string saveFilePath = "";

        [Header("[Log type]")]
        public bool saveOn = false;
        public bool timeOn = true;
        public bool methodOn = true;
        private bool textOn = false;

        [SerializeField, Tooltip("新しいログがUI範囲内に収まるようにテキストを調整する(Truncate限定)")]
        private bool viewInRect = true;
        private bool inited = false;

        private void Start() {
            CatchLogInit();
        }

        /*//----------------------------------
        //  print文
        //----------------------------------
        #region print文
        public static void Print(string message) {
            StringBuilder logData = new StringBuilder();
            
            StackFrame objStackFrame = new StackFrame(1);
            string strClassName = objStackFrame.GetMethod().ReflectedType.FullName;
            string strMethodName = objStackFrame.GetMethod().Name;

            logData.Append(message);
            logData.Append(Environment.NewLine);
            logData.Append(strClassName + ":");
            logData.Append(strMethodName + "()");

            print(logData);
        }
        #endregion*/

        //----------------------------------
        //  catch log
        //----------------------------------
        #region CatchLog
        private void CatchLogInit() {
            // 保存フォルダ作成
            if (!inited) FileInit();
        }

        private void OnEnable() {
            if (!inited) FileInit();
            if (saveOn || textOn) Application.logMessageReceived += HandleLog;
        }

        private void OnDisable() {
            if (saveOn || textOn) Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logData, string stackTrace, LogType logType) {
            if (string.IsNullOrEmpty(logData))
                return;

            // time
            string time = "";
            if (timeOn) {
                time += "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + +DateTime.Now.Second + ":" + +DateTime.Now.Millisecond + "] ";
            }

            // method
            string thisLog;
            if (methodOn) thisLog = time + logData + Environment.NewLine + stackTrace + Environment.NewLine;
            else thisLog = time + logData + Environment.NewLine;

            // save
            if (saveOn) {
                if (!inited) FileInit();
                string thisHtmlLog = LogHtml(thisLog, logType);
                File.AppendAllText(saveFilePath, thisHtmlLog);
            }

            // text
            if (textOn) {
                logText.text += LogColor(thisLog, logType);

                if (viewInRect && logText.verticalOverflow == VerticalWrapMode.Truncate)
                    AdjustText();
            }
        }

        private void OnDestroy() {
            if(saveOn) File.AppendAllText(saveFilePath, Environment.NewLine + "</body>" + Environment.NewLine + "</html>" + Environment.NewLine);
        }
        #endregion

        #region function
        private void FileInit() {

            // 保存フォルダ作成
            if (saveOn) {
                saveDir = Application.dataPath + saveDir;
                if (!Directory.Exists(saveDir)) {
                    Directory.CreateDirectory(saveDir);
                }
                saveFilePath = saveDir + saveFileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
                if (!File.Exists(saveFilePath)) File.AppendAllText(saveFilePath, "<html>" + Environment.NewLine + "<body>" + Environment.NewLine);
            }

            if (logText != null) {
                textOn = true;
            }

            inited = true;
        }

        // Textの範囲内に文字列を収める
        private void AdjustText() {
            TextGenerator generator = logText.cachedTextGenerator;
            var settings = logText.GetGenerationSettings(logText.rectTransform.rect.size);
            generator.Populate(logText.text, settings);

            int countVisible = generator.characterCountVisible;
            if (countVisible == 0 || logText.text.Length <= countVisible)
                return;

            int truncatedCount = logText.text.Length - countVisible;
            var lines = logText.text.Split('\n');
            foreach (string line in lines) {
                // 見切れている文字数が0になるまで、テキストの先頭行から消してゆく
                truncatedCount -= (line.Length + 1);
                if (truncatedCount <= 0)
                    break;

                logText.text = logText.text.Remove(0, line.Length + 1);
            }
        }

        // ログに色を付ける
        private string LogColor(string thisLog, LogType logType) {
            switch (logType) {
                case LogType.Error:
                    thisLog = string.Format("<color=red>{0}</color>", thisLog);
                    break;
                case LogType.Assert:
                    thisLog = string.Format("<color=red>{0}</color>", thisLog);
                    break;
                case LogType.Exception:
                    thisLog = string.Format("<color=red>{0}</color>", thisLog);
                    break;
                case LogType.Warning:
                    thisLog = string.Format("<color=#d8c600>{0}</color>", thisLog);
                    break;
                default:
                    break;
            }

            return thisLog;
        }

        // ログをhtml化
        private string LogHtml(string thisLog, LogType logType) {
            StringBuilder htmlLog = new StringBuilder();
            string[] del = { "\r\n", Environment.NewLine, "\n" };

            string[] logArray = thisLog.Split(del, StringSplitOptions.None);
            for (int i = 0; i < logArray.Length; i++) {
                htmlLog.Append(logArray[i]);

                if (i != logArray.Length - 1) {
                    htmlLog.Append("<br>");
                    htmlLog.Append(Environment.NewLine);
                }
            }

            string colorLog = "";
            switch (logType) {
                case LogType.Error:
                    colorLog = string.Format("<font color=red>{0}</font>", htmlLog);
                    break;
                case LogType.Assert:
                    colorLog = string.Format("<font color=red>{0}</font>", htmlLog);
                    break;
                case LogType.Exception:
                    colorLog = string.Format("<font color=red>{0}</font>", htmlLog);
                    break;
                case LogType.Warning:
                    colorLog = string.Format("<font color=#d8c600>{0}</font>", htmlLog);
                    break;
                default:
                    colorLog = htmlLog.ToString();
                    break;
            }

            return colorLog;
        }

        #endregion
    }
}

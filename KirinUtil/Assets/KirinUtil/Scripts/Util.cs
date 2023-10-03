////////////////////////////////////////////////////////////////////////////////
//
//  MIZUTANI KIRIN
//  Copyright 2016-2020 MIZUTANI KIRIN All Rights Reserved.
//
//  NOTICE: MIZUTANI KIRIN permits you to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

//#define MovieEnable

using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace KirinUtil {

    [Serializable]
    public class OrderData {
        public string id;
        public float value;
    }

    [RequireComponent(typeof(KRNMedia))]
    [RequireComponent(typeof(KRNFile))]
    public class Util : MonoBehaviour {
        [NonSerialized] public static string version = "ver1.0.7";
        [NonSerialized] public static string copylight = "Copyright 2016-2022 MIZUTANI KIRIN All Rights Reserved.";

        [NonSerialized] public static KRNMedia media;
        [NonSerialized] public static KRNFile file;
        [NonSerialized] public static NetManager net;

        [NonSerialized] public static SoundManager sound;
        [NonSerialized] public static ImageManager image;
#if MovieEnable
        [NonSerialized] public static MovieManager movie;
#endif

        private void Awake() {

            media = gameObject.GetComponent<KRNMedia>();
            file = gameObject.GetComponent<KRNFile>();
            net = gameObject.GetComponent<NetManager>();

            if (gameObject.transform.Find("soundManager") != null)
                sound = gameObject.transform.Find("soundManager").gameObject.GetComponent<SoundManager>();
            if (gameObject.transform.Find("imageManager") != null)
                image = gameObject.transform.Find("imageManager").gameObject.GetComponent<ImageManager>();
#if MovieEnable
            if (gameObject.transform.Find("movieManager") != null)
                movie = gameObject.transform.Find("movieManager").gameObject.GetComponent<MovieManager>();
#endif

        }

        private void Update() {
            BasicSettingUpdate();
        }

        //----------------------------------
        //  BasicSetting
        //----------------------------------
        #region BasicSetting
        private static bool cursorVisible = true;
        private static bool set = false;
        private static Vector3 mousePosPre = Vector3.zero;
        private static float cursorTimer;
        public static void BasicSetting(bool _cursor) {
            print("BasicSetting");

            set = true;
            cursorVisible = _cursor;

        }

        public static void BasicSetting(Vector2 screenSize, bool fullscreen, int targetFps, bool _cursor) {
            print("BasicSetting");

            Application.targetFrameRate = targetFps;
            Screen.SetResolution((int)screenSize.x, (int)screenSize.y, fullscreen);

            set = true;
            cursorVisible = _cursor;

        }

        private void BasicSettingUpdate() {
            if (!set) return;

            if (Input.GetKeyUp(KeyCode.Escape)) {
                Application.Quit();
            }

            // カーソルを非表示
            if (!cursorVisible) {
                Vector3 mousePos = Input.mousePosition;

                if (mousePos != mousePosPre) {
                    Cursor.visible = true;
                    cursorTimer = 0.0f;
                } else {
                    if (cursorTimer >= 2.0f) {
                        Cursor.visible = false;
                    } else {
                        cursorTimer += Time.deltaTime;
                    }
                }

                mousePosPre = mousePos;
            }
        }
        #endregion

        //----------------------------------
        //  ShuffleArray
        //----------------------------------
        #region ShuffleArray
        public static int[] ShuffleArray(int[] arr) {
            int length = arr.Length;
            int[] newArr = new int[length];
            newArr = arr;

            while (length != 0) {
                int rnd = (int)Mathf.Floor(UnityEngine.Random.value * length);
                length--;
                var tmp = newArr[length];
                newArr[length] = newArr[rnd];
                newArr[rnd] = tmp;
            }

            return newArr;
        }

        public static float[] ShuffleArray(float[] arr) {
            int length = arr.Length;
            float[] newArr = new float[length];
            newArr = arr;

            while (length != 0) {
                int rnd = (int)Mathf.Floor(UnityEngine.Random.value * length);
                length--;
                var tmp = newArr[length];
                newArr[length] = newArr[rnd];
                newArr[rnd] = tmp;
            }

            return newArr;
        }

        public static string[] ShuffleArray(string[] arr) {
            int length = arr.Length;
            string[] newArr = new string[length];
            newArr = arr;

            while (length != 0) {
                int rnd = (int)Mathf.Floor(UnityEngine.Random.value * length);
                length--;
                var tmp = newArr[length];
                newArr[length] = newArr[rnd];
                newArr[rnd] = tmp;
            }

            return newArr;
        }

        public static Vector3[] ShuffleArray(Vector3[] arr) {
            int length = arr.Length;
            Vector3[] newArr = new Vector3[length];
            newArr = arr;

            while (length != 0) {
                int rnd = (int)Mathf.Floor(UnityEngine.Random.value * length);
                length--;
                var tmp = newArr[length];
                newArr[length] = newArr[rnd];
                newArr[rnd] = tmp;
            }

            return newArr;
        }
        #endregion


        //----------------------------------
        //  DayArea
        //----------------------------------
        #region DayArea
        public static bool DayArea(string _startDate, string _endDate) {
            bool flag = false;

            int now_month = DateTime.Now.Month;
            int now_date = DateTime.Now.Day;

            //----------------------------------
            //  init
            //----------------------------------
            // 日にち
            if (_startDate == "" || _startDate == null || _endDate == "" || _endDate == null) {
                flag = true;
            } else {
                string[] start_day = _startDate.Split("/"[0]);
                int start_month = int.Parse(start_day[0]);
                int start_date = int.Parse(start_day[1]);

                string[] end_day = _endDate.Split("/"[0]);
                int end_month = int.Parse(end_day[0]);
                int end_date = int.Parse(end_day[1]);


                if (start_month < end_month) {
                    // 通常
                    if (now_month > start_month && now_month < end_month) {
                        // 月が範囲内の場合
                        flag = true;
                    } else if (now_month == start_month) {
                        // start_monthと同じ月の場合
                        if (now_date > start_date) {
                            // 日が範囲内の場合
                            flag = true;
                        } else if (now_date == start_date) {
                            // 日がstart_dateと同じ場合
                            flag = true;
                        }
                    } else if (now_month == end_month) {
                        // end_monthと同じ月の場合
                        if (now_date < end_date) {
                            // 日が範囲内の場合
                            flag = true;
                        } else if (now_date == end_date) {
                            // 日がend_dateと同じ場合
                            flag = true;
                        }
                    }
                } else if (start_month > end_month) {
                    // 年を越す
                    if (now_month > start_month || now_month < end_month) {
                        // 月が範囲内の場合
                        flag = true;
                    } else if (now_month == start_month) {
                        // start_monthと同じ月の場合
                        if (now_date > start_date) {
                            // 日が範囲内の場合
                            flag = true;
                        } else if (now_date == start_date) {
                            // 日がstart_dateと同じ場合
                            flag = true;
                        }
                    } else if (now_month == end_month) {
                        // end_monthと同じ月の場合
                        if (now_date < end_date) {
                            // 日が範囲内の場合
                            flag = true;
                        } else if (now_date == end_date) {
                            // 日がend_dateと同じ場合
                            flag = true;
                        }
                    }
                } else {
                    if (now_month == start_month) {
                        // start_monthとend_monthが同じ月
                        if (now_date > start_date && now_date < end_date) {
                            // 日が範囲内の場合
                            flag = true;
                        } else if (now_date == start_date) {
                            // 日がstart_dateと同じ場合
                            flag = true;
                        } else if (now_date == end_date) {
                            // 日がend_dateと同じ場合
                            flag = true;
                        } else {
                            // 今日の日付とは違うが常時表示
                            //if( start_date == end_date){
                            //	if( now_date == start_date ) flag = true;
                            //}
                        }
                    }
                }
            }
            return flag;
        }
        #endregion

        //----------------------------------
        //  TimeArea
        //----------------------------------
        #region TimeArea
        public static bool TimeArea(string startTime, string nowTime, string endTime) {

            bool returnFlag = false;

            string[] startTimeTmp = startTime.Split(":"[0]);
            int startHour = int.Parse(startTimeTmp[0]);
            int startMinute = int.Parse(startTimeTmp[1]);

            string[] nowTimeTmp = nowTime.Split(":"[0]);
            int nowHour = int.Parse(nowTimeTmp[0]);
            int nowMinute = int.Parse(nowTimeTmp[1]);

            string[] endTimeTmp = endTime.Split(":"[0]);
            int endHour = int.Parse(endTimeTmp[0]);
            int endMinute = int.Parse(endTimeTmp[1]);

            // 
            if (nowHour > startHour) { // 10:50 - 14:45 - 
                if (nowHour < endHour) {
                    // (1) 10:50 - 14:45 - 16:20
                    returnFlag = true;
                } else if (nowHour == endHour) {
                    if (nowMinute > endMinute) {
                        // (2) 10:50 - 14:45 - 14:20
                        returnFlag = false;
                    } else {
                        // (3) 10:50 - 14:45 - 14:50
                        returnFlag = true;
                    }
                } else {
                    if (startHour < endHour) {
                        // (4) 10:50 - 14:45 - 13:20
                        returnFlag = false;
                    } else if (startHour == endHour) {
                        if (startMinute > endMinute) {
                            // (5) 10:50 - 14:45 - 10:40
                            returnFlag = true;
                        } else {
                            // (6) 10:50 - 14:45 - 10:52;
                            returnFlag = false;
                        }
                    } else {
                        // (7) 11:50 - 14:45 - 10:50;
                        returnFlag = true;
                    }
                }
            } else if (nowHour == startHour) { // 14:20 - 14:45 -
                if (nowMinute >= startMinute) {
                    if (nowHour < endHour) {
                        // (5) 14:20 - 14:45 - 16:20
                        returnFlag = true;
                    } else if (nowHour == endHour) {
                        if (nowMinute > endMinute) {
                            if (startMinute >= endMinute) {
                                // (6) 14:20 - 14:45 - 14:10
                                returnFlag = true;
                            } else {
                                // (7) 14:20 - 14:45 - 14:30
                                returnFlag = false;
                            }

                        } else {
                            // (7) 14:20 - 14:45 - 14:50
                            returnFlag = true;
                        }
                    } else {
                        // (4) 14:20 - 14:45 - 13:50
                        returnFlag = true;
                    }
                } else {
                    if (startHour < endHour) {
                        // (4) 14:50 - 14:45 - 15:20
                        returnFlag = false;
                    } else if (startHour == endHour) {
                        if (startMinute > endMinute) {
                            // (4) 14:50 - 14:45 - 14:20
                            returnFlag = false;
                        } else if (startMinute == endMinute) {
                            // (4) 14:50 - 14:45 - 14:50
                            returnFlag = true;
                        } else {
                            // (4) 14:50 - 14:45 - 14:55
                            returnFlag = false;
                        }
                    } else {
                        // (4) 14:50 - 14:45 - 13:20
                        returnFlag = false;
                    }
                }
            } else { // 18:20 - 14:45 - 
                if (nowHour < endHour) {
                    if (startHour > endHour) {
                        // (4) 18:20 - 14:45 - 16:20
                        returnFlag = true;
                    } else if (startHour == endHour) {
                        if (startMinute < endMinute) {
                            // 18:20 - 14:45 - 18:30
                            returnFlag = false;
                        } else if (startMinute == endMinute) {
                            // 18:20 - 14:45 - 18:20
                            returnFlag = true;
                        } else {
                            // 18:30 - 14:45 - 18:20
                            returnFlag = true;
                        }
                    } else {
                        // 18:20 - 14:45 - 19:20
                        returnFlag = false;
                    }

                } else if (nowHour == endHour) {
                    if (nowMinute > endMinute) {
                        // (2) 18:20 - 14:45 - 14:20
                        returnFlag = false;
                    } else {
                        // (3) 18:20 - 14:45 - 14:50
                        returnFlag = true;
                    }
                } else {
                    // (3) 18:20 - 14:45 - 12:50
                    returnFlag = false;
                }
            }

            return returnFlag;
        }
        #endregion

        //----------------------------------
        //  WeekArea
        //----------------------------------
        #region WeekArea
        public static bool WeekArea(int _startWeek, int _endWeek) {
            bool returnFlag = false;
            int nowWeek = WeekNum(System.DateTime.Now.DayOfWeek.ToString());

            if (_startWeek < _endWeek) {
                // 通常時
                if (nowWeek >= _startWeek && nowWeek <= _endWeek) {
                    returnFlag = true;
                }
            } else if (_startWeek > _endWeek) {
                // 土日を挟む時
                if (nowWeek >= _startWeek) {
                    returnFlag = true;
                } else if (nowWeek <= _endWeek) {
                    returnFlag = true;
                }
            } else if (_startWeek == _endWeek) {
                // _startWeekと_endWeekが同じ時
                if (nowWeek == _startWeek) {
                    returnFlag = true;
                }
            }

            return returnFlag;
        }

        private static int WeekNum(string week) {
            int weekNum = -1;

            if (week == "Sunday")
                weekNum = 0;
            else if (week == "Monday")
                weekNum = 1;
            else if (week == "Tuesday")
                weekNum = 2;
            else if (week == "Wednesday")
                weekNum = 3;
            else if (week == "Thursday")
                weekNum = 4;
            else if (week == "Friday")
                weekNum = 5;
            else if (week == "Saturday")
                weekNum = 6;

            return weekNum;
        }
        #endregion


        //----------------------------------
        //  2点間の角度
        //----------------------------------
        public static float Angle2(Vector2 p1, Vector2 p2) {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }

        //----------------------------------
        //  3点の角度
        //----------------------------------
        #region Angle3
        public static float Angle3(Vector2 p0, Vector2 p1, Vector2 p2) {
            float[] ba = new float[2];
            ba[0] = p0.x - p1.x;
            ba[1] = p0.y - p1.y;
            float[] bc = new float[2];
            bc[0] = p2.x - p1.x;
            bc[1] = p2.y - p1.y;

            float babc = ba[0] * bc[0] + ba[1] * bc[1];
            float ban = (ba[0] * ba[0]) + (ba[1] * ba[1]);
            float bcn = (bc[0] * bc[0]) + (bc[1] * bc[1]);
            float radian = Mathf.Acos(babc / (Mathf.Sqrt(ban * bcn)));
            float angle = (float)(radian * 180 / Mathf.PI);  // 結果（ラジアンから角度に変換）

            return angle;
        }

        /*private float GetDeltaAngle(Vector2 p0, Vector2 p1, Vector2 p2) {
            float deltaAngle = Angle2(p0, p1) - Angle2(p0, p2);
            deltaAngle += deltaAngle > 180 ? -360 : 0;
            deltaAngle += deltaAngle < -180 ? 360 : 0;
            return deltaAngle;
        }*/

        #endregion

        //----------------------------------
        //  0-360度にする
        //----------------------------------
        public static float To360Angle(float angle) {
            float fixAngle = angle;

            if (angle < 0) {
                while (fixAngle < 0) {
                    fixAngle += 360;
                }
            } else if (angle > 360) {
                while (fixAngle > 360) {
                    fixAngle -= 360;
                }
            }

            return fixAngle;
        }


        //----------------------------------
        //  Radian
        //----------------------------------
        #region Radian
        public static float Deg2Rad(float degAngle) {
            return Mathf.Deg2Rad * degAngle;
        }

        public static float Rad2Deg(float radian) {
            return Mathf.Rad2Deg * radian;
        }
        #endregion


        //----------------------------------
        //  AddZero
        //----------------------------------
        // 指定した桁数にする。
        // AddZero(1, 2) = 01
        public static string AddZero(int num, int digits) {
            string returnSt = "";
            int numKeta = num.ToString().Length;
            int addZero = digits - numKeta;

            if (addZero < 0)
                addZero = 0;

            for (int i = 0; i < addZero; i++) {
                returnSt += "0";
            }

            returnSt += num;

            return returnSt;
        }


        //----------------------------------
        //  小数点の桁数を揃える
        //----------------------------------
        // PointAlignment(小数点の数字, 小数点何位に揃えるか)
        public static string PointAlignment(float num, int pointNum) {

            float point2 = PointRound(num, pointNum);
            string numSt = point2.ToString();

            if (numSt.IndexOf("."[0]) == -1) {
                numSt += ".";
                for (int i = 0; i < pointNum; i++) {
                    numSt += "0";
                }
            } else {
                string[] st = numSt.Split("."[0]);
                if (st.Length == 2) {
                    int loopNum = pointNum - st[1].Length;
                    for (int i = 0; i < loopNum; i++) {
                        numSt += "0";
                    }
                }
            }

            return numSt;
        }


        //----------------------------------
        //  四捨五入
        //----------------------------------
        // PointRound(小数点の数字, 小数点何位を四捨五入するか)
        public static float PointRound(float num, int pointNum) {
            if (num.ToString().IndexOf(".") == -1) return num;
            if (pointNum <= 0) return num;

            float point = 0;

            int scaleUp = 1;
            float scaleDown = 1;
            for (int i = 0; i < pointNum; i++) {
                scaleUp *= 10;
                scaleDown *= 0.1f;
            }

            point = RoundToInt(num * scaleUp);
            point = point * scaleDown;

            if (point.ToString().IndexOf(".") != -1) {
                string[] point2St = point.ToString().Split("."[0]);
                if (point2St[1].Length > pointNum) {
                    point2St[1] = point2St[1].Substring(0, pointNum);
                    point = float.Parse(point2St[0] + "." + point2St[1]);
                }

            }

            return point;
        }


        //----------------------------------
        //  FrameRate
        //----------------------------------
        #region FrameRate
        private static int frameCount;
        private static float prevTime;
        private static float framerate = 0.0f;

        public static int FrameRateUpdate(float calcSpanTime = 1, Text fpsText = null) {
            ++frameCount;
            float time = Time.realtimeSinceStartup - prevTime;

            if (time >= calcSpanTime) {
                framerate = frameCount / time;
                frameCount = 0;
                prevTime = Time.realtimeSinceStartup;
            }

            int fpsInt = Mathf.FloorToInt(framerate);
            if (fpsText != null) fpsText.text = fpsInt.ToString();

            return fpsInt;
        }
        #endregion


        //----------------------------------
        //  GetRandomString
        //----------------------------------
        public static string GetRandomString(int length) {
            string passwordChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder stringBuilder = new StringBuilder(length);
            System.Random random = new System.Random();
            for (int index1 = 0; index1 < length; ++index1) {
                int index2 = random.Next(passwordChars.Length);
                char passwordChar = passwordChars[index2];
                stringBuilder.Append(passwordChar);
            }
            return stringBuilder.ToString();
        }

        //----------------------------------
        //  特定文字のカウント
        //----------------------------------
        public static int CountOf(string message, string[] strArray) {
            int count = 0;

            foreach (string str in strArray) {

                int index = str.IndexOf(message, 0);
                while (index != -1) {
                    count++;
                    index = str.IndexOf(message, index + str.Length);
                }
            }

            return count;
        }


        //----------------------------------
        //  world座標をCanvas座標に変換
        //----------------------------------
        #region world座標をCanvas座標に変換
        public static Vector2 GetUIPos(Camera uiCamera, Camera worldCamera, Canvas canvas, GameObject targetObj) {

            var uiPos = Vector2.zero;
            var canvasRect = canvas.GetComponent<RectTransform>();

            var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, targetObj.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out uiPos);

            //uiPos = new Vector2(uiPos.x + canvasRect.sizeDelta.x * 0.5f, uiPos.y + canvasRect.sizeDelta.y * 0.5f);

            return uiPos;
        }

        public static Vector2 GetUIPos(Camera uiCamera, Camera worldCamera, Canvas canvas, Vector3 targetPos) {

            var uiPos = Vector2.zero;
            var canvasRect = canvas.GetComponent<RectTransform>();

            var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, targetPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out uiPos);

            //uiPos = new Vector2(uiPos.x + canvasRect.sizeDelta.x * 0.5f, uiPos.y + canvasRect.sizeDelta.y * 0.5f);

            return uiPos;
        }
        #endregion


        //----------------------------------
        //  Canvas座標をworld座標に変換
        //----------------------------------
        #region Canvas座標をworld座標に変換
        // mouse
        public static Vector3 GetWorldMousePos(Camera camera, float z) {

            Vector3 mousePos = Input.mousePosition;
            Vector3 pos = camera.ScreenToWorldPoint(mousePos);
            pos.z = z;

            return pos;
        }

        // object
        public static Vector3 GetWorldPos(Camera camera, Canvas canvas, Vector3 canvasPos, float z) {

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            canvasPos.x += canvasRect.sizeDelta.x * 0.5f;
            canvasPos.y += canvasRect.sizeDelta.y * 0.5f;
            canvasPos.z = z;

            Vector3 pos = camera.ScreenToWorldPoint(canvasPos);
            //pos.z = z;

            return pos;
        }
        #endregion

        //----------------------------------
        //  position
        //----------------------------------
        #region position
        public static void PosX(GameObject obj, float x) {
            obj.transform.position = new Vector3(x, obj.transform.position.y, obj.transform.position.z);
        }

        public static void PosY(GameObject obj, float y) {
            obj.transform.position = new Vector3(obj.transform.position.x, y, obj.transform.position.z);
        }

        public static void PosZ(GameObject obj, float z) {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, z);
        }

        public static void LocalPosX(GameObject obj, float x) {
            obj.transform.localPosition = new Vector3(x, obj.transform.localPosition.y, obj.transform.localPosition.z);
        }

        public static void LocalPosY(GameObject obj, float y) {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, y, obj.transform.localPosition.z);
        }

        public static void LocalPosZ(GameObject obj, float z) {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, z);
        }
        #endregion

        //----------------------------------
        //  scale
        //----------------------------------
        #region scale
        public static void Scale(GameObject obj, float scale) {
            obj.transform.localScale = new Vector3(scale, scale, scale);
        }

        public static void ScaleX(GameObject obj, float x) {
            obj.transform.localScale = new Vector3(x, obj.transform.localScale.y, obj.transform.localScale.z);
        }

        public static void ScaleY(GameObject obj, float y) {
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, y, obj.transform.localScale.z);
        }

        public static void ScaleZ(GameObject obj, float z) {
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, z);
        }
        #endregion


        //----------------------------------
        //  rotation
        //----------------------------------
        #region rotation

        public static void RotationX(GameObject obj, float x)
        {
            obj.transform.rotation = Quaternion.Euler(x, obj.transform.rotation.eulerAngles.y, obj.transform.rotation.eulerAngles.z);
        }

        public static void RotationY(GameObject obj, float y)
        {
            obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.eulerAngles.x, y, obj.transform.rotation.eulerAngles.z);
        }

        public static void RotationZ(GameObject obj, float z)
        {
            obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.eulerAngles.x, obj.transform.rotation.eulerAngles.y, z);
        }

        public static void LocalRotationX(GameObject obj, float x)
        {
            obj.transform.localRotation = Quaternion.Euler(x, obj.transform.rotation.eulerAngles.y, obj.transform.rotation.eulerAngles.z);
        }

        public static void LocalRotationY(GameObject obj, float y)
        {
            obj.transform.localRotation = Quaternion.Euler(obj.transform.rotation.eulerAngles.x, y, obj.transform.rotation.eulerAngles.z);
        }

        public static void LocalRotationZ(GameObject obj, float z)
        {
            obj.transform.localRotation = Quaternion.Euler(obj.transform.rotation.eulerAngles.x, obj.transform.rotation.eulerAngles.y, z);
        }
        #endregion

        //----------------------------------
        //  改行処理
        //----------------------------------
        // <br>をEnvironment.NewLineにする
        public static string GetLineText(string str) {

            string lineSt = "";

            //print("GetLineText: " + str.IndexOf("<br>"));
            if (str.IndexOf("<br>") != -1) {
                string[] del = { "<br>" };
                string[] st = str.Split(del, StringSplitOptions.None);

                for (int i = 0; i < st.Length; i++) {
                    if (i != st.Length - 1) lineSt += st[i] + Environment.NewLine;
                    else lineSt += st[i];
                }

                print(lineSt);
            } else {
                lineSt = str;
            }

            return lineSt;
        }

        //----------------------------------
        //  デバッグ用StopWatch
        //  (コードの処理速度を計るためのもの)
        //----------------------------------
        #region デバッグ用StopWatch
        private static System.Diagnostics.Stopwatch stopwatch;
        public static void DebugWatchStart() {
            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
        }

        public static void DebugWatchStop(Text debugText = null) {
            stopwatch.Stop();

            string message = stopwatch.ElapsedMilliseconds + "ms";
            print(message);
            if (debugText != null) debugText.text = message;
        }
        #endregion


        //----------------------------------
        //  List中の最大/最小を探しList番号
        //  を返す
        //----------------------------------
        #region Find Max value
        public static int GetMaxListNum(List<float> thisList) {
            int listNum = 0;
            float maxNum = 0;

            for (int i = 0; i < thisList.Count; i++) {
                if (thisList[i] > maxNum) {
                    maxNum = thisList[i];
                    listNum = i;
                }
            }

            return listNum;
        }

        public static int GetMaxListNum(List<int> thisList) {
            int listNum = 0;
            int maxNum = 0;

            for (int i = 0; i < thisList.Count; i++) {
                if (thisList[i] > maxNum) {
                    maxNum = thisList[i];
                    listNum = i;
                }
            }

            return listNum;
        }

        public static int GetMaxListNum(List<GameObject> thisList, AxisType type, bool isLocal) {
            int listNum = 0;
            float maxNum = 0;

            if (isLocal) {
                for (int i = 0; i < thisList.Count; i++) {
                    if (type == AxisType.X) {
                        if (thisList[i].transform.localPosition.x > maxNum) {
                            maxNum = thisList[i].transform.localPosition.x;
                            listNum = i;
                        }
                    } else if (type == AxisType.Y) {
                        if (thisList[i].transform.localPosition.y > maxNum) {
                            maxNum = thisList[i].transform.localPosition.y;
                            listNum = i;
                        }
                    } else if (type == AxisType.Z) {
                        if (thisList[i].transform.localPosition.z > maxNum) {
                            maxNum = thisList[i].transform.localPosition.z;
                            listNum = i;
                        }
                    }

                }
            } else {
                for (int i = 0; i < thisList.Count; i++) {
                    if (type == AxisType.X) {
                        if (thisList[i].transform.position.x > maxNum) {
                            maxNum = thisList[i].transform.position.x;
                            listNum = i;
                        }
                    } else if (type == AxisType.Y) {
                        if (thisList[i].transform.position.y > maxNum) {
                            maxNum = thisList[i].transform.position.y;
                            listNum = i;
                        }
                    } else if (type == AxisType.Z) {
                        if (thisList[i].transform.position.z > maxNum) {
                            maxNum = thisList[i].transform.position.z;
                            listNum = i;
                        }
                    }

                }
            }


            return listNum;
        }

        #endregion

        #region Find Min value
        public static int GetMinListNum(List<float> thisList) {
            int listNum = 0;
            float minNum = thisList[0];

            for (int i = 0; i < thisList.Count; i++) {
                if (thisList[i] < minNum) {
                    minNum = thisList[i];
                    listNum = i;
                }
            }

            return listNum;
        }

        public static int GetMinListNum(List<int> thisList) {
            int listNum = 0;
            int minNum = thisList[0];

            for (int i = 0; i < thisList.Count; i++) {
                if (thisList[i] < minNum) {
                    minNum = thisList[i];
                    listNum = i;
                }
            }

            return listNum;
        }

        public static int GetMinListNum(List<GameObject> thisList, AxisType type, bool isLocal) {
            int listNum = 0;
            float minNum = 0;

            if (isLocal) {

                if (type == AxisType.X) minNum = thisList[0].transform.localPosition.x;
                else if (type == AxisType.Y) minNum = thisList[0].transform.localPosition.y;
                else if (type == AxisType.Z) minNum = thisList[0].transform.localPosition.z;

                for (int i = 0; i < thisList.Count; i++) {
                    if (type == AxisType.X) {
                        if (thisList[i].transform.localPosition.x < minNum) {
                            minNum = thisList[i].transform.localPosition.x;
                            listNum = i;
                        }
                    } else if (type == AxisType.Y) {
                        if (thisList[i].transform.localPosition.y < minNum) {
                            minNum = thisList[i].transform.localPosition.y;
                            listNum = i;
                        }
                    } else if (type == AxisType.Z) {
                        if (thisList[i].transform.localPosition.z < minNum) {
                            minNum = thisList[i].transform.localPosition.z;
                            listNum = i;
                        }
                    }

                }
            } else {

                if (type == AxisType.X) minNum = thisList[0].transform.position.x;
                else if (type == AxisType.Y) minNum = thisList[0].transform.position.y;
                else if (type == AxisType.Z) minNum = thisList[0].transform.position.z;

                for (int i = 0; i < thisList.Count; i++) {
                    if (type == AxisType.X) {
                        if (thisList[i].transform.position.x < minNum) {
                            minNum = thisList[i].transform.position.x;
                            listNum = i;
                        }
                    } else if (type == AxisType.Y) {
                        if (thisList[i].transform.position.y < minNum) {
                            minNum = thisList[i].transform.position.y;
                            listNum = i;
                        }
                    } else if (type == AxisType.Z) {
                        if (thisList[i].transform.position.z < minNum) {
                            minNum = thisList[i].transform.position.z;
                            listNum = i;
                        }
                    }

                }
            }


            return listNum;
        }
        #endregion


        //----------------------------------
        //  中心点を取得
        //----------------------------------
        #region CenterPos
        // CenterPos
        public static float CenterPosX(GameObject obj0, GameObject obj1) {
            return CenterValue(obj0.transform.position.x, obj1.transform.position.x);
        }

        public static float CenterPosY(GameObject obj0, GameObject obj1) {
            return CenterValue(obj0.transform.position.y, obj1.transform.position.y);
        }

        public static float CenterPosZ(GameObject obj0, GameObject obj1) {
            return CenterValue(obj0.transform.position.z, obj1.transform.position.z);
        }

        public static Vector3 CenterPos(GameObject obj0, GameObject obj1) {
            Vector3 centerPos = new Vector3(
                CenterPosX(obj0, obj1),
                CenterPosY(obj0, obj1),
                CenterPosZ(obj0, obj1)
            );

            return centerPos;
        }

        // CenterLocalPos
        public static float CenterLocalPosX(GameObject obj0, GameObject obj1) {
            return CenterValue(obj0.transform.localPosition.x, obj1.transform.localPosition.x);
        }

        public static float CenterLocalPosY(GameObject obj0, GameObject obj1) {
            return CenterValue(obj0.transform.localPosition.y, obj1.transform.localPosition.y);
        }

        public static float CenterLocalPosZ(GameObject obj0, GameObject obj1) {
            return CenterValue(obj0.transform.localPosition.z, obj1.transform.localPosition.z);
        }

        public static Vector3 CenterLocalPos(GameObject obj0, GameObject obj1) {
            Vector3 centerPos = new Vector3(
                CenterLocalPosX(obj0, obj1),
                CenterLocalPosY(obj0, obj1),
                CenterLocalPosZ(obj0, obj1)
            );

            return centerPos;
        }

        // 数値
        public static float CenterValue(float pos0, float pos1) {
            return (pos0 - pos1) / 2f + pos0;
        }
        #endregion


        //----------------------------------
        //  Splitして指定した配列番号の値を
        //  取得する
        //----------------------------------
        #region Splitして指定した配列番号の値を取得
        public static int GetSplitInt(string message, string splitStr, int getNum) {
            string[] data = message.Split(splitStr[0]);
            return int.Parse(data[getNum]);
        }

        public static float GetSplitFloat(string message, string splitStr, int getNum) {
            string[] data = message.Split(splitStr[0]);
            return float.Parse(data[getNum]);
        }

        public static string GetSplitString(string message, string splitStr, int getNum) {
            string[] data = message.Split(splitStr[0]);
            return data[getNum];
        }

        public static long GetSplitLong(string message, string splitStr, int getNum) {
            string[] data = message.Split(splitStr[0]);
            return long.Parse(data[getNum]);
        }

        public static int[] GetSplitInt(string message, string splitStr) {
            string[] data = message.Split(splitStr[0]);

            // intに変更
            int[] dataInt = new int[data.Length];
            for (int i = 0; i < dataInt.Length; i++) {
                dataInt[i] = int.Parse(data[i]);
            }

            return dataInt;
        }

        public static float[] GetSplitFloat(string message, string splitStr) {
            string[] data = message.Split(splitStr[0]);

            // floatに変更
            float[] dataFloat = new float[data.Length];
            for (int i = 0; i < dataFloat.Length; i++) {
                dataFloat[i] = float.Parse(data[i]);
            }

            return dataFloat;
        }

        public static long[] GetSplitLong(string message, string splitStr) {
            string[] data = message.Split(splitStr[0]);

            // longに変更
            long[] dataLong = new long[data.Length];
            for (int i = 0; i < dataLong.Length; i++) {
                dataLong[i] = long.Parse(data[i]);
            }

            return dataLong;
        }
        #endregion

        //----------------------------------
        //  区切り文字の処理
        //----------------------------------
        #region 指定した区切り文字で区切りListで値を返す
        public static List<string> GetSplitStringList(string str, string separate) {
            if (str == "") return null;

            List<string> dataList = new List<string>();

            string[] dataArray = str.Split(separate[0]);
            dataList.AddRange(dataArray);

            return dataList;
        }

        public static List<int> GetSplitIntList(string str, string separate) {
            if (str == "") return null;

            List<int> dataList = new List<int>();

            string[] dataArray = str.Split(separate[0]);
            for (int i = 0; i < dataArray.Length; i++) {
                dataList.Add(int.Parse(dataArray[i]));
            }

            return dataList;
        }

        public static List<float> GetSplitFloatList(string str, string separate) {
            if (str == "") return null;

            List<float> dataList = new List<float>();

            string[] dataArray = str.Split(separate[0]);
            for (int i = 0; i < dataArray.Length; i++) {
                dataList.Add(float.Parse(dataArray[i]));
            }

            return dataList;
        }
        #endregion

        #region Listを指定した区切り文字で区切ったstringを返す
        public static string GetSeparatedString(List<string> dataList, string separate) {
            if (dataList == null || dataList.Count == 0) return "";

            string data = "";

            for (int i = 0; i < dataList.Count - 1; i++) {
                data += dataList[i] + separate;
            }
            data += dataList[dataList.Count - 1];

            return data;
        }

        public static string GetSeparatedString(List<int> dataList, string separate) {
            if (dataList == null || dataList.Count == 0) return "";

            string data = "";

            for (int i = 0; i < dataList.Count - 1; i++) {
                data += dataList[i] + separate;
            }
            data += dataList[dataList.Count - 1];

            return data;
        }

        public static string GetSeparatedString(List<float> dataList, string separate) {
            if (dataList == null || dataList.Count == 0) return "";

            string data = "";

            for (int i = 0; i < dataList.Count - 1; i++) {
                data += dataList[i] + separate;
            }
            data += dataList[dataList.Count - 1];

            return data;
        }
        #endregion


        //----------------------------------
        //  オブジェクトのListを小さい順番
        // で返す。(id、数値)
        //----------------------------------
        #region GetOrderList
        public static List<OrderData> GetOrderList(List<string> idList, List<float> valueList, Direction direction) {

            List<OrderData> userDataList = new List<OrderData>();
            for (int i = 0; i < idList.Count; i++) {
                OrderData thisData = new OrderData();
                thisData.id = idList[i];
                thisData.value = valueList[i];
                userDataList.Add(thisData);
            }

            if (direction == Direction.Up) userDataList.Sort((a, b) => Math.Sign(b.value - a.value));
            else userDataList.Sort((a, b) => Math.Sign(a.value - b.value));

            return userDataList;
        }
        #endregion

        //----------------------------------
        //  四捨五入
        //----------------------------------
        #region 四捨五入
        public static float Round(float value) {
            return (float)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static int RoundToInt(float value) {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }
        #endregion


        //----------------------------------
        //  検索文字がリストにあるかどうか
        //----------------------------------
        #region 検索文字がリストにあるかどうか
        public static bool MatchWord(List<string> wordList, string matchWord) {
            bool match = false;

            for (int i = 0; i < wordList.Count; i++) {
                if (wordList[i] == matchWord) {
                    match = true;
                    break;
                }
            }

            return match;
        }
        #endregion

        #region 該当id検索してリスト番号を返す
        public static int GetListNum(List<string> idList, string id) {
            int listNum = -1;

            for (int i = 0; i < idList.Count; i++) {
                if (idList[i] == id) {
                    listNum = i;
                    break;
                }
            }

            return listNum;
        }
        #endregion


        //----------------------------------
        //  等間隔に並べる
        //----------------------------------
        #region 等間隔
        public static void EquidistantX(List<GameObject> objList, float startX, float endX, float posY) {

            float areaWidth = Mathf.Abs(endX - startX);
            float oneWidth = areaWidth / (float)(objList.Count + 1);

            for (int i = 0; i < objList.Count; i++) {
                objList[i].transform.localPosition = new Vector3(
                    startX + oneWidth * (i + 1),
                    posY,
                    0
                );
            }
        }

        public static void EquidistantX(List<GameObject> objList, float startX, float endX) {

            float areaWidth = Mathf.Abs(endX - startX);
            float oneWidth = areaWidth / (float)(objList.Count + 1);

            for (int i = 0; i < objList.Count; i++) {
                objList[i].transform.localPosition = new Vector3(
                    startX + oneWidth * (i + 1),
                    objList[i].transform.localPosition.y,
                    0
                );
            }
        }

        public static void EquidistantY(List<GameObject> objList, float startY, float endY, float posX) {

            float areaHeight = Mathf.Abs(endY - startY);
            float oneHeight = areaHeight / (float)(objList.Count + 1);

            for (int i = 0; i < objList.Count; i++) {
                objList[i].transform.localPosition = new Vector3(
                    posX,
                    startY + oneHeight * (i + 1),
                    0
                );
            }
        }

        public static void EquidistantY(List<GameObject> objList, float startY, float endY) {

            float areaHeight = Mathf.Abs(endY - startY);
            float oneHeight = areaHeight / (float)(objList.Count + 1);

            for (int i = 0; i < objList.Count; i++) {
                objList[i].transform.localPosition = new Vector3(
                    objList[i].transform.localPosition.x,
                    startY + oneHeight * (i + 1),
                    0
                );
            }
        }
        #endregion


        //----------------------------------
        //  Cameraの前にObjectを表示させる
        //----------------------------------
        #region Cameraの前にObjectを表示させる

        // カメラの前の座標を取得
        // camera: camera
        // distance: カメラからの距離
        // yFix: y軸を固定するかどうか
        // yPos: 固定するy位置(yFixがTrueのときのみ有効)
        public static Vector3 GetPosInFrontOfCamera(Camera camera, float distance, bool yFix, float yPos = 0)
        {
            var direction = Quaternion.Euler(camera.transform.eulerAngles) * Vector3.forward;

            float y = direction.y;
            if (yFix) y = yPos;

            return camera.transform.position + new Vector3(direction.x, y, direction.z) * distance;
        }

        // カメラの前にObjectを配置する
        // camera: camera
        // targetObj: 配置するGameObject
        // distance: カメラからの距離
        // yFix: y軸を固定するかどうか
        // yPos: 固定するy位置(yFixがTrueのときのみ有効)
        // cameraDirectionOn: カメラの方向に回転させるかどうか
        // cameraDirectionXFixOn: Trueのときxの回転角度は0となる(cameraDirectionOnがTrueのときのみ有効)
        public static void SetObjectInFrontOfCamera(Camera camera, GameObject targetObj, float distance, bool yFix, float yPos = 0, bool cameraDirectionOn = true, bool cameraDirectionXFixOn = false)
        {
            targetObj.transform.position = GetPosInFrontOfCamera(camera, distance, yFix, yPos);
            if (cameraDirectionOn)
            {
                if(cameraDirectionXFixOn) targetObj.transform.rotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, camera.transform.rotation.eulerAngles.z);
                else targetObj.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles);
            }
        }
        #endregion

        //----------------------------------
        //  ui(スクロール)でfitさせている
        //  ときのlayoutGroupの更新 
        //----------------------------------
        #region (layoutGroupの更新)
        public void LayoutChange(VerticalLayoutGroup layoutGroup)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
        }
        public void LayoutChange(HorizontalLayoutGroup layoutGroup)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
        }
        #endregion

        //----------------------------------
        //  日本語関連
        //----------------------------------
        // 文字列のカウント(日本語は2, 英数字は1としてカウント)
        public int TextLengthJPN(string message)
        {
            int textLength = 0;
            for (int i = 0; i < message.Length; i++)
            {
                string word = message.Substring(i, 1);
                if (IsJPN(word)) textLength += 2;
                else textLength += 1;
            }

            return textLength;
        }

        // 文字列に日本語が入っているかどうか
        public bool IsJPN(string message)
        {
            var isJapanese = Regex.IsMatch(message, @"[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+");
            return isJapanese;
        }
    }
}
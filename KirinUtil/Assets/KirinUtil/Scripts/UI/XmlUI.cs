using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KirinUtil;

namespace KirinUtil {
    public class XmlUI : MonoBehaviour {

        [Serializable]
        public class TagList {
            public List<Tags> tags = new List<Tags>();

            public void Init() {
                tags = null;
            }
        }

        [Serializable]
        public class Tags {
            public string tag;
            public string title;

            public void Init() {
                tag = "";
                title = "";
            }
        }
        // -------------- 登録 -------------------
        public List<TagList> tagData;

        private string[] tagNameArray;
        private List<List<InputField>> settingInputList;
        private string[] tagValueArray;

        public GameObject settingInputPrefab;
        public GameObject settingUI;
        public float inputInitY;
        public float uiMargin = 15;
        public string settingFilePath = "/../../AppData/Setting/app_setting.xml";

        //----------------------------------
        //  登録
        //----------------------------------
        #region init

        // ここに初期値を登録していく
        private void RegistrationInit() {
            /* 例
            settingInputList[0][9].text = terrainManager.pusherHandleMarginX + "";
            settingInputList[0][10].text = terrainManager.handleYTop + "";
            settingInputList[0][11].text = terrainManager.handleYBottom + "";
            */

        }
        #endregion

        #region inputEnd時
        private void RegistrationInputEnd(int categoryNum, int inputNum, string data) {
            /* 例
            if (categoryNum == 0) {
                if (inputNum == 0) {
                    terrainManager.groundSetPos.x = float.Parse(data);
                    terrainManager.SetGroundPos();
                } else if (inputNum == 1) {
                    terrainManager.groundSetPos.y = float.Parse(data);
                    terrainManager.SetGroundPos();
                }
            }*/
        }
        #endregion

        //----------------------------------
        //  Setting
        //----------------------------------
        #region init

        void Start() {
            //SettingInit();
        }

        public void SettingInit() {

            settingFilePath = Application.dataPath + settingFilePath;

            settingUI.SetActive(false);

            int categoryNum = GetCategoryNum();
            int elementNum = GetAllElementNum();

            print(categoryNum + "  " + elementNum);
            tagNameArray = new string[elementNum];
            tagValueArray = new string[elementNum];

            // 高さ設定
            Transform inputTrf = settingInputPrefab.transform.Find("input");
            Transform titleTrf = settingInputPrefab.transform.Find("title");
            float uiHeight = inputTrf.GetComponent<RectTransform>().sizeDelta.y;
            float uiHeight2 = titleTrf.GetComponent<RectTransform>().sizeDelta.y;
            if (uiHeight < uiHeight2) uiHeight = uiHeight2;

            // 横幅設定
            float uiWidth = inputTrf.GetComponent<RectTransform>().sizeDelta.x;
            float uiWidth2 = titleTrf.GetComponent<RectTransform>().sizeDelta.x;
            uiWidth += uiWidth2;

            settingInputList = new List<List<InputField>>();
            for (int i = 0; i < categoryNum; i++) {
                settingInputList.Add(InputInit(i, GetTileArray(i), uiHeight, uiWidth * i));
            }

            RegistrationInit();

            // 登録が終わったらこれを実行
            int arrayNum = 0;
            for (int i = 0; i < categoryNum; i++) {
                for (int j = 0; j < settingInputList[i].Count; j++) {
                    tagNameArray[arrayNum + j] = tagData[i].tags[j].tag;
                    tagValueArray[arrayNum + j] = settingInputList[i][j].text;
                }
                arrayNum += GetElementNum(i);
            }
        }

        private List<InputField> InputInit(int idNum, string[] name, float prefabHeight, float posX) {
            List<InputField> inputList = new List<InputField>();

            for (int i = 0; i < name.Length; i++) {

                GameObject thisInputObj = Util.media.CreateUIObj(
                    settingInputPrefab,
                    settingUI,
                    "input_" + idNum + "_" + i,
                    new Vector3(posX, inputInitY - (prefabHeight + uiMargin) * i, 0),
                    Vector3.zero,
                    Vector3.one
                );

                thisInputObj.GetComponentInChildren<Text>().text = name[i];

                InputField thiInput = thisInputObj.GetComponentInChildren<InputField>();
                inputList.Add(thiInput);
                string id = "," + idNum + "," + i;
                thiInput.onEndEdit.AddListener(value => SettingInputEnd(value + id));
            }

            return inputList;
        }

        private void SettingInputEnd(string st) {
            if (st == null) return;

            string[] data = st.Split(","[0]);
            if (data[0] == "" || data.Length < 3) return;

            int category = int.Parse(data[1]);
            int arrayNum = int.Parse(data[2]);
            // print("category: " + category + "  arrayNum: " + arrayNum);

            RegistrationInputEnd(category, arrayNum, data[0]);


            // 登録が終わったらこれを実行
            int arrayBaseNum = 0;
            for (int i = 0; i < category; i++) {
                arrayBaseNum += GetElementNum(i);
            }
            tagValueArray[arrayBaseNum + arrayNum] = data[0];
        }


        #endregion

        //----------------------------------
        //  Update & btn
        //----------------------------------
        #region Update

        // Update is called once per frame
        void Update() {
            SettingUpdate();
        }

        private void SettingUpdate() {
            if (Input.GetKeyUp(KeyCode.S)) {
                settingUI.SetActive(!settingUI.activeSelf);
            }
        }

        public void SaveBtnClick() {
            string xmlContents = Util.file.OpenTextFile(settingFilePath);
            for (int i = 0; i < tagNameArray.Length; i++) {
                print(tagNameArray[i] + ":" + tagValueArray[i]);
            }
            Util.file.SaveXml(xmlContents, tagNameArray, tagValueArray, settingFilePath);
        }

        #endregion

        //----------------------------------
        //  functions
        //----------------------------------
        #region functions
        // カテゴリの数を返す
        private int GetCategoryNum() {
            return tagData.Count;
        }

        // 全エレメントの数を返す
        private int GetAllElementNum() {
            int elementNum = 0;

            for (int i = 0; i < GetCategoryNum(); i++) {
                elementNum += tagData[i].tags.Count;
            }

            return elementNum;
        }

        // 指定したカテゴリ番号のタイトル配列を返す
        private string[] GetTileArray(int listNum) {
            int elementNum = GetElementNum(listNum);

            string[] titles = new string[elementNum];
            for (int i = 0; i < elementNum; i++) {
                titles[i] = tagData[listNum].tags[i].title;
            }

            return titles;
        }

        // 指定したカテゴリ番号の配列数を返す
        private int GetElementNum(int listNum) {
            return tagData[listNum].tags.Count;
        }

        #endregion

    }
}

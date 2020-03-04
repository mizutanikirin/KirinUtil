using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KirinUtil;
using System;
using System.Xml;
using UnityEngine.UI;

public class SampleManager : MonoBehaviour {

    public CountDown countDown;
    public Text countDownText;
    public SlideManager slide;
    public DialogManager dialog;

    public GameObject fadeObj;
    public GameObject fade2Obj;
    public GameObject brightObj;
    public GameObject parentObj;

    // balloon
    public BalloonMessageManager balloonMessage;
    public GameObject balloonParentObj;

    // 等間隔
    public List<GameObject> equidistantObjList;

    [Header("Setting")]
    public string rootDataDir = "/../../AppData/";
    public string settingDataDir = "Setting/";
    public string appXmlFileName = "app_setting.xml";

    // Use this for initialization
    void Start() {

        //Log.Print("test");

        //Screen.SetResolution(screenWidth, screenHeight, false, 60);

        Util.BasicSetting(false);

        Util.media.FadeOutUI(fadeObj, 5.0f, 0.0f);
        /*util.media.FadeOutGui(fade2Obj, 2.0f, 5.0f);

        util.media.ChangeTextureBright(brightObj, 2.5f);
        util.media.image.LoadUIImages();
        util.media.movie.LoadUIMovies();*/

        Util.sound.LoadSounds();

        //ReadJsonSample();

        // xml
        rootDataDir = Application.dataPath + rootDataDir;
        settingDataDir = rootDataDir + settingDataDir + "/";
        string appXmlContents = Util.file.OpenTextFile(settingDataDir + appXmlFileName);
        //XmlParse(appXmlContents, "app");

        // コルーチンの止め方
        // 結局の所止まらないからStopAllCoroutines();した方が確実。
        Coroutine retC = StartCoroutine(TestWait());
        StopCoroutine(retC);

        // カウントダウン
        countDown.SetCountDown(10, countDownText, false, 0, 5);
        //countDown.SetCountDown(10, util.media.sound, 0);

        // 直下のGameObjectのみ取得
        List<GameObject> childrenObj = Util.media.GetChildGameObject(parentObj, "child");
        for (int i = 0; i < childrenObj.Count; i++) {
            print("children: " + childrenObj[i].name);
        }

        // OSC
        OSCStart();

        // ランキング
        List<string> id = new List<string>();
        id.Add("aaa");
        id.Add("bbb");
        id.Add("ccc");
        id.Add("ddd");
        id.Add("eee");
        List<float> value = new List<float>();
        value.Add(5);
        value.Add(3);
        value.Add(2);
        value.Add(4);
        value.Add(1);
        List<OrderData> data = Util.GetOrderList(id, value, Direction.Down);
        string rank = "";
        for (int i = 0; i < data.Count; i++) {
            rank += data[i].id + ": " + data[i].value + Environment.NewLine;
        }
        print(rank);


        // 吹き出し
        balloonMessage.Create(balloonParentObj, 0);
        balloonMessage.PlayAll(0);
        balloonMessage.Create(balloonParentObj, 1);
        balloonMessage.PlayAll(1);
        StartCoroutine(BalloonWait());

        // 等間隔に整列
        Util.EquidistantX(equidistantObjList, -200, 300, 0);

        // スライド
        slide.Set();
        Util.movie.LoadUIMovies(); // スライド登録したあとにムービーをロードしないといけない
        slide.Play("slide1");

        // dialog
        //dialog.Popup("toast", Vector2.zero, "というわけです。", DialogManager.ButtonType.None);
        //dialog.Popup("okDialog", Vector2.zero, "わかりましたか？", DialogManager.ButtonType.OK);
        dialog.Popup("yesNoDialog", Vector2.zero, "どします？", DialogManager.ButtonType.YesNo);
        
        StartCoroutine(DebugWatch());
    }

    private IEnumerator DebugWatch() {
        Util.DebugWatchStart();
        yield return new WaitForSeconds(1.0f);
        Util.DebugWatchStop();
    }

    // toggleButton
    public void ToggleOn() {
        print("ToggleOn");
    }

    public void ToggleOff() {
        print("ToggleOff");
    }

    // dialog
    #region dialog
    public void DialogYesBtnClick(string id) {
        print("DialogYesBtnClick: " + id);
    }

    public void DialogNoBtnClick(string id) {
        print("DialogNoBtnClick: " + id);
    }

    public void DialogOkBtnClick(string id) {
        print("DialogOkBtnClick: " + id);
    }
    #endregion

    // 吹き出し
    private IEnumerator BalloonWait() {
        yield return new WaitForSeconds(5f);
        balloonMessage.StopAll(0);
    }

    private IEnumerator TestWait() {
        while (true) {
            print("loop Coroutine");
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void SoundLoaded() {
        print("Play");
        //util.media.sound.PlaySE(0);
    }

    public void ImageLoaded() {
        print("ImageLoaded");
    }

    public void MovieLoaded() {
        print("MovieLoaded");
        //util.media.movie.Play(0);
    }

    // Update is called once per frame
    void Update() {
        // countDown
        if (countDown.Update2()) {

        }

        // osc
        //OSCReceiveUpdate();
        OSCSendUpdate();
    }

    //----------------------------------
    //  OSC Sample
    //----------------------------------
    #region OSC
    public string ip;
    public int receivePort;
    public int sendPort;
    int count = 0;

    private void OSCStart() {
        Util.net.OSCStart(ip, sendPort, receivePort);
    }

    private void OSCSendUpdate() {
        Util.net.OSCSendUpdate("test", count.ToString());
        count++;
    }

    // KRNNetworkのイベントから呼び出される
    public void OSCReceived(List<NetManager.OSCData> oscData) {
        for (int i = 0; i < oscData.Count; i++) {
            print("OSCReceived: " + oscData[i].Key + "  " + oscData[i].Address + "  " + oscData[i].Data);
        }
    }
    #endregion

    //----------------------------------
    //  xml
    //----------------------------------
    #region xml main
    private void XmlParse(string xmlString, string type) {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        // system
        XmlNodeList nodes = xmlDoc.GetElementsByTagName("setting");

        foreach (XmlNode node in nodes) {
            foreach (XmlNode nodeItems in node) {
                if (type == "app") AppXmlParse(nodeItems);
            }
        }
    }
    #endregion

    #region AppXmlParse
    private void AppXmlParse(XmlNode node) {

        /*if (node.Name == "screenWidth") {
            screenWidth = int.Parse(node.InnerText);
        } else if (node.Name == "screenHeight") {
            screenHeight = int.Parse(node.InnerText);
        } else if (node.Name == "message") {
            XmlNodeList node0List = node.ChildNodes;

            foreach (XmlNode node0 in node0List) {
                if (node0.Name == "deveiceOpenError") {
                    deveiceOpenError = node0.InnerText;
                    test = float.Parse(node0.Attributes["power"].Value);
                } else if (node0.Name == "scanError") {
                    scanErrorMessage = node0.InnerText;
                }
            }
        } else if (node.Name == "ipAddress") {
            ipAddress = node.InnerText;
            print(ipAddress);
        } else if (node.Name == "portNum") {
            portNum = int.Parse(node.InnerText);
        }*/
    }

    #endregion

    // xmlでファイル名指定してからPlayImageを読み取りたいときの読み込み方例
    /*
    private void LoadImageContents() {

        util.media.image.playImages = new ImageManager.PlayImages[imageContentsDirPathList.Count * characterActionNum];

        // 各poseの読み込み
        for (int i = 0; i < imageContentsDirPathList.Count; i++) {

            List<GameObject> thisObjList = new List<GameObject>();

            for (int j = 0; j < characterActionNum; j++) {
                // parentObj作成
                GameObject thisParentObj = new GameObject();
                thisParentObj.transform.SetParent(charParentObj.transform, false);
                thisParentObj.name = imageContentsDirPathList[i] + "_" + j;
                thisObjList.Add(thisParentObj);

                // playimage作成
                util.media.image.playImages[i * characterActionNum + j] = new ImageManager.PlayImages();
                util.media.image.playImages[i * characterActionNum + j].id = imageContentsDirPathList[i] + "_" + j;
                util.media.image.playImages[i * characterActionNum + j].folderName = imageContentsDirPathList[i] + "/Pose" + j;
                if (j == 0) util.media.image.playImages[i * characterActionNum + j].loopStartFileName = "0.png";
                util.media.image.playImages[i * characterActionNum + j].scale = 1;
                util.media.image.playImages[i * characterActionNum + j].parentObj = thisParentObj;
                util.media.image.playImages[i * characterActionNum + j].pos = characterPosList[i];
            }

            characterObjList.Add(thisObjList);

        }

        util.media.image.LoadPlayImages();

    }
    */

    // xmlでファイル名指定してからSoundを読み取りたいときの読み込み方例
    /*
    private void LoadSound() {

        // bgm
        util.media.sound.bgmSound = new BGMSound[1];
        util.media.sound.bgmSound[0] = new BGMSound();
        util.media.sound.bgmSound[0].id = "";
        util.media.sound.bgmSound[0].fileName = soundFileName[0];
        util.media.sound.bgmSound[0].volume = soundVolumeList[0];
        util.media.sound.bgmSound[0].loop = true;

        // se
        util.media.sound.seSound = new SESound[6];
        for (int i = 0; i < 6; i++) {
            // sound登録
            util.media.sound.seSound[i] = new SESound();
            util.media.sound.seSound[i].id = "";
            util.media.sound.seSound[i].fileName = soundFileName[1 + i];
            util.media.sound.seSound[i].volume = soundVolumeList[1 + i];

        }

        util.media.sound.SEInit();
        util.media.sound.LoadSounds();
    }
    */

    // xmlでファイル名指定してからMovieを読み取りたいときの読み込み方例
    public void LoadMovie(GameObject movieUIPrefab, GameObject parentObj, List<string> filePath) {
        for (int i = 0; i < filePath.Count; i++) {
            GameObject movieObj = Util.media.CreateUIObj(movieUIPrefab, parentObj, "movieObj" + i, Vector3.zero, Vector3.zero, Vector3.one);
            Array.Resize(ref Util.movie.uiMovies, Util.movie.uiMovies.Length + 1);
            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1] = new MovieManager.UIMovies();
            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].fileName = "../../" + filePath[i];
            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].visible = true;
            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].loop = false;
            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].obj = movieObj;
            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].volume = 1;
        }
    }

    // json読み取りサンプル
    #region json sample
    /*
    {
        "playerScore":[
            {
              "playerId":"device0_20190124175739",
              "score": 123,
              "rank": 1

            },
            {
              "playerId":"device0_20190124175749",
              "score": 123,
              "rank": 1
            },
            {
              "playerId":"device0_20190124175750",
              "score": 123,
              "rank": 1
            },
            {
              "playerId":"device0_20190124175751",
              "score": 123,
              "rank": 1
            }
        ]
    }
    */
    [Serializable]
    public class PlayerScoreJsonData {
        public List<ScoreData> playerScore;
    }

    [Serializable]
    public class ScoreData {
        public string playerId;
        public int score;
        public int rank;
    }
    private PlayerScoreJsonData playerScoreData;
    private void ReadJsonSample() {
        string testJson = "{\"playerScore\":[{\"playerId\":\"device0_20190124175739\",\"score\": 123,\"rank\": 1},{\"playerId\":\"device0_20190124175749\",\"score\": 123,\"rank\": 1},{\"playerId\":\"device0_20190124175750\",\"score\": 123,\"rank\": 1},{\"playerId\":\"device0_20190124175751\",\"score\": 123,\"rank\": 1}]}";

        playerScoreData = JsonUtility.FromJson<PlayerScoreJsonData>(testJson);
        print(playerScoreData.playerScore);

        for (int i = 0; i < playerScoreData.playerScore.Count; i++) {
            print("playerId: " + playerScoreData.playerScore[i].playerId);
            print("score: " + playerScoreData.playerScore[i].score);
            print("rank: " + playerScoreData.playerScore[i].rank);
        }
    }
    #endregion
}

//#define MovieEnable

using KirinUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {
    public class SlideManager : MonoBehaviour {

        public GameObject imageUIPrefab;
        public GameObject movieUIPrefab;

        private List<GameObject> slideObjList;
        private List<float> slideTimeList;
        private bool isSlidePlay = false;
        private Timer slideTimer;
        private int slideNum = 0;
        private int playingSlideId;
        private int playingMovieNum;
        private bool isPlayingMovie;
        private float movieAllTime;

        private List<List<GameObject>> allObjList;
        private List<List<float>> allTimeList;
        private List<GameObject> allParentObjList;
        private List<string> idList;


        public enum RootPath {
            dataPath,
            persistentDataPath,
            temporaryCachePath,
            streamingAssetsPath
        }
        [SerializeField] RootPath rootPath;
        private string rootDataPath;


        [System.SerializableAttribute]
        public class SlideList {
            public List<Slide> slide = new List<Slide>();

            public SlideList(List<Slide> list) {
                slide = list;
            }
        }
        [SerializeField]
        private List<Slide> slideList = new List<Slide>();

        [Serializable]
        public class Slide {
            public string id;
            public GameObject parentObj;
            public string folderPath;
            public List<string> fileName;
            public List<float> timeList;
        }

        //----------------------------------
        //  event
        //----------------------------------
        #region event
        [System.Serializable]
        public class SlideEndEvent : UnityEngine.Events.UnityEvent<string> { }

        [SerializeField]
        private SlideEndEvent slideEndEvent = new SlideEndEvent();

        void OnEnable() {
            slideEndEvent.AddListener(SlideEnd);
        }

        void OnDisable() {
            slideEndEvent.RemoveListener(SlideEnd);
        }

        void SlideEnd(string id) {
            print("SlideEnd");
        }
        #endregion


        //----------------------------------
        //  init
        //----------------------------------
        private void Awake() {

            if (rootPath == RootPath.dataPath) {
                rootDataPath = Application.dataPath;
            } else if (rootPath == RootPath.persistentDataPath) {
                rootDataPath = Application.persistentDataPath;
            } else if (rootPath == RootPath.streamingAssetsPath) {
                rootDataPath = Application.streamingAssetsPath;
            } else {
                rootDataPath = Application.temporaryCachePath;
            }

            allObjList = new List<List<GameObject>>();
            allTimeList = new List<List<float>>();
            idList = new List<string>();
            allParentObjList = new List<GameObject>();
        }

        private void Start() {
        }

        //----------------------------------
        //  スライドコンテンツの作成
        //----------------------------------
        public void Set() {

            for (int i = 0; i < slideList.Count; i++) {
                List<GameObject> thisList = new List<GameObject>();

                for (int j = 0; j < slideList[i].fileName.Count; j++) {
                    string filePath = rootDataPath + slideList[i].folderPath + slideList[i].fileName[j];
                    string extention = Path.GetExtension(filePath);

                    string type = "";
                    if (extention == ".png" || extention == ".jpg" || extention == ".gif") type = "image";
#if MovieEnable
                    if (extention == ".mp4" || extention == ".mov") type = "movie";
#endif

                    if (type != "") {
                        print("slide set: " + filePath);

                        if (type == "image") {
                            GameObject imageObj = Util.media.CreateUIObj(imageUIPrefab, slideList[i].parentObj, "imageObj" + j, Vector3.zero, Vector3.zero, Vector3.one);
                            Util.image.LoadAndSetImage(filePath, imageObj.GetComponent<Image>());
                            thisList.Add(imageObj);
                        } else {
#if MovieEnable
                            GameObject movieObj = Util.media.CreateUIObj(movieUIPrefab, slideList[i].parentObj, "movieObj" + j, Vector3.zero, Vector3.zero, Vector3.one);
                            Array.Resize(ref Util.movie.uiMovies, Util.movie.uiMovies.Length + 1);
                            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1] = new MovieManager.UIMovies();
                            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].fileName = ".." + slideList[i].folderPath + slideList[i].fileName[j];
                            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].visible = true;
                            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].loop = false;
                            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].obj = movieObj;
                            Util.movie.uiMovies[Util.movie.uiMovies.Length - 1].volume = 1;
                            movieObj.name = "movieObj_" + (Util.movie.uiMovies.Length - 1);
                            thisList.Add(movieObj);

                            slideList[i].timeList[j] = -1;
#endif
                        }

                        if (i == 0) thisList[0].SetActive(true);
                        else thisList[i].SetActive(false);
                    }
                }

                allParentObjList.Add(slideList[i].parentObj);
                idList.Add(slideList[i].id);
                allObjList.Add(thisList);
                allTimeList.Add(slideList[i].timeList);

            }
        }

        //----------------------------------
        //  コントロール
        //----------------------------------
#region コントロール
        public void Play(string id) {
            print("slide Play: " + id);

            isSlidePlay = true;
            slideNum = 0;
            int listNum = GetSlideListNum(id);
            playingSlideId = listNum;
            slideObjList = allObjList[listNum];
            allParentObjList[listNum].SetActive(true);
            slideTimeList = allTimeList[listNum];

            // スライド表示
            for (int i = 1; i < slideObjList.Count; i++) {
                slideObjList[i].SetActive(false);
            }
            slideObjList[0].SetActive(true);

            if (slideObjList[slideNum].name.IndexOf("_"[0]) != -1) SlideMoviePlay();
            else SlideTimerInit();
        }

        public void Stop() {
            SlideStopMovie();

            slideObjList = new List<GameObject>();
            isSlidePlay = false;
        }

        private int GetSlideListNum(string id) {
            int listNum = -1;
            for (int i = 0; i < slideList.Count; i++) {
                if (slideList[i].id == id) {
                    listNum = i;
                    break;
                }
            }

            return listNum;
        }

        private void SlideTimerInit() {
            slideTimer = new Timer();
            slideTimer.LimitTime = slideTimeList[slideNum];
        }
#endregion

        private void Update() {
            if (!isSlidePlay) return;

            if (slideTimeList[slideNum] == -1) {
#if MovieEnable
                // 動画の場合
                float movieNowTime = Util.movie.GetCurrentTimeMs(playingMovieNum) / 1000f;

                if (isPlayingMovie) {
                    if (movieAllTime - 0.6f < movieNowTime) {
                        isPlayingMovie = false;
                        NextSlide();
                    }
                }
#endif
            } else {
                // 画像の場合
                if (slideTimer.Update()) {
                    NextSlide();
                }
            }
        }

        private void NextSlide() {
            print("NextSlide");

            slideNum++;
            if (slideObjList.Count <= slideNum) {
                // すべての再生が終わったら実行する
                Stop();
                slideEndEvent.Invoke(idList[playingSlideId]);
            } else {
                ChangeNextSlide();
            }
        }

        private void ChangeNextSlide() {
            if (slideTimeList[slideNum] != -1) SlideTimerInit();

            Util.media.FadeOutUI(slideObjList[slideNum - 1], 0.1f, 0.5f);
            Util.media.FadeInUI(slideObjList[slideNum], 0.5f, 0);

            if (slideObjList[slideNum].name.IndexOf("_"[0]) != -1) {
                SlideMoviePlay();
            }
        }

        private void SlideMoviePlay() {
#if MovieEnable
            string[] nameStr = slideObjList[slideNum].name.Split("_"[0]);
            playingMovieNum = int.Parse(nameStr[1]);
            Util.movie.Play(playingMovieNum);
            isPlayingMovie = true;
            movieAllTime = Util.movie.mediaPlayer[playingMovieNum].Info.GetDurationMs() / 1000f;
#endif
        }

        private void SlideStopMovie() {
#if MovieEnable
            if (isPlayingMovie) {
                string[] nameStr = slideObjList[slideNum].name.Split("_"[0]);
                playingMovieNum = int.Parse(nameStr[1]);
                Util.movie.Stop(playingMovieNum);
            }
            isPlayingMovie = false;
#endif
        }
    }
}

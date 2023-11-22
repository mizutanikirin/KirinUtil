using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KirinUtil
{
    public class AlphaMediaManager : MonoBehaviour
    {

        #region Vars(Common)
        [Separator("Common")]
        [SerializeField] private RootPath rootPath;
        [SerializeField] private string mediaDirPath = "/../../AppData/Data/AlphaMedia/";
        [SerializeField] private bool startLoad;

        private string rootDataPath;
        private enum RootPath
        {
            dataPath,
            persistentDataPath,
            temporaryCachePath,
            streamingAssetsPath
        }
        #endregion

        #region Medias
        [Header("Medias")]
        public Media[] medias;

        [Serializable]
        public class Media
        {
            public string id;
            public MediaType type;
            public bool isLoop;
            public bool playOnAwake;
            public bool startVisible;

            [Tooltip("Image: folder path, Video: file path")]
            public string path;
            public GameObject parentObj;
            [NonSerialized] public AlphaMediaPlayer mediaPlayer;

            public Vector3 pos;
            public float angle;
            public float scale;

            [Header("[Image Setting]")]
            public string loopStartFileName;
            public float framerate;
            [NonSerialized] public int loopStartNum;

            public Media()
            {
                id = "";
                type = MediaType.Image;
                isLoop = false;
                playOnAwake = false;
                startVisible = false;

                pos = Vector3.zero;
                angle = 0;
                scale = 1;

                path = "";
                parentObj = null;
                mediaPlayer = null;

                loopStartFileName = "";
                framerate = 1;
                loopStartNum = 0;
            }
        }

        public enum MediaType
        {
            Image,
            Video
        }
        #endregion

        #region Event
        [Separator("Event")]
        // Loaded event
        [SerializeField] private UnityEngine.Events.UnityEvent loadedEvent = new UnityEngine.Events.UnityEvent();

        // play end event
        [Serializable] private class PlayEndEvent : UnityEngine.Events.UnityEvent<string> { }
        [SerializeField] private PlayEndEvent playEndEvent = new PlayEndEvent();

        void OnEnable()
        {
            loadedEvent.AddListener(Loaded);
            playEndEvent.AddListener(PlayEnded);
        }

        void OnDisable()
        {
            loadedEvent.RemoveListener(Loaded);
            playEndEvent.RemoveListener(PlayEnded);
        }

        void Loaded()
        {
            //print("Loaded All Media");
        }

        void PlayEnded(string id)
        {
            //print("PlayEnded: " + id);
        }
        #endregion


        //----------------------------------
        //  Init
        //----------------------------------
        #region Init

        private void Start()
        {
            if (rootPath == RootPath.dataPath)
                rootDataPath = Application.dataPath;
            else if (rootPath == RootPath.persistentDataPath)
                rootDataPath = Application.persistentDataPath;
            else if (rootPath == RootPath.streamingAssetsPath)
                rootDataPath = Application.streamingAssetsPath;
            else
                rootDataPath = Application.temporaryCachePath;

            if (startLoad) LoadAlphaMedias();
        }
        #endregion

        #region LoadAlphaMedias
        public void LoadAlphaMedias()
        {
            for (int i = 0; i < medias.Length; i++)
            {
                if (medias[i].type == MediaType.Video) LoadMovie(i);
                else LoadImage(i);
            }

            loadedEvent.Invoke();
        }

        // 動画の場合の読み込み
        private void LoadMovie(int index)
        {
            Media media = medias[index];

            GameObject mediaObj = new GameObject(media.id);
            mediaObj.transform.SetParent(media.parentObj.transform, false);
            mediaObj.transform.localPosition = media.pos;

            // AddComponent
            AlphaMediaPlayer mediaPlayer = mediaObj.AddComponent<AlphaMediaPlayer>();
            VideoPlayer videoPlayer = mediaObj.AddComponent<VideoPlayer>();
            mediaObj.AddComponent<RawImage>();

            // mediaPlayerの登録
            media.mediaPlayer = mediaPlayer;

            // video setting
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = rootDataPath + mediaDirPath + media.path;
            videoPlayer.isLooping = true;
            videoPlayer.playOnAwake = false;

            // mediaObjの設定
            mediaObj.transform.localScale = 
                new Vector3(media.scale, media.scale, 1);
            mediaObj.SetActive(media.startVisible);
            mediaPlayer.Init(media, this);
        }

        // 連番画像の場合の読み込み
        private void LoadImage(int index)
        {
            // init
            Media media = medias[index];
            media.loopStartNum = 0;
            List<string> filePaths = 
                Util.file.GetAllFilePath(rootDataPath + mediaDirPath + media.path);
            int count = 0;
            GameObject mediaObj = new GameObject(media.id);
            mediaObj.transform.SetParent(media.parentObj.transform, false);
            AlphaMediaPlayer mediaPlayer = mediaObj.AddComponent<AlphaMediaPlayer>();

            mediaObj.transform.localPosition = media.pos;
            mediaObj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, media.angle);

            for (int i = 0; i < filePaths.Count; i++)
            {
                if (Path.GetExtension(filePaths[i]) == ".meta") continue;

                Texture2D texture = LoadTexture2D(filePaths[i]);

                // ファイル名から画像の順番に変換
                string fileName = Path.GetFileName(filePaths[i]);
                if (fileName == media.loopStartFileName)
                {
                    media.loopStartNum = i;
                }

                // 画像作成
                GameObject obj = new GameObject("image" + count);
                RawImage image = obj.AddComponent<RawImage>();
                obj.transform.SetParent(mediaObj.transform, false);

                image.GetComponent<RectTransform>().sizeDelta = 
                    new Vector2(texture.width, texture.height);
                image.texture = texture;
                obj.SetActive(false);
                UnloadTexture(texture);
                count++;
            }

            // mediaPlayerの登録
            media.mediaPlayer = mediaPlayer;

            // mediaObjの設定
            mediaObj.transform.localScale = 
                new Vector3(media.scale, media.scale,1);
            mediaObj.SetActive(media.startVisible);
            mediaPlayer.Init(media, this);

        }
        #endregion


        //----------------------------------
        //  Control
        //----------------------------------
        #region Control
        public void Play(string id)
        {
            int index = GetListNum(id);
            if (index == -1) return;

            medias[index].mediaPlayer.Play();
        }

        public void PlayEnd(string id)
        {
            playEndEvent.Invoke(id);
        }

        public void Pause(string id)
        {
            int index = GetListNum(id);
            if (index == -1) return;

            medias[index].mediaPlayer.Pause();
        }

        public void Stop(string id)
        {
            int index = GetListNum(id);
            if (index == -1) return;

            medias[index].mediaPlayer.Stop();
        }

        public void Destroy(string id)
        {
            int index = GetListNum(id);
            if (index == -1) return;

            medias[index].mediaPlayer.Destroy();
        }
        #endregion


        //----------------------------------
        //  Common
        //----------------------------------
        #region ImageManager
        private Texture2D LoadTexture2D(string path)
        {

            if (!File.Exists(path)) return null;

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(LoadByte(path));

            if (texture != null) print("imageLoaded:" + path);
            else print("imageLoadError:" + path);

            return texture;
        }

        private byte[] LoadByte(string path)
        {
            if (path == null || path == "")
                return null;
            if (!File.Exists(path))
                return null;

            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] buf = br.ReadBytes((int)br.BaseStream.Length);
            br.Close();

            return buf;
        }

        private void UnloadTexture(Texture texture)
        {
            texture = null;
            Resources.UnloadAsset(texture);
            Resources.UnloadUnusedAssets();
        }
        #endregion

        #region Common
        private int GetListNum(string id)
        {
            int num = -1;

            if (medias != null)
            {
                for (int i = 0; i < medias.Length; i++)
                {
                    if (medias[i].id == id) return i;
                }
            }

            if(num == -1) Debug.LogWarning("Not found alphaMedia id");

            return num;
        }
        #endregion
    }
}
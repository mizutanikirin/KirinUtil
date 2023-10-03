using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {
    public class ImageManager : MonoBehaviour {

        //----------------------------------
        //  Common
        //----------------------------------
        #region Common
        public enum RootPath {
            dataPath,
            persistentDataPath,
            temporaryCachePath,
            streamingAssetsPath
        }

        [Separator("Common")]
        [SerializeField] RootPath rootPath;
        public string imageDirPath = "/../../AppData/Data/Images/";
        #endregion

        //----------------------------------
        //  UI image
        //----------------------------------
        #region UI image
        [Serializable]
        public class Images {
            public string fileName = "";
            public GameObject obj;
            public bool visible;

            public void Init() {
                fileName = "";
                obj = null;
                visible = false;
            }
        }

        [Separator("UI Image")]
        public bool awakeLoad;
        [Tooltip("TextureをImageに割り当てる場合はこちらにファイル名とImageを登録する")]
        public Images[] images;

        [SerializeField]
        private UnityEngine.Events.UnityEvent LoadedImageEvent = new UnityEngine.Events.UnityEvent();

        #endregion


        //----------------------------------
        //  only texture
        //----------------------------------
        #region only texture
        [Separator("Only Texture")]
        public bool awakeLoadTexture;
        [Tooltip("Texture読み込みのみの場合こちらにファイル名を登録する")]
        public string[] textureFileNames;
        //[Tooltip("透過させるかどうか")]
        //public bool alphaIsTransparency;
        [NonSerialized] public List<Texture2D> textures;
        private string rootDataPath;
        #endregion

        //----------------------------------
        //  play image
        //----------------------------------
        #region play image
        [Serializable]
        public class PlayImages {
            public string id = "";
            public string folderName = "";
            public string loopStartFileName = "";
            public Vector3 pos = Vector3.zero;
            public float rotate = 0;
            public float scale = 1;
            public GameObject parentObj;
            [NonSerialized] public int loopStartNum = 0;

            public void Init() {
                id = "";
                folderName = "";
                loopStartFileName = "";
                pos = Vector3.zero;
                rotate = 0;
                scale = 1;
                loopStartNum = 0;
                parentObj = null;
            }
        }

        [Separator("PlayImage")]
        public bool awakeLoadPlayImage;
        [Tooltip("シーケンス再生画像の登録")]
        public PlayImages[] playImages;
        public int imageFrameRate;
        private List<List<GameObject>> playImageObjList = new List<List<GameObject>>();
        private List<int> playImageNum = new List<int>();
        private List<int> playImageCurrentFrame = new List<int>();

        [System.Serializable]
        public class PlayImageEndEvent : UnityEngine.Events.UnityEvent<int> { }

        [SerializeField]
        private PlayImageEndEvent playImageEndEvent = new PlayImageEndEvent();

        #endregion

        //----------------------------------
        //  Init / Event
        //----------------------------------
        #region Init / Event
        void OnEnable() {
            LoadedImageEvent.AddListener(Loaded);
            playImageEndEvent.AddListener(PlayEnded);
        }

        void OnDisable() {
            LoadedImageEvent.RemoveListener(Loaded);
            playImageEndEvent.RemoveListener(PlayEnded);
        }

        void Loaded() {
            print("Loaded All Image");
        }
        void LoadedMaterialImage() {
            print("Loaded All Image");
        }

        void PlayEnded(int playNum) {
            print("PlayEnded Image: " + playNum);
        }

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

            if (awakeLoad) LoadImages();
            if (awakeLoadTexture) LoadTexture2DList();
            if (awakeLoadPlayImage) LoadPlayImages();
        }
        #endregion


        //----------------------------------
        //  LoadUIImages
        //----------------------------------
        public void LoadImages() {
            print("LoadUIImages");

            for (int i = 0; i < images.Length; i++) {
                if (images[i].fileName != "") {
                    Texture2D texture = LoadTexture2D(rootDataPath + imageDirPath + images[i].fileName);

                    if (images[i].obj != null) {
                        Image image = images[i].obj.GetComponent<Image>();
                        if (image != null) {
                            // ui imageの場合
                            image.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
                            image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero);
                            images[i].obj.SetActive(images[i].visible);
                        } else {
                            // ui rawimageの場合
                            RawImage rawImage = images[i].obj.GetComponent<RawImage>();
                            if (rawImage != null) {
                                rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
                                rawImage.texture = texture;
                                images[i].obj.SetActive(images[i].visible);
                            } else {
                                // 3d object imageの場合
                                Material material = images[i].obj.GetComponent<Renderer>().material;
                                if (material != null) {
                                    material.mainTexture = texture;
                                    images[i].obj.SetActive(images[i].visible);
                                }
                            }
                        }
                    }

                    UnloadTexture(texture);
                }
            }

            LoadedImageEvent.Invoke();
        }



        //----------------------------------
        //  load image
        //----------------------------------
        #region load image
        // 指定した画像をロードしてimageにセットする
        public void LoadAndSetImage(string path, Image thisImage) {

            Texture2D texture = LoadTexture2D(path);

            thisImage.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
            thisImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero);

            UnloadTexture(texture);
        }

        public void LoadAndSetRawImage(string path, RawImage thisImage) {

            Texture2D texture = LoadTexture2D(path);

            thisImage.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
            thisImage.texture = texture;

            UnloadTexture(texture);
        }

        // 指定した画像を読み込みtextureを返す
        public Texture2D LoadTexture2D(string path) {

            if (path == null || path == "")
                return null;

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(LoadByte(path));

            if (texture != null) print("imageLoaded:" + path);
            else print("imageLoadError:" + path);

            return texture;
        }

        public byte[] LoadByte(string path) {
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
        #endregion


        //----------------------------------
        //  image play
        //----------------------------------
        #region image play

        private Coroutine playCoroutine = null;

        // 0: id
        // 1: folderPath
        // 2: loopStartFileName
        // 3: scale
        // 4: kinectEnable
        // 5: kinectPosX
        // 6: kinectPosY
        // 7: kinectRotate
        // 8: fixedPosX
        // 9: fixedPosY
        //10: fixedRotate
        public void LoadPlayImages() {

            for (int i = 0; i < playImages.Length; i++) {

                // 2. フォルダの画像を読み込んでimagesに入れる
                print(rootDataPath + playImages[i].folderName);
                List<string> filePaths = Util.file.GetAllFilePath(rootDataPath + imageDirPath + playImages[i].folderName);
                List<GameObject> thisAllObj = new List<GameObject>();

                int count = 0;
                for (int j = 0; j < filePaths.Count; j++) {
                    if (Path.GetExtension(filePaths[j]) == ".meta") continue;

                    Texture2D texture = LoadTexture2D(filePaths[j]);

                    // ファイル名から画像の順番に変換
                    string fileName = Path.GetFileName(filePaths[j]);
                    if (fileName == playImages[i].loopStartFileName) {
                        playImages[i].loopStartNum = j;
                    }

                    // 画像作成
                    GameObject obj = new GameObject("image" + count);
                    RawImage image = obj.AddComponent<RawImage>();

                    obj.transform.localPosition = playImages[i].pos;
                    obj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, playImages[i].rotate);
                    playImages[i].parentObj.transform.localScale = new Vector3(playImages[i].scale, playImages[i].scale, playImages[i].scale);

                    obj.transform.SetParent(playImages[i].parentObj.transform, false);

                    image.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
                    image.texture = texture;
                    obj.SetActive(false);
                    thisAllObj.Add(obj);
                    UnloadTexture(texture);
                    count++;
                }
                playImageObjList.Add(thisAllObj);

            }
        }

        public void PlayImage(int imageNum) {

            // 再生中かどうかチェック
            for (int i = 0; i < playImageNum.Count; i++) {
                if (playImageNum[i] == imageNum) {
                    return;
                }
            }

            GameObject parentObj = playImageObjList[imageNum][0].transform.parent.gameObject;
            parentObj.SetActive(true);
            for (int i = 0; i < playImageObjList[imageNum].Count; i++) {
                playImageObjList[imageNum][i].SetActive(false);
            }
            playImageObjList[imageNum][0].SetActive(true);

            playImageNum.Add(imageNum);
            playImageCurrentFrame.Add(0);

            StopPlayImageCoroutine();
            playCoroutine = StartCoroutine(PlayingImage());
        }

        public void PlayImage(string imageId) {

            int imageNum = -1;
            for (int i = 0; i < playImages.Length; i++) {
                if (imageId == playImages[i].id) {
                    imageNum = i;
                    break;
                }
            }

            print("PlayImage: " + imageId + " " + imageNum);

            if (imageNum == -1) print("PlayImage Error: " + imageId);
            else PlayImage(imageNum);
        }

        private void StopPlayImageCoroutine() {
            if (playCoroutine != null) {
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }
        }


        private IEnumerator PlayingImage() {
            yield return new WaitForSeconds(1 / (float)imageFrameRate);

            if (playImageNum == null || playImageNum.Count == 0) StopPlayImageCoroutine();


            for (int i = 0; i < playImageNum.Count; i++) {
                int imageNum = playImageNum[i];

                bool stopFlag = false;
                playImageCurrentFrame[i]++;
                if (playImageCurrentFrame[i] == playImageObjList[imageNum].Count) {
                    string startPos = playImages[imageNum].loopStartFileName;
                    if (startPos == "") {
                        playImageEndEvent.Invoke(imageNum);
                        StopImage(imageNum);
                        stopFlag = true;
                    } else {
                        playImageCurrentFrame[i] = playImages[imageNum].loopStartNum;
                    }
                }

                if (!stopFlag) {
                    int currntFrame = playImageCurrentFrame[i];
                    for (int j = 0; j < playImageObjList[imageNum].Count; j++) {
                        playImageObjList[imageNum][j].SetActive(false);
                    }
                    playImageObjList[imageNum][currntFrame].SetActive(true);

                }
            }

            playCoroutine = StartCoroutine(PlayingImage());
        }

        public void StopImage(int imageNum) {

            int stopImageNum = -1;
            for (int i = 0; i < playImageNum.Count; i++) {
                if (playImageNum[i] == imageNum) {
                    stopImageNum = i;
                    break;
                }
            }

            if (stopImageNum == -1) return;

            GameObject parentObj = playImageObjList[imageNum][0].transform.parent.gameObject;
            parentObj.SetActive(false);
            for (int j = 0; j < playImageObjList[imageNum].Count; j++) {
                playImageObjList[imageNum][j].SetActive(false);
            }
            playImageNum.RemoveAt(stopImageNum);
            playImageCurrentFrame.RemoveAt(stopImageNum);

            StopPlayImageCoroutine();
        }

        public void StopImage(string imageId) {
            int imageNum = -1;

            for (int i = 0; i < playImages.Length; i++) {
                if (imageId == playImages[i].id) {
                    imageNum = i;
                    break;
                }
            }

            if (imageNum == -1) print("StopImage Error: " + imageId);
            else StopImage(imageNum);
        }
        #endregion


        //----------------------------------
        //  UnloadTexture
        //----------------------------------
        // Textureをアンロードする
        public void UnloadTexture(Texture texture)
        {
            texture = null;
            Resources.UnloadAsset(texture);
            Resources.UnloadUnusedAssets();
        }


        //----------------------------------
        //  LoadTexture2DList
        //----------------------------------
        public void LoadTexture2DList()
        {

            print("LoadTexture2DList");

            textures = new List<Texture2D>();

            for (int i = 0; i < textureFileNames.Length; i++)
            {
                if (textureFileNames[i] != "")
                {
                    Texture2D texture = LoadTexture2D(rootDataPath + imageDirPath + textureFileNames[i]);
                    //texture.alphaIsTransparency = alphaIsTransparency;
                    textures.Add(texture);
                }
            }
        }

        //----------------------------------
        //  Texture2D -> Texture2D
        //----------------------------------
        private Texture2D texture2D = null;
        public Texture2D ToTexture2D(Texture texture)
        {
            int width = texture.width;
            int height = texture.height;

            texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
            RenderTexture renderTexture = new RenderTexture(width, height, 32);

            Graphics.Blit(texture, renderTexture);
            RenderTexture.active = renderTexture;

            Rect source = new Rect(0, 0, renderTexture.width, renderTexture.height);
            texture2D.ReadPixels(source, 0, 0);
            texture2D.Apply();
            RenderTexture.active = RenderTexture.active;

            return texture2D;
        }


        //----------------------------------
        //  LoadAlphaMaskImage
        //----------------------------------
        /*#region LoadAlphaMaskImage
        public void LoadAlphaMaskImage(string fileName, GameObject maskObj) {
            Texture2D texture = LoadTexture2D(Application.dataPath + imageDirPath + fileName);

            maskObj.GetComponent<ToJ.Mask>().MainTex = texture;
            maskObj.GetComponent<RectTransform>().sizeDelta = new Vector2(
                texture.width,
                texture.height
            );
            UnloadTexture(texture);
        }
        #endregion*/
    }
}

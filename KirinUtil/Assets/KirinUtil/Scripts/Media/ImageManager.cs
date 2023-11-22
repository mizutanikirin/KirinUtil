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
        //  Init / Event
        //----------------------------------
        #region Init / Event
        void OnEnable() {
            LoadedImageEvent.AddListener(Loaded);
        }

        void OnDisable() {
            LoadedImageEvent.RemoveListener(Loaded);
        }

        void Loaded() {
            print("Loaded All Image");
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
        }
        #endregion


        //----------------------------------
        //  LoadUIImages
        //----------------------------------
        #region LoadUIImages
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

        #endregion


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

            if (!File.Exists(path)) return null;

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

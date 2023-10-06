using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil
{
    public class AssetBundleManager : MonoBehaviour
    {
        public enum RootPath
        {
            dataPath,
            persistentDataPath,
            temporaryCachePath,
            streamingAssetsPath
        }

        [SerializeField] RootPath rootPath;
        [SerializeField] private string assetDirPath = "/../../AppData/Data/AssetBundle/";
        private string rootDataPath;
        private bool initialized = false;

        //----------------------------------
        //  Init
        //----------------------------------
        #region Init
        // Start is called before the first frame update
        void Start()
        {
            if (!initialized) Init();
        }

        private void Init()
        {
            initialized = true;

            if (rootPath == RootPath.dataPath)
                rootDataPath = Application.dataPath;
            else if (rootPath == RootPath.persistentDataPath)
                rootDataPath = Application.persistentDataPath;
            else if (rootPath == RootPath.streamingAssetsPath)
                rootDataPath = Application.streamingAssetsPath;
            else
                rootDataPath = Application.temporaryCachePath;
        }
        #endregion


        //----------------------------------
        //  Load AssetBundle
        //----------------------------------
        // load assetbundle (common)
        private Object[] LoadAssetBundle(string assetName)
        {
            if (!initialized) Init();

            var assetBundle = AssetBundle.LoadFromFile(rootDataPath + assetDirPath + assetName);
            if (assetBundle == null)
            {
                Debug.LogWarning("Failed to load AssetBundle!");
                return null;
            }

            Object[] objects = assetBundle.LoadAllAssets();

            // AssetBundleのメタ情報をアンロード
            assetBundle.Unload(false);

            return objects;
        }

        // objectの読み込み
        public List<Object> LoadObjects(string assetName)
        {
            Object[] objects = LoadAssetBundle(assetName);
            if(objects == null) return null;

            List<Object> objList = new List<Object>();
            objList.AddRange(objects);

            return objList;
        }

        // GameObjectの読み込み
        public List<GameObject> LoadGameObjects(string assetName, GameObject parentObj, bool initVisible = false)
        {
            Object[] objects = LoadAssetBundle(assetName);
            if (objects == null) return null;

            List<GameObject> assetObjs = new List<GameObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                GameObject obj = Instantiate((GameObject)objects[i]);
                obj.transform.SetParent(parentObj.transform, false);
                obj.SetActive(initVisible);
                assetObjs.Add(obj);
            }

            return assetObjs;
        }
    }

}
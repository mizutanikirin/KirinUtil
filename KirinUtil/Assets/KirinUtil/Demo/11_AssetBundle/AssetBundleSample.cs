using KirinUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleSample : MonoBehaviour
{
    [SerializeField] private AssetBundleManager assetBundleManager;
    [SerializeField] private string assetBundleName;
    [SerializeField] private GameObject parentObj;

    // Start is called before the first frame update
    void Start()
    {
        assetBundleManager.LoadGameObjects(assetBundleName, parentObj, true);
    }
}

using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportKirinUtil
{
    [MenuItem("KirinUtil/Export Package")]
    public static void ExportCustomPackage()
    {
        // 除外するファイルやフォルダのパスを指定
        string[] excludePaths = new string[]
        {
            "Assets/KirinUtil/Scripts/Editor/ExportKirinUtil.cs",
            "Assets/KirinUtil/Scripts/Util/AutoDestroyMaterials.cs",
            "Assets/KirinUtil/Scripts/Util/FollowingCamera.cs",
            "Assets/KirinUtil/Scripts/Util/SceneViewCamera.cs",
            "Assets/KirinUtil/Scripts/UI/RotateText.cs",
            "Assets/KirinUtil/Scripts/UI/TextOutline.cs",
            "Assets/KirinUtil/Scripts/Media/Blob.cs",
            "Assets/KirinUtil/Scripts/Media/BlobDetector.cs",
            "Assets/KirinUtil/Scripts/Media/SavWav.cs",
            "Assets/KirinUtil/Scripts/Media/SoundRecorder.cs",
            "Assets/KirinUtil/Scripts/Mobile/DetectSwipe.cs",
            "Assets/KirinUtil/Scripts/Network/InternetAccessChecker.cs",
            "Assets/KirinUtil/Fonts",
            "Assets/ThirdLib"
        };

        // 除外するファイルやフォルダをHashSetに追加（高速な検索のため）
        HashSet<string> excludeSet = new HashSet<string>(excludePaths);

        // エクスポートするアセットのリスト
        List<string> exportAssets = new List<string>();

        // プロジェクト内のすべてのアセットを走査
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        foreach (var asset in allAssets)
        {
            // アセットが除外リストにない場合のみリストに追加
            if (!excludeSet.Contains(asset))
            {
                exportAssets.Add(asset);
            }
        }

        // エクスポートするアセットがあればパッケージを作成
        if (exportAssets.Count > 0)
        {
            AssetDatabase.ExportPackage(exportAssets.ToArray(), "../Release/KirinUtil_New.unitypackage", ExportPackageOptions.Recurse);
            Debug.Log("Custom package exported.");
        }
        else
        {
            Debug.Log("No assets to export.");
        }
    }
}

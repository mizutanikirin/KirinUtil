using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using KirinUtil;

public class KRNUtilMenu : Editor {

    //----------------------------------
    //  Add Class/MovieManager
    //----------------------------------
    #region Add Class/MovieManager
    [MenuItem("KirinUtil/Add Class/MovieManager", false, 1)]
    private static void AddMovieManager() {

        bool isOK = EditorUtility.DisplayDialog("Remove MovieManager", "MovieManagerを追加しますか？", "Yes", "No");
        if (!isOK) return;

        string filePath = Application.dataPath + "/KirinUtil/Scripts/Media/MovieManager.cs";
        string backupPath = filePath + ".backup";

        if (File.Exists(backupPath)) {

            // KRNUtilEditor
            DefineComment(Application.dataPath + "/KirinUtil/Editor/KRNUtilEditor.cs", "MovieEnable", true);

            // Util
            DefineComment(Application.dataPath + "/KirinUtil/Scripts/Util.cs", "MovieEnable", true);

            // SlideManager
            DefineComment(Application.dataPath + "/KirinUtil/Scripts/UI/SlideManager.cs", "MovieEnable", true);

            // MovieManagerの追加
            if (File.Exists(backupPath + ".meta")) File.Delete(backupPath + ".meta");
            File.Move(backupPath, filePath);

            Debug.Log("MovieManagerを追加しました。");
        } else {
            Debug.Log(backupPath + "が存在しません。");
        }
    }

    #endregion

    #region Add Class/QRManager
    [MenuItem("KirinUtil/Add Class/QRManager", false, 1)]
    private static void AddQRManager() {

        bool isOK = EditorUtility.DisplayDialog("Remove QRManager", "QRManagerを追加しますか？", "Yes", "No");
        if (!isOK) return;

        string filePath = Application.dataPath + "/KirinUtil/Scripts/Media/QRManager.cs";
        string backupPath = filePath + ".backup";

        if (File.Exists(backupPath)) {
            DefineComment(Application.dataPath + "/KirinUtil/Editor/KRNUtilEditor.cs", "QREnable", true);

            if (File.Exists(backupPath + ".meta")) File.Delete(backupPath + ".meta");
            File.Move(backupPath, filePath);
            Debug.Log("QRManagerを追加しました。");
        } else {
            Debug.Log(backupPath + "が存在しません。");
        }
    }
    #endregion

    #region Add Class/PrintManager
    [MenuItem("KirinUtil/Add Class/PrintManager", false, 1)]
    private static void AddPrintManager() {

        bool isOK = EditorUtility.DisplayDialog("Remove PrintManager", "PrintManagerを追加しますか？", "Yes", "No");
        if (!isOK) return;

        string filePath = Application.dataPath + "/KirinUtil/Scripts/Util/PrintManager.cs";
        string backupPath = filePath + ".backup";

        if (File.Exists(backupPath)) {
            DefineComment(Application.dataPath + "/KirinUtil/Editor/KRNUtilEditor.cs", "PrintEnable", true);

            if (File.Exists(backupPath + ".meta")) File.Delete(backupPath + ".meta");
            File.Move(backupPath, filePath);
            Debug.Log("PrintManagerを追加しました。");
        } else {
            Debug.Log(backupPath + "が存在しません。");
        }
    }
    #endregion

    //----------------------------------
    //  Remove Class
    //----------------------------------
    #region Remove Class/MovieManager
    [MenuItem("KirinUtil/Remove Class/MovieManager", false, 1)]
    private static void DeleteMovieManager() {

        bool isOK = EditorUtility.DisplayDialog("Remove MovieManager", "本当にMovieManagerを削除しますか？", "Yes", "No");
        if (!isOK) return;

        string filePath = Application.dataPath + "/KirinUtil/Scripts/Media/MovieManager.cs";

        if (File.Exists(filePath)) {

            // KRNUtilEditor
            DefineComment(Application.dataPath + "/KirinUtil/Editor/KRNUtilEditor.cs", "MovieEnable", false);

            // Util
            DefineComment(Application.dataPath + "/KirinUtil/Scripts/Util.cs", "MovieEnable", false);

            // SlideManager
            DefineComment(Application.dataPath + "/KirinUtil/Scripts/UI/SlideManager.cs", "MovieEnable", false);

            // MovieManagerの削除
            if (File.Exists(filePath + ".meta")) File.Delete(filePath + ".meta");
            File.Move(filePath, filePath + ".backup");

            Debug.Log("MovieManagerを削除しました。");
        } else {
            Debug.Log("MovieManagerが存在しません。");
        }
    }

    #endregion

    #region Remove Class/QRManager
    [MenuItem("KirinUtil/Remove Class/QRManager", false, 1)]
    private static void DeleteQRManager() {

        bool isOK = EditorUtility.DisplayDialog("Remove QRManager", "本当にQRManagerを削除しますか？", "Yes", "No");
        if (!isOK) return;

        string filePath = Application.dataPath + "/KirinUtil/Scripts/Media/QRManager.cs";

        if (File.Exists(filePath)) {
            DefineComment(Application.dataPath + "/KirinUtil/Editor/KRNUtilEditor.cs", "QREnable", false);

            if (File.Exists(filePath + ".meta")) File.Delete(filePath + ".meta");
            File.Move(filePath, filePath + ".backup");
            Debug.Log("QRManagerを削除しました。");
        } else {
            Debug.Log(filePath + "が存在しません。");
        }
    }
    #endregion

    #region Remove Class/PrintManager
    [MenuItem("KirinUtil/Remove Class/PrintManager", false, 1)]
    private static void DeletePrintManager() {

        bool isOK = EditorUtility.DisplayDialog("Remove PrintManager", "本当にPrintManagerを削除しますか？", "Yes", "No");
        if (!isOK) return;

        string filePath = Application.dataPath + "/KirinUtil/Scripts/Util/PrintManager.cs";

        if (File.Exists(filePath)) {
            DefineComment(Application.dataPath + "/KirinUtil/Editor/KRNUtilEditor.cs", "PrintEnable", false);

            if (File.Exists(filePath + ".meta")) File.Delete(filePath + ".meta");
            File.Move(filePath, filePath + ".backup");
            Debug.Log("PrintManagerを削除しました。");
        } else {
            Debug.Log(filePath + "が存在しません。");
        }
    }
    #endregion

    //----------------------------------
    //  スクリーンショット
    //----------------------------------
    #region Screenshot
    [MenuItem("KirinUtil/Screenshot #%F12")]
    private static void ScreenShot() {

        var filename = System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png";

        ScreenCapture.CaptureScreenshot(filename);

        // GameViewを取得してくる
        var assembly = typeof(EditorWindow).Assembly;
        var type = assembly.GetType("UnityEditor.GameView");
        var gameview = EditorWindow.GetWindow(type);

        // GameViewを再描画
        gameview.Repaint();

        Debug.Log("ScreenShot: " + filename);
    }
    #endregion

    //----------------------------------
    //  functions
    //----------------------------------
    #region functions
    private static string OpenTextFile(string filePath) {

        FileInfo fi = new FileInfo(filePath);
        string returnSt = "";

        try {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), System.Text.Encoding.UTF8)) {
                returnSt = sr.ReadToEnd();
            }
        } catch (System.Exception e) {
            Debug.Log(e);
            returnSt = "";
        }

        return returnSt;
    }

    private static void WriteTextFile(string _filePath, string _contents, bool addWrite, string encode = "UTF-8") {

        StreamWriter sw;
        System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(encode);

        try {
            sw = new StreamWriter(_filePath, addWrite, encoding);
            sw.Write(_contents);
            sw.Close();
        } catch (System.Exception e) {
            Debug.Log(e);
        }
    }

    private static void DefineComment(string filePath, string defineVarName, bool toOn) {
        if (File.Exists(filePath)) {
            string code = OpenTextFile(filePath);
            if(!toOn) code = code.Replace("#define " + defineVarName, "//#define " + defineVarName);
            else code = code.Replace("//#define " + defineVarName, "#define " + defineVarName);
            WriteTextFile(filePath, code, false);

            if (File.Exists(filePath + ".meta")) File.Delete(filePath + ".meta");
        }
    }
    #endregion
}

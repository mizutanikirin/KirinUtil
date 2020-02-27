using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScreenShotEditor : Editor {

    [MenuItem("Edit/Screenshot #%F12")]
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
}

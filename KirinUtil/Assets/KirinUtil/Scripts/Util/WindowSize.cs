using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;

public class WindowSize : MonoBehaviour {

#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

#endif

    private Rect screenPosition = new Rect(0, 0, 1920, 1080);
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;

    void Start() {
    }

    public void Init(int posX, int posY, int screenWidth, int screenHeight) {
        int _x = posX;
        int _y = posY;
        int _w = screenWidth;
        int _h = screenHeight;

        if (_w != 0 && _h != 0)
            screenPosition = new Rect(_x, _y, _w, _h);

        SetWindowDirect();
    }

    public void SetWindowDirect() {

#if UNITY_STANDALONE_WIN
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
        bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
        print("setWindowDirect: " + result);
#endif
    }

    public void SetScreenMini() {
#if UNITY_STANDALONE_WIN
        print("SetScreenMini");
        ShowWindow(GetForegroundWindow(), 2);
#endif
    }

    public void SetScreenNormal() {
#if UNITY_STANDALONE_WIN
        print("SetScreenNormal");
        ShowWindow(GetForegroundWindow(), 9);
#endif
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;

public class WindowInit : MonoBehaviour {
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

    public Rect screenPosition = new Rect(0, 0, 1920, 1080);

    public string m_mytitle = "";

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;


    void Start() {
    }

    public void Init(int screenWidth, int screenHeight) {
        int _x = 0;// xmlSetting.GetInt("screenX");
        int _y = 0;// xmlSetting.GetInt("screenY");
        int _w = screenWidth;
        int _h = screenHeight;
        string _outstr = string.Format("window pos[ {0}, {1} ], size[ {2}, {3} ]", _x, _y, _w, _h);

        UnityEngine.Debug.Log(_outstr);
        if (_w != 0 && _h != 0)
            screenPosition = new Rect(_x, _y, _w, _h);

        setWindowDirect();
    }


    public void setWindowDirect() {
        bool result;

        if (m_mytitle != "") {
            SetWindowLong(FindWindow(null, m_mytitle), GWL_STYLE, WS_BORDER);
            result = SetWindowPos(FindWindow(null, m_mytitle), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
        } else {
            SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
            result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
        }

        print("setWindowDirect: " + result);
    }

    public void SetScreenMini() {
        if (m_mytitle != "") {
            ShowWindow(FindWindow(null, m_mytitle), 2);
        } else {
            ShowWindow(GetForegroundWindow(), 2);
        }
        print("set mini");
    }

    public void SetScreenNormal() {
        if (m_mytitle != "") {
            ShowWindow(FindWindow(null, m_mytitle), 8);
        } else {
            ShowWindow(GetForegroundWindow(), 8);
        }
    }

}

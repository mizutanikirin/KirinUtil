using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowForefront : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    void Start()
    {
#if !UNITY_EDITOR
        SetWindowTopMost();
#endif
    }

    void SetWindowTopMost()
    {
        var hwnd = GetActiveWindow();
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
    }
}

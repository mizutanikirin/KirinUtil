//--------------------------------------------------------------------------
//
//  iOSでサファリを表示させるClass
//
//--------------------------------------------------------------------------
using System.Runtime.InteropServices;
using UnityEngine;


namespace KirinUtil
{
    public static class SafariView
    {
#if UNITY_IOS
    [DllImport("__Internal")]
    extern static void launchUrl(string url);
    [DllImport("__Internal")]
    extern static void dismiss();
#endif

        //----------------------------------
        //  WebView表示
        //----------------------------------
        public static void LaunchURL(string url)
        {
#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_IOS
        launchUrl(url);
#endif
        }

        //----------------------------------
        //  WebView非表示
        //----------------------------------
        public static void Dismiss()
        {
#if UNITY_EDITOR
#elif UNITY_IOS
        dismiss();
#endif
        }
    }
}
#import <SafariServices/SafariServices.h>

extern UIViewController* UnityGetGLViewController();

extern "C"
{
    void launchUrl(const char *url)
    {
        // Unityが今表示しているViewControllerのインスタンスを取得
        UIViewController *uvc = UnityGetGLViewController();

        // C#側から渡されたC文字列を元に、NSURLオブジェクトを生成
        NSURL *URL = [NSURL URLWithString:[[NSString alloc] initWithUTF8String:url]];

        // 生成したURLから、SFSafariViewControllerオブジェクトを生成
        SFSafariViewController *sfvc = [[SFSafariViewController alloc] initWithURL:URL];

        // 生成したSFSafariViewControllerオブジェクトを起動
        [uvc presentViewController:sfvc animated:YES completion:nil];
    }

    void dismiss()
    {
        UIViewController * uvc = UnityGetGLViewController();
        [uvc dismissViewControllerAnimated:YES completion:nil];
    }
}
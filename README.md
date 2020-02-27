## 概要
KirinUtilはインスタレーション、体験型ゲームアプリを作成するときにコードを簡易化できる様々な機能が入ったユーティリティAssetです。

## 使い方
1. 様々なスクリプトに[iTween](https://assetstore.unity.com/packages/tools/animation/itween-84)を使っています。プロジェクトにiTweenをインポートください。
2. QRコードを作るQRManagerは「Zxing」を使用しています。QRManagerを使う場合[ここからDL](https://github.com/micjahn/ZXing.Net/releases)して`zxing.unity.dll、zxing.unity.pdb、zxing.unity.xml`を`Assets/Plugins`に追加してください。QRManagerを使わない場合QRManager.csを消してください。
3. 動画再生するMovieManagerは[AVProVideo](https://assetstore.unity.com/packages/tools/video/avpro-video-56355)が必要です。MovieManagerを使う場合プロジェクトに「AVProVideo」をインポートください。使わない場合、MovieManager.cs及びMovieManagerに関するコードを消してください。

1～3が終わったら適当なGameObjectを作成して`Inspector > Add Compoment > Util`してください。
そうするとUtil、KRNMedia、KRNFileが追加されます。

よく使うCompomentは以下のようにUtilのボタンから追加することができます。
![Util](https://user-images.githubusercontent.com/4795806/75427623-318e7500-598a-11ea-85cb-0bd23ecf1ca0.png)

※Utilを追加していないと他のKirinUtilのCompomentが使えない場合があります。

## 機能一覧
- Util
  計算を簡略化や位置の変更などする関数が使えます。
- KRNMedia
- KRNFile
- BillBoard
- CountDown
- Log
- PlayerPrefs2
- PrintManager
- ProcessManager
- StartMultiDisplay
- Timer
- WindowInit
- BalloonMessageManager
- DialogManager
- SetDropDownScrollPosition
- SlideManager
- SliderToText
- TextToSlider
- ToggleBtn
- UILine
- XmlUI
- HttpConnect
- NetManager
- UDPReceiveManager
- UDPSendManager
- CaptureManager
- ImageManager
- MovieManager
- QRManager
- SoundManager
- SoundRecorder
- StopTween
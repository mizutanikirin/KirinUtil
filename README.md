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
### Util
- <b>[Util](https://github.com/mizutanikirin/KirinUtil/wiki/Util)</b>  
  計算を簡略化や位置の変更などする関数が使えます。  
  
- <b>[KRNFile](https://github.com/mizutanikirin/KirinUtil/wiki/KRNFile)</b>  
  KRNFileにはファイルを扱うときに簡略化できる関数を用意しています。  
  
- <b>[Log](https://github.com/mizutanikirin/KirinUtil/wiki/Log)</b>  
  EditorのConsoleに表示される内容をuGUIのtextに表示可能、htmlとして保存することができます。  

- <b>[PrintManager](https://github.com/mizutanikirin/KirinUtil/wiki/PrintManager)</b>  
  PrintManagerでは印刷関連の関数を使うことができます。  
  
- <b>[ProcessManager](https://github.com/mizutanikirin/KirinUtil/wiki/ProcessManager)</b>  
  ProcessManagerでは外部アプリを実行/終了制御ができる関数を集めたComponentです。  
  
- <b>[Timer](https://github.com/mizutanikirin/KirinUtil/wiki/Timer)</b>  
  Timerはいわゆるタイマーの機能を持っているComponentです。  
  
- <b>[CountDown](https://github.com/mizutanikirin/KirinUtil/wiki/CountDown)</b>  
  カウントダウンを簡単に実装できます。  
  
- <b>[PlayerPrefs2](https://github.com/mizutanikirin/KirinUtil/wiki/PlayerPrefs2)</b>  
  PlayerPrefs2はPlayerPrefsを拡張したスクリプトです。  
  
- <b>[BillBoard](https://github.com/mizutanikirin/KirinUtil/wiki/BillBoard)</b>  
  GameObjectが常に指定したカメラの方向を向きます。  
  
- <b>[StartMultiDisplay](https://github.com/mizutanikirin/KirinUtil/wiki/StartMultiDisplay)</b>  
  アプリが複数ディスプレイで表示できるようにします。
  
- <b>[WindowSize](https://github.com/mizutanikirin/KirinUtil/wiki/WindowSize)</b>  
  アプリのウィンドウを指定した位置、大きさにします。また最小化、最小化前のウィンドウ表示に戻すことができます。  
  
- <b>[StopTween](https://github.com/mizutanikirin/KirinUtil/wiki/StopTween)</b>  
  GameObjectがOnDisable()、OnDestroy()したときにiTweenを止めます。
### Media
- <b>[KRNMedia](https://github.com/mizutanikirin/KirinUtil/wiki/KRNMedia)</b>  
  
- <b>[ImageManager](https://github.com/mizutanikirin/KirinUtil/wiki/ImageManager)</b>  
  
- <b>[MovieManager](https://github.com/mizutanikirin/KirinUtil/wiki/MovieManager)</b>  
  
- <b>[SoundManager](https://github.com/mizutanikirin/KirinUtil/wiki/SoundManager)</b>  
  外部のwavファイルを簡単に読み込み、再生ができるCompomentです。  
  
- <b>[CaptureManager](https://github.com/mizutanikirin/KirinUtil/wiki/CaptureManager)</b>  
  カメラに映っている映像をキャプチャして画像保存/Texture2Dを返します。
  
- <b>[QRManager](https://github.com/mizutanikirin/KirinUtil/wiki/QRManager)</b>  
  QRコードの生成/読み取りができます。  
  
### Network
- <b>[NetManager](https://github.com/mizutanikirin/KirinUtil/wiki/NetManager)</b>  
  
- <b>[HttpConnect](https://github.com/mizutanikirin/KirinUtil/wiki/HttpConnect)</b>  
  
- <b>[UDPReceiveManager](https://github.com/mizutanikirin/KirinUtil/wiki/UDPReceiveManager)</b>  
  
- <b>[UDPSendManager](https://github.com/mizutanikirin/KirinUtil/wiki/UDPSendManager)</b>  
  
### UI
- <b>[BalloonMessageManager](https://github.com/mizutanikirin/KirinUtil/wiki/BalloonMessageManager)</b>  
  uGUIで吹き出しメッセージを作ることができます。  
  
- <b>[DialogManager](https://github.com/mizutanikirin/KirinUtil/wiki/DialogManager)</b>  
  uGUIでダイアログを作ることができます。  
  
- <b>[SlideManager](https://github.com/mizutanikirin/KirinUtil/wiki/SlideManager)</b>  
  
- <b>[XmlUI](https://github.com/mizutanikirin/KirinUtil/wiki/XmlUI)</b>  
  
- <b>[UILine](https://github.com/mizutanikirin/KirinUtil/wiki/UILine)</b>  
  uGUIのImageでラインを引きます。
  
- <b>[SetDropDownScrollPosition](https://github.com/mizutanikirin/KirinUtil/wiki/SetDropDownScrollPosition)</b>  
  デフォのDropDownだとDropDownを開くときスクロールの初期位置が一番上になっていますが、SetDropDownScrollPositionを使うと選択した位置にスクロールして表示されます。  

- <b>[InputSlider](https://github.com/mizutanikirin/KirinUtil/wiki/InputSlider)</b>  
  InputFieldを持ったSliderを簡単に導入できます。
  
- <b>[ToggleButton](https://github.com/mizutanikirin/KirinUtil/wiki/ToggleButton)</b>  
  ButtonをToggleのようにOn/Offを切り替えられるようになります。
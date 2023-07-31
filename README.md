# 概要
KirinUtilはインスタレーション、Kinectやwebカメラなどを使用した体験型ゲームアプリを作成するときにコードを簡易化できる様々な機能が入ったユーティリティAssetです。  

KirinUtilの機能は今まで作成してきたアプリのよく使う機能をまとめたもので、下に掲載しているアプリのようなアプリを作成していくことができます。  
[インタラクティブ開発 | TheDesignium](https://www.thedesignium.com/interactive)
  
<b>[開発環境]</b>  
OS: Windows10 Pro  
Unity: 2019.2.19f1

# 使い方
### 1. インストール(基本)
1. [KirinUtil Latest](https://github.com/mizutanikirin/KirinUtil/releases/tag/Latest) から最新のunitypackageをダウンロード＆インポートします。
2. `Player Setting > Player > Other Settings > Api Compatibility Level*`を`.Net 4.x`に設定してください。
3. [iTween](https://assetstore.unity.com/packages/tools/animation/itween-84)をプロジェクトにiTweenをインポートください。

### 2. インストール(オプション)
1. 動画再生するMovieManagerを使う場合
  `Menu > KirinUtil > Add Class > MovieManager`を選択して追加してください。  
  MovieManagerは[AVProVideo](https://assetstore.unity.com/packages/tools/video/avpro-video-56355)が必要です。プロジェクトに「AVProVideo」をインポートしてください。  
  
2. QRコードを作るQRManagerを使う場合
  `Menu > KirinUtil > Add Class > QRManager`を選択して追加してください。  
  QRManagerは「Zxing」を使用しています。QRManagerを使う場合[ここからDL](https://github.com/micjahn/ZXing.Net/releases)して`zxing.unity.dll、zxing.unity.pdb、zxing.unity.xml`を`Assets/Plugins`に追加してください。
  
3. 印刷をするPrintManagerを使う場合
  `Menu > KirinUtil > Add Class > PrintManager`を選択して追加してください。  
  PrintManagerでは`System.Drawing.dll`を使用しています。`Assets/KirinUtil/Plugins`に`System.Drawing.dll`を入れてください。

※ 再度、削除したいときは`Menu > KirinUtil > Rmove Class`から追加したい項目を選択し追加してください。 

### 3. Unityでの使い方
UnityEditorで適当なGameObjectを作成し`Inspector > Add Compoment > Util`してください。Util、KRNMedia、KRNFileが追加されます。  
  
KirinUtilで使用できる機能は下の「機能一覧」を御覧ください。一部動作デモは`Assets/KirinUtil/Demo/`に入っています。  
使用頻度の高いCompomentは以下のようにUtilのボタンから追加することができます。  
![Util](https://user-images.githubusercontent.com/4795806/78106744-fff84600-742e-11ea-906d-96da81bdd02a.png)  

以下アプリをKirinUtilサンプルアプリとして公開しています。
- [tone color](https://github.com/mizutanikirin/ToneColor)
- [タッチゲーム](https://github.com/mizutanikirin/SampleKirinUtil)

### 4. インタラクション・体験型アプリ開発に必要な知識
インタラクション・体験型アプリ開発に必要なことをこちらにまとめました。アプリ初心者の方はこちらも参考になるかと思います。  
[インタラクション・体験型アプリ開発に必要な知識まとめ](https://note.com/thedesignium/n/n5660ba38dcb6)

### 5. AppDataについて
AppDataフォルダにはアプリで読み込む外部ファイルが入っています。  
アプリのフォルダ構造は以下を想定してScriptのデフォルトのフォルダ位置などは設定しています。  
```
- [Release]
  ├ App ← exeなど実行ファイルを入れる場所
  │   
  └ AppData ← 外部ファイルを入れる場所
    ├ Settings ← xmlなど設定ファイル
    └ Data
      ├ Images ← 画像ファイル
      ├ Movies ← 動画ファイル
      └ Sounds ← 音ファイル
```

通常Unityでは外部ファイルはStreamingAssetsフォルダに入れることが多いと思いますが、以下の理由でAppDataフォルダに入れることにしています。
- exeの下に設定ファイルを置くとアプリ更新時に設定ファイルや画像など上書きされるリスクを回避するため。
- StreamingAssetsに置くとビルド時に毎回自分でファイルを置かないといけなくなるため。
- お客様が画像など少しでもわかりやすく変更できるため。
		
# 機能一覧
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
  GameObjectの作成、フェードなどGameObjectに関する関数を主にまとめたComponentです。  
  
- <b>[ImageManager](https://github.com/mizutanikirin/KirinUtil/wiki/ImageManager)</b>  
  外部Textureの読み込み＋Image,RawImageへの割当の自動化、シーケンス画像の再生など画像に関する関数をまとめたManagerです。
  
- <b>[MovieManager](https://github.com/mizutanikirin/KirinUtil/wiki/MovieManager)</b>  
  外部動画の読み込み、動画の再生/停止などの関数をまとめたManagerです。
  
- <b>[SoundManager](https://github.com/mizutanikirin/KirinUtil/wiki/SoundManager)</b>  
  外部のwavファイルを簡単に読み込み、再生などコントロールができるManagerです。
  
- <b>[CaptureManager](https://github.com/mizutanikirin/KirinUtil/wiki/CaptureManager)</b>  
  カメラに映っている映像をキャプチャして画像保存/Texture2Dを返します。
  
- <b>[QRManager](https://github.com/mizutanikirin/KirinUtil/wiki/QRManager)</b>  
  QRコードの生成/読み取りができます。  
  
### Network
- <b>[NetManager](https://github.com/mizutanikirin/KirinUtil/wiki/NetManager)</b>  
  単純化したOSCの送信/受信やローカルIPアドレス取得などネットワーク系のManagerです。  
  
- <b>[HttpConnect](https://github.com/mizutanikirin/KirinUtil/wiki/HttpConnect)</b>  
  UnityWebRequestを使ったGet、Postと画像送信を簡単に実装できます。  
  
- <b>[UDPSendManager](https://github.com/mizutanikirin/KirinUtil/wiki/UDPSendManager)</b>  
  UDPの送信ができます。
  
- <b>[UDPReceiveManager](https://github.com/mizutanikirin/KirinUtil/wiki/UDPReceiveManager)</b>  
  UDPの受信ができます。
  
### UI
- <b>[BalloonMessageManager](https://github.com/mizutanikirin/KirinUtil/wiki/BalloonMessageManager)</b>  
  uGUIで吹き出しメッセージを作ることができます。  
  
- <b>[DialogManager](https://github.com/mizutanikirin/KirinUtil/wiki/DialogManager)</b>  
  uGUIでダイアログを作ることができます。  
  
- <b>[SlideManager](https://github.com/mizutanikirin/KirinUtil/wiki/SlideManager)</b>  
  uGUIで画像/動画のスライドを簡単に作ることができます。  
  
- <b>[UILine](https://github.com/mizutanikirin/KirinUtil/wiki/UILine)</b>  
  uGUIのImageでラインを引きます。
  
- <b>[SetDropDownScrollPosition](https://github.com/mizutanikirin/KirinUtil/wiki/SetDropDownScrollPosition)</b>  
  デフォのDropDownだとDropDownを開くときスクロールの初期位置が一番上になっていますが、SetDropDownScrollPositionを使うと選択した位置にスクロールして表示されます。  

- <b>[InputSlider](https://github.com/mizutanikirin/KirinUtil/wiki/InputSlider)</b>  
  InputFieldを持ったSliderを簡単に導入できます。
  
- <b>[ToggleButton](https://github.com/mizutanikirin/KirinUtil/wiki/ToggleButton)</b>  
  ButtonをToggleのようにOn/Offを切り替えられるようになります。

# Lisence
MIT License

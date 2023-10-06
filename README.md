# 概要
KirinUtilはインスタレーション、Kinectやwebカメラなどを使用した体験型ゲームアプリや、ARアプリを作成するときにコードを簡易化できる様々な機能が入ったユーティリティAssetです。  
  
<b>[開発環境]</b>  
- OS: Windows10 Pro  
- Unity: 2021.3.17f1

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
UnityEditorで適当なGameObjectを作成し`Inspector > Add Compoment > Util`をしてアタッチしてください。または`Hierarchy > KirinUtil > Add KirinUtil`からも追加ができます。追加するとUtilと同時にKRNMedia、KRNFileも追加されます。  
  
KirinUtilで使用できる機能は下の「機能一覧」を御覧ください。一部機能はデモシーンを用意しています。`Assets/KirinUtil/Demo/`を御覧ください。  
使用頻度の高いCompomentは以下のようにUtilのボタンから追加することができます。  
![Util](https://github.com/mizutanikirin/KirinUtil/assets/4795806/85e4c045-bd62-4665-811a-6dc8a3844950)  

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

通常Unityでは外部ファイルはStreamingAssetsフォルダなどに入れることが多いと思いますが、以下の理由でAppDataフォルダに入れることにしています。(iOS, Androidの場合はImageManager, SoundManagerなどはStreamingAssetsから読み込み変更できるようになっています。)
- exeの下に設定ファイルを置くとアプリ更新時に設定ファイルや画像など上書きされるリスクを回避するため。
- StreamingAssetsに置くとビルド時に毎回自分でファイルを置かないといけなくなるため。
- お客様が画像や音などをわかりやすく変更できるため。
		
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
  
- <b>[AssetBundleManager](https://github.com/mizutanikirin/KirinUtil/wiki/AssetBundleManager)</b>  
  簡易的なAssetBundleの読み込みをします。  
  
- <b>[FlickManager](https://github.com/mizutanikirin/KirinUtil/wiki/FlickManager)</b>  
  横方向おFlickの判別が可能になります。 

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
  
- <b>[PSAdjustments](https://github.com/mizutanikirin/KirinUtil/wiki/PSAdjustments)</b>  
  Photoshopの色調補正を再現した画像加工ができます。  
  
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
  
- <b>[UIDragManager](https://github.com/mizutanikirin/KirinUtil/wiki/UIDragManager)</b>  
  UIのImageをドラッグアンドドロップできるようにした機能です。指定GameObjectに吸着させたりもできます。
  
- <b>[VerticalText](https://github.com/mizutanikirin/KirinUtil/wiki/VerticalText)</b>  
  TextMeshProを縦書きに表示できる機能です。

### Mobile
- <b>[SafariView](https://github.com/mizutanikirin/KirinUtil/wiki/SafariView)</b>  
  iOS端末でSafariのWebViewを表示することができます。

# Menu > KirinUtil
![menu](https://github.com/mizutanikirin/KirinUtil/assets/4795806/b9d9a49a-5d00-4ea5-b3b7-8485f233e933)  
メニューでは以下のことができます。
- <b>Add Class</b>  
  特別なClassの追加ができます。
- <b>Remove Class</b>  
  特別なClassの削除ができます。
- <b>AssetBundle</b>
  - <b>Build</b>  
    AssetBundleのBuildができます。
  - <b>Search Prefab</b>  
    バンドル名が付けられているPrefabをConsoleに出力します。バンドル名とPrefabのパスが出力されます。
- <b>Vertex</b>
  - <b>Vertex display</b>  
    SceneViewで、選択しているGameObjectに頂点(赤)を追加して表示することができます。(頂点数が多いGameObjectの場合動作が重くなる可能性があります。)
  - <b>Vertex hidden</b>  
    表示している頂点を非表示にすることができます。
- <b>About KirinUtil</b>  
  KirinUtil

# Hierarchy > KirinUtil
![Hierarchy](https://github.com/mizutanikirin/KirinUtil/assets/4795806/db74339a-37fb-4bed-b9d6-6ced42af3465)  
Hierarchyのメニューでは以下のことができます。
- <b>Add KirinUtil</b>  
  Util.csをアタッチしたGameObject(KirinUtil)が作成されます。
- <b>Group Object</b>  
  空のGameObjectが作成されます。そのGameObjectは現在Hierarchyで選択中のGameObjectと同じ階層にPosition:(0,0,0)、Roatetion(0,0,0)、Scale(1,1,1)で作成されます。(Hierarchyで選択していない場合はRootに作成されます。)
- <b>GroupUI Object</b>  
  RectTransformの付いたUI用の空のGameObjectが作成されます。そのGameObjectは現在Hierarchyで選択中のGameObjectと同じ階層にPosition:(0,0,0)、Roatetion(0,0,0)、Scale(1,1,1)で作成されます。(Hierarchyで選択していない場合はRootに作成されます。)
- <b>Bold Line</b>  
  Hierarchyを見やすくするための区切りの太いライン(GameObject)が作成されます。  
  ![HierarchyLine](https://github.com/mizutanikirin/KirinUtil/assets/4795806/0700c51e-dc18-4aee-9de0-11afea823921)
- <b>Thin Line</b>  
  Hierarchyを見やすくするための区切りの細いライン(GameObject)が作成されます。(上図参考)

<b>[Note]</b>  
Bold Line、Thin Lineは手動でも追加できます。
- GameObject名を3文字以上の「=」にするとBold Lineになります。例：======
- GameObject名を3文字以上の「-」にするとThin Lineになります。例：-----

# Inspector拡張
![transform](https://github.com/mizutanikirin/KirinUtil/assets/4795806/770c1235-89cc-4d8f-a62c-41b721e46999)  
InspectorのTransformのPosition, Rotation, Scaleを初期化できるボタンを作っています。  
Pボタンを押すとPositionが(0,0,0)に、Rボタンを押すとRotationが(0,0,0)に、Sボタンを押すとSCaleが(1,1,1)になります。初期化はlocalな値になります。  
  
※こちら機能が必要ない場合は`/Assets/KirinUti/Editor/TransformInspector.cs`を削除してください。

# Lisence
[MIT License](https://github.com/mizutanikirin/KirinUtil/blob/master/LICENSE)

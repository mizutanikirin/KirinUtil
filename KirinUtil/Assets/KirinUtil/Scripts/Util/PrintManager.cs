using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.IO;
using System;
using System.Drawing.Printing;

namespace KirinUtil {
    public class PrintManager:MonoBehaviour {

        public float saveWaitTime = 2.0f;
        public string printFilePath = "tmp.png";
        private PrintDocument pd;

        /*// Use this for initialization
        void Start () {
            CaptureAndPrint();
        }

        // Update is called once per frame
        void Update () {

        }*/

        public void CaptureAndPrint(bool originAtMargins, bool onLandscape) {

            string rootDir = "/";
            if (Application.platform == RuntimePlatform.WindowsEditor) {
                rootDir = "/../";
            }
            string realPath = Application.dataPath + rootDir + printFilePath;
            string capturePath = printFilePath;
            printFilePath = realPath;

            if (File.Exists(realPath)) {
                File.Delete(realPath);
                print("del: " + realPath);
            }

            ScreenCapture.CaptureScreenshot(capturePath);

            StartCoroutine(WaitSave(originAtMargins, onLandscape));
        }

        private IEnumerator WaitSave(bool originAtMargins, bool onLandscape) {

            float latency = 0;
            while (latency < saveWaitTime) {

                //ファイルが存在していればループ終了
                if (File.Exists(printFilePath)) {
                    PrintImage(null, originAtMargins, onLandscape);
                    break;
                }
                latency += Time.deltaTime;
                yield return null;
            }

            //待機時間が上限に達していたら警告表示(おそらくスクショが保存出来ていない時)
            if (latency >= saveWaitTime) {
                Debug.LogWarning("print error: not found file");
            }
        }

        // 撮影画像を印刷する
        public void PrintImage(string filePath, bool originAtMargins, bool onLandscape) {

            if (filePath != null) {
                printFilePath = filePath;
            }

            //PrintDocumentオブジェクトの作成
            pd = new PrintDocument();

            pd.OriginAtMargins = originAtMargins;   //true = soft margins, false = hard margins
            pd.DefaultPageSettings.Landscape = onLandscape;

            // PrintPageイベントハンドラの追加
            pd.PrintPage += new PrintPageEventHandler(PrintPage);

            // PrintControllerプロパティをStandardPrintControllerにする
            pd.PrintController = new StandardPrintController();
            try {
                //印刷を開始する
                pd.Print();
            } catch (System.Exception ex) {
                Debug.LogError(ex.Message);
            }
        }

        //プリント画像を指定する
        private void PrintPage(object sender, PrintPageEventArgs e) {
            Image printImg = null;

            if (File.Exists(printFilePath)) {

                printImg = Image.FromFile(printFilePath);
                if (printImg != null) {

                    DrawAspectFillImage(e, printImg);
                    printImg.Dispose();

                    //File.Delete(printFilePath);

                }
            }

            //次のページがないことを通知する
            e.HasMorePages = false;

        }


        public void DrawAspectFillImage(PrintPageEventArgs e, Image image) {
            System.Drawing.Graphics graphics = e.Graphics;

            if (image == null)
                return;

            RectangleF marginBounds = e.MarginBounds;
            RectangleF printableArea = e.PageSettings.PrintableArea;
            int availableWidth = (int)Math.Floor(pd.OriginAtMargins ? marginBounds.Width : ( e.PageSettings.Landscape ? printableArea.Height : printableArea.Width ));
            int availableHeight = (int)Math.Floor(pd.OriginAtMargins ? marginBounds.Height : ( e.PageSettings.Landscape ? printableArea.Width : printableArea.Height ));

            float ratio = (float)image.Height / (float)image.Width;
            int printHeight = Mathf.FloorToInt(availableWidth * ratio);
            int marginHeight = ( availableHeight - printHeight ) / 2;

            //graphics.DrawRectangle(Pens.Red, 0, 0, availableWidth - 1, availableHeight - 1);
            graphics.DrawImage(image, 0, marginHeight, availableWidth, printHeight);
        }
    }
}
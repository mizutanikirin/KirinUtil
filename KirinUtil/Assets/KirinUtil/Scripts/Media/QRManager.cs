using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

namespace KirinUtil {
    public class QRManager:MonoBehaviour {

        //----------------------------------
        //  ReadQR
        //----------------------------------
        public string ReadQR(Texture2D texture) {
            string response = "";

            BarcodeReader reader = new BarcodeReader();
            reader.Options.TryHarder = true;
            reader.Options.PossibleFormats = new List<BarcodeFormat>();
            reader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            reader.AutoRotate = true;
            reader.TryInverted = true;
            reader.Options.TryHarder = true;

            Color32[] color = texture.GetPixels32();
            int width = texture.width;
            int height = texture.height;

            ZXing.Result result = reader.Decode(color, width, height);
            if (result != null) {
                response = result.Text;
            } else {
                response = null;
            }

            return response;

        }

        //----------------------------------
        //  CreateQR
        //----------------------------------
        public void CreateQR(Image qrCodeImage, string contents, int textureSize) {
            Debug.Log("createQRCode");

            Texture2D texture = new Texture2D(textureSize, textureSize);
            qrCodeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
            qrCodeImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero);

            var qrCodeColors = Write(contents, texture.width, texture.height);
            texture.SetPixels32(qrCodeColors);
            texture.Apply();

        }

        public void SaveQR(string contents, int textureSize, string filePath, ImageFormat format = ImageFormat.PNG) {
            Texture2D texture = new Texture2D(textureSize, textureSize);
            var qrCodeColors = Write(contents, texture.width, texture.height);
            texture.SetPixels32(qrCodeColors);
            texture.Apply();

            // save image
            if (format == ImageFormat.PNG) {
                filePath += ".png";
                File.WriteAllBytes(filePath, texture.EncodeToPNG());
            } else {
                filePath += ".jpg";
                File.WriteAllBytes(filePath, texture.EncodeToJPG());
            }
        }

        private Color32[] Write(string content, int width, int height) {
            var writer = new BarcodeWriter {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions {
                    Height = height,
                    Width = width
                }
            };

            return writer.Write(content);
        }
    }
}
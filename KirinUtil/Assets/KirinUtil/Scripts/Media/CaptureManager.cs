using UnityEngine;
using System.Collections;
using System.IO;

namespace KirinUtil {

    public class CaptureManager:MonoBehaviour {

        // 保存
        public void Take(Camera targetCamera, string filePath, bool alpha, ImageFormat format = ImageFormat.PNG) {

            // camera size
            Vector2 min = targetCamera.ViewportToScreenPoint(Vector2.zero);
            Vector2 max = targetCamera.ViewportToScreenPoint(Vector2.one);
            int width = Mathf.CeilToInt(max.x - min.x)-1;
            int height = Mathf.CeilToInt(max.y - min.y);
            int y = Screen.height - Mathf.FloorToInt(min.y + height);

            TextureFormat alphaFormat;
            if (alpha)
                alphaFormat = TextureFormat.RGBA32;
            else
                alphaFormat = TextureFormat.RGB24;

            // RenderTexture set
            RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            RenderTexture oldTargetTexture = targetCamera.targetTexture;
            RenderTexture oldActiveTexture = RenderTexture.active;
            targetCamera.targetTexture = renderTexture;
            targetCamera.Render();
            RenderTexture.active = renderTexture;

            // set texture
            Texture2D texture2 = new Texture2D(width, height, alphaFormat, false);
            texture2.ReadPixels(new Rect(min.x, y, width, height), 0, 0);
            texture2.Apply();

            // RenderTexture remove
            RenderTexture.active = oldActiveTexture;
            targetCamera.targetTexture = oldTargetTexture;
            RenderTexture.ReleaseTemporary(renderTexture);

            // save image
            if (format == ImageFormat.PNG) {
                filePath += ".png";
                File.WriteAllBytes(filePath, texture2.EncodeToPNG());
            } else {
                filePath += ".jpg";
                File.WriteAllBytes(filePath, texture2.EncodeToJPG());
            }
        }

        // 保存
        public void Take(Camera targetCamera, Rect rect, string filePath, bool alpha, ImageFormat format = ImageFormat.PNG) {
            // RenderTexture set
            RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            RenderTexture oldTargetTexture = targetCamera.targetTexture;
            RenderTexture oldActiveTexture = RenderTexture.active;
            targetCamera.targetTexture = renderTexture;
            targetCamera.Render();
            RenderTexture.active = renderTexture;

            TextureFormat alphaFormat;
            if (alpha)
                alphaFormat = TextureFormat.RGBA32;
            else
                alphaFormat = TextureFormat.RGB24;

            // set texture
            Texture2D texture2 = new Texture2D(Mathf.CeilToInt(rect.width), Mathf.CeilToInt(rect.height), alphaFormat, false);
            texture2.ReadPixels(rect, 0, 0);
            texture2.Apply();

            // RenderTexture remove
            RenderTexture.active = oldActiveTexture;
            targetCamera.targetTexture = oldTargetTexture;
            RenderTexture.ReleaseTemporary(renderTexture);

            // save image
            if (format == ImageFormat.PNG) {
                filePath += ".png";
                File.WriteAllBytes(filePath, texture2.EncodeToPNG());
            } else {
                filePath += ".jpg";
                File.WriteAllBytes(filePath, texture2.EncodeToJPG());
            }
        }

        // 保存せずにTexture2Dを返す
        private RenderTexture renderTexture = null;
        private RenderTexture oldTargetTexture = null;
        private RenderTexture oldActiveTexture = null;
        public Texture2D Take(Camera targetCamera, bool alpha) {

            // camera size
            Vector2 min = targetCamera.ViewportToScreenPoint(Vector2.zero);
            Vector2 max = targetCamera.ViewportToScreenPoint(Vector2.one);
            int width = Mathf.CeilToInt(max.x - min.x) - 1;
            int height = Mathf.CeilToInt(max.y - min.y);
            int y = Screen.height - Mathf.FloorToInt(min.y + height);

            TextureFormat alphaFormat;
            if (alpha)
                alphaFormat = TextureFormat.RGBA32;
            else
                alphaFormat = TextureFormat.RGB24;

            // RenderTexture set
            renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            oldTargetTexture = targetCamera.targetTexture;
            oldActiveTexture = RenderTexture.active;
            targetCamera.targetTexture = renderTexture;
            targetCamera.Render();
            RenderTexture.active = renderTexture;

            // set texture
            Texture2D texture2 = new Texture2D(width, height, alphaFormat, false);
            texture2.ReadPixels(new Rect(min.x, y, width, height), 0, 0);
            texture2.Apply();

            // RenderTexture remove
            RenderTexture.active = oldActiveTexture;
            targetCamera.targetTexture = oldTargetTexture;
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = null;

            // save image
            return texture2;
        }

        // 保存せずにTexture2Dを返す
        public Texture2D Take(Camera targetCamera, Rect rect, bool alpha) {
            // RenderTexture set
            RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            RenderTexture oldTargetTexture = targetCamera.targetTexture;
            RenderTexture oldActiveTexture = RenderTexture.active;
            targetCamera.targetTexture = renderTexture;
            targetCamera.Render();
            RenderTexture.active = renderTexture;

            TextureFormat alphaFormat;
            if (alpha)
                alphaFormat = TextureFormat.RGBA32;
            else
                alphaFormat = TextureFormat.RGB24;

            // set texture
            Texture2D texture2 = new Texture2D(Mathf.CeilToInt(rect.width), Mathf.CeilToInt(rect.height), alphaFormat, false);
            texture2.ReadPixels(rect, 0, 0);
            texture2.Apply();

            // RenderTexture remove
            RenderTexture.active = oldActiveTexture;
            targetCamera.targetTexture = oldTargetTexture;
            RenderTexture.ReleaseTemporary(renderTexture);

            return texture2;
        }

    }

}
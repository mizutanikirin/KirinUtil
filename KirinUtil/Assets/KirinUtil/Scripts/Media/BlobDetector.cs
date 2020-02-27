using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace KirinUtil {
    public class BlobDetector : MonoBehaviour {

        public GameObject blobPrefab;
        public GameObject blobsRootObj;

        [Range(0, 500)]
        [Tooltip("Max X and Y distance to blob, in pixels, to consider a pixel part of it.")]
        public int xyDistanceToBlob = 10;

        [Range(0, 500)]
        [Tooltip("Minimum amount of pixels in a blob.")]
        public int minPixelsInBlob = 50;

        [Range(1, 10)]
        [Tooltip("Increment in X & Y directions, when analyzing the raw depth image.")]
        public int xyIncrement = 3;

        [Tooltip("UI-Text to display info messages.")]
        public UnityEngine.UI.Text infoText;

        // depth image resolution
        public Vector2 imageSize;

        // depth scale
        public Vector3 depthScale = Vector3.one;

        // screen rectangle taken by the foreground image (in pixels)
        public Rect foregroundImgRect;

        // list of blobs
        private List<Blob> blobs = new List<Blob>();

        private Color32[] blobColor;
        public Vector2 displayScale;



        //----------------------------------
        //  Get
        //----------------------------------
        #region Get
        /// <summary>
        /// Gets the number of detected blobs.
        /// </summary>
        /// <returns>Number of blobs.</returns>
        public int GetBlobsCount() {
            return blobs.Count;
        }


        /// <summary>
        /// Gets the blob with the given index.
        /// </summary>
        /// <param name="i">Blob index.</param>
        /// <returns>The blob.</returns>
        public Blob GetBlob(int i) {
            if (i >= 0 && i < blobs.Count) {
                return blobs[i];
            }

            return null;
        }


        /// <summary>
        /// Gets position on the depth image of the given blob. 
        /// </summary>
        /// <param name="i">Blob index.</param>
        /// <returns>Depth image position of the blob.</returns>
        public Vector2 GetBlobImagePos(int i) {
            if (i >= 0 && i < blobs.Count) {
                Vector3 blobCenter = blobs[i].GetBlobCenter();
                return (Vector2)blobCenter;

            }

            return Vector2.zero;
        }


        /// <summary>
        /// Gets position in the 3d space of the given blob.
        /// </summary>
        /// <param name="i">Blob index.</param>
        /// <returns>Space position of the blob.</returns>
        public Vector3 GetBlobSpacePos(int i) {
            if (i >= 0 && i < blobs.Count) {
                Vector3 blobCenter = blobs[i].GetBlobCenter();
                Vector3 spacePos = blobCenter;//new Vector3();//kinectManager.MapDepthPointToSpaceCoords(sensorIndex, (Vector2)blobCenter, (ushort)blobCenter.z, true);

                return spacePos;

            }

            return Vector3.zero;
        }
        #endregion

        //----------------------------------
        //  main
        //----------------------------------
        // これを呼び出してblobの情報を取得
        public Color32[] GetBlobColor(Color32[] cameraColors) {
            DetectBlobsInRawDepth(cameraColors);
            InstantiateBlobObjects(cameraColors);

            return blobColor;
        }

        // blob解析本体
        // detects blobs of pixel in the raw depth image
        private void DetectBlobsInRawDepth(Color32[] cameraColors) {

            Color[] change_pixels = new Color[cameraColors.Length];
            int[] finalImageArray = new int[cameraColors.Length];
            for (int i = 0; i < cameraColors.Length; i++) {
                change_pixels[i] = cameraColors[i];
                if (cameraColors[i].r != 0) finalImageArray[i] = 1;
                else finalImageArray[i] = 0;
            }

            blobs.Clear();

            int di = 0;
            int count = 0;
            for (int y = 0; y < imageSize.y; y += xyIncrement) {
                for (int x = 0; x < imageSize.x; x += xyIncrement) {

                    di = x + y * (int)imageSize.x;

                    if (finalImageArray[di] != 0) {
                        bool blobFound = false;
                        foreach (var b in blobs) {
                            if (b.IsNearOrInside(x, y, xyDistanceToBlob)) {
                                b.AddDepthPixel(x, y);
                                blobFound = true;
                                break;
                            }
                        }

                        if (!blobFound) {
                            Blob b = new Blob(x, y);
                            blobs.Add(b);
                        }

                        count++;
                    }
                }
            }

            var insideblobs = new List<Blob>();

            // remove inside blobs
            foreach (var b in blobs)
                foreach (var b2 in blobs)
                    if (b.IsInside(b2) && !insideblobs.Contains(b) && b != b2)
                        insideblobs.Add(b);

            for (int i = 0; i < insideblobs.Count; i++)
                if (blobs.Contains(insideblobs[i]))
                    blobs.Remove(insideblobs[i]);

            // remove small blobs
            var smallBlobs = blobs.Where(x => x.pixels < minPixelsInBlob).ToList();
            for (int i = 0; i < smallBlobs.Count; i++)
                if (blobs.Contains(smallBlobs[i]))
                    blobs.Remove(smallBlobs[i]);

            if (infoText) {
                string sMessage = blobs.Count + " blobs detected.\n";

                for (int i = 0; i < blobs.Count; i++) {
                    Blob b = blobs[i];
                    if (infoText != null) sMessage += string.Format("x1: {0}, y1: {1}, x2: {2}, y2: {3}, pix: {4}\n", b.minx, b.miny, b.maxx, b.maxy, b.pixels);
                    //sMessage += "x: " + b.minx + "  y: " + b.miny + "  pixel: " + b.pixels + System.Environment.NewLine;
                    //sMessage += string.Format("Blob {0} at {1}\n", i, GetBlobSpacePos(i));
                }

                //Debug.Log(sMessage);
                if (infoText != null) infoText.text = sMessage;
            }


        }

        // blobの表示
        // instantiates representative blob objects for each blob
        private void InstantiateBlobObjects(Color32[] cameraColors) {

            if (cameraColors == null) return;

            int rectX = (int)foregroundImgRect.xMin;
            int rectY = (int)foregroundImgRect.yMin;

            // display blob rectangles
            int bi = 0;

            Util.media.DeleteAllGameObject(blobsRootObj, false);

            blobColor = new Color32[cameraColors.Length];
            for (int i = 0; i < cameraColors.Length; i++) {
                blobColor[i] = new Color32(0, 0, 0, 0);
            }

            foreach (var b in blobs) {
                float x = (depthScale.x >= 0f ? b.minx : imageSize.x - b.maxx) * displayScale.x;  // b.minx * scaleX;
                float y = (depthScale.y >= 0f ? b.miny : imageSize.y - b.maxy) * displayScale.y;  // b.maxy * scaleY;

                Rect rectBlob = new Rect(rectX + x, rectY + y, (b.maxx - b.minx) * displayScale.x, (b.maxy - b.miny) * displayScale.y);

                Vector3 blobCenter = b.GetBlobCenter();
                x = (depthScale.x >= 0f ? blobCenter.x : imageSize.x - blobCenter.x) * displayScale.x;  // blobCenter.x * scaleX;
                y = (depthScale.y >= 0f ? blobCenter.y : imageSize.y - blobCenter.y) * displayScale.y;  // blobCenter.y* scaleY; // 

                Vector3 blobPos = new Vector3(rectX + x, rectY + y, 0);
                Util.media.CreateUIObj(blobPrefab, blobsRootObj, "blob" + bi, new Vector3(1920 - blobPos.x - 960, blobPos.y - 540, 0), Vector3.zero, new Vector3(rectBlob.width, rectBlob.height, 1));

                /*int startX = Mathf.RoundToInt(blobPos.x - rectBlob.width / 2);
                int endX = Mathf.RoundToInt(blobPos.x + rectBlob.width / 2);
                int startY = Mathf.RoundToInt(blobPos.y - rectBlob.height / 2);
                int endY = Mathf.RoundToInt(blobPos.y + rectBlob.height / 2);*/

                for (int x1 = b.minx; x1 < b.maxx; x1++) {
                    for (int y1 = b.miny; y1 < b.maxy; y1++) {
                        int i = x1 + y1 * 1280;
                        blobColor[i] = new Color32(255, 255, 255, 255);
                    }
                }

                bi++;
            }
        }
    }
}

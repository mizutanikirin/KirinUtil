// http://setchi-q.hatenablog.com/entry/2015/02/04/131834

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;

namespace KirinUtil
{
    public class HttpConnect : MonoBehaviour
    {
        private static HttpConnect instance;

        static HttpConnect Instance
        {

            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("HttpManager");
                    instance = obj.AddComponent<HttpConnect>();
                }
                return instance;
            }
        }

        //----------------------------------
        //  UnityWebRequest
        //----------------------------------
        #region Get(UnityWebRequest)
        public static void Get(string url, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);

            Instance.StartCoroutine(Instance.WaitForRequestGet(request, onSuccess, onError));

        }

        public static void Get(string url, string token, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", "Bearer " + token);

            Instance.StartCoroutine(Instance.WaitForRequestGet(request, onSuccess, onError));

        }


        IEnumerator WaitForRequestGet(UnityWebRequest request, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError)
        {

            // リクエスト送信
            yield return request.SendWebRequest();

            // 通信エラーチェック
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    if (request.responseCode == 200)
                    {
                        onSuccess(request);
                    }
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    if (onError != null) onError(request);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    if (onError != null) onError(request);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    if (onError != null) onError(request);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion


        #region Post(UnityWebRequest)
        public static void Post(string url, List<List<string>> fieldList, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError = null)
        {

            WWWForm form = new WWWForm();
            for (int i = 0; i < fieldList.Count; i++)
            {
                //print("field: " + fieldList[i][0] + ", " + fieldList[i][1]);
                form.AddField(fieldList[i][0], fieldList[i][1]);
            }

            Instance.StartCoroutine(Instance.WaitForRequestPost(url, "", form, onSuccess, onError));
        }

        public static void Post(string url, string token, List<List<string>> fieldList, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError = null)
        {

            WWWForm form = new WWWForm();
            for (int i = 0; i < fieldList.Count; i++)
            {
                //print("field: " + fieldList[i][0] + ", " + fieldList[i][1]);
                form.AddField(fieldList[i][0], fieldList[i][1]);
            }

            Instance.StartCoroutine(Instance.WaitForRequestPost(url, token, form, onSuccess, onError));
        }

        IEnumerator WaitForRequestPost(string url, string token, WWWForm form, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError)
        {


            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {

                if (token != "") request.SetRequestHeader("Authorization", "Bearer " + token);

                yield return request.SendWebRequest();

                // 通信エラーチェック
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        if (request.responseCode == 200)
                        {
                            onSuccess(request);
                        }
                        break;
                    case UnityWebRequest.Result.ConnectionError:
                        onError(request);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        onError(request);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        onError(request);
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

        }
        #endregion


        //----------------------------------
        //  画像送信
        //----------------------------------
        #region SendImage
        public static void SendImage(string url, Texture2D texture, string varName, string fileName, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError = null)
        {

            Instance.StartCoroutine(Instance.WaitSendImage(url, texture, varName, fileName, onSuccess, onError));

        }

        private IEnumerator WaitSendImage(string url, Texture2D texture, string varName, string fileName, Action<UnityWebRequest> onSuccess, Action<UnityWebRequest> onError)
        {

            byte[] imageData = texture.EncodeToPNG();

            string imageType = Path.GetExtension(fileName);
            string type = "";
            if (imageType == ".jpg" || imageType == ".jpeg") type = "image/jpeg";
            if (imageType == ".gif") type = "image/gif";
            else type = "image/png";

            WWWForm form = new WWWForm();
            form.AddBinaryData(varName, imageData, fileName, type);
            UnityWebRequest request = UnityWebRequest.Post(url, form);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                onError(request);
            }
            else
            {

                if (request.responseCode == 200)
                {
                    onSuccess(request);
                }
            }
        }
        #endregion

    }
}

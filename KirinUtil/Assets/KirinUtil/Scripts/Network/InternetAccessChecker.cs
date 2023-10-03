using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace KirinUtil
{
    public class InternetAccessChecker : MonoBehaviour
    {

        /// <summary>
        /// インターネットに接続しているか確認
        /// </summary>
        public void CheckNetwork(string checkURL, Action<bool> callback, int timeOut = 2)
        {
            StartCoroutine(Check(checkURL, callback, timeOut));
        }

        private IEnumerator Check(string checkURL, Action<bool> callback, int timeOut)
        {
            // 回線に繋がっていない(繋がっていても通信出来るとは限らないのでそっちの判定はしない)
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                callback(false);
                yield break;
            }

            using (UnityWebRequest request = UnityWebRequest.Get(checkURL))
            {
                request.timeout = timeOut;
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            }
        }

    }
}
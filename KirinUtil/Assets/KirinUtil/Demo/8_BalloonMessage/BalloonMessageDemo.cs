using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class BalloonMessageDemo : MonoBehaviour
    {
        // balloon
        public BalloonMessageManager balloonMessage;
        public GameObject balloonParentObj;

        // Start is called before the first frame update
        void Start()
        {
            // 吹き出し
            balloonMessage.Create(balloonParentObj, 0);
            balloonMessage.PlayAll(0);
            StartCoroutine(BalloonWait());
        }

        private IEnumerator BalloonWait()
        {
            yield return new WaitForSeconds(5f);
            
            balloonMessage.Create(balloonParentObj, 1);
            balloonMessage.PlayAll(1);

            yield return new WaitForSeconds(5f);

            balloonMessage.StopAll(0);
        }
    }
}

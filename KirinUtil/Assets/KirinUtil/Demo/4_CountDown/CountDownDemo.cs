using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil.Demo
{
    public class CountDownDemo : MonoBehaviour
    {
        [SerializeField] private CountDown countDown;
        [SerializeField] private Text countDownText;
        private bool isFinish = false;

        // Start is called before the first frame update
        void Start()
        {
            isFinish = false;
            countDown.SetCountDown(10, countDownText, false, 0, 3);
            //countDown.SetCountDown(10, util.media.sound, 0);
        }

        // Update is called once per frame
        void Update()
        {
            // countDown
            if (!isFinish && countDown.Update2())
            {
                print("Finish!");
                isFinish = true;
            }
        }
    }
}

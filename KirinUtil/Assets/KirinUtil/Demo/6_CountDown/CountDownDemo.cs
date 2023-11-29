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

            // Demo1
            // カウントダウン + text
            // カウント数字前後に追加のstring表示あり
            //countDown.Set(3);

            // Demo2
            // カウントダウン + text
            // カウント数字前後に追加のstring表示あり
            countDown.Set(10, countDownText, "あと", "秒");

            // Demo3
            // カウントダウン + text + 音
            // カウント10秒で3秒前からSE音を鳴らす(Text)
            //countDown.Set(10, countDownText, false, 0, 3);

            // Demo4
            // カウントダウン + 音
            // カウント10秒でカウントのたびに音を鳴らす(Text)
            //countDown.Set(10, true, 0);
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

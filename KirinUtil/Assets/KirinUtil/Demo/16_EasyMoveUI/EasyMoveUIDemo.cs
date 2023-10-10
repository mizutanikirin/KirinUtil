using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class EasyMoveUIDemo : MonoBehaviour
    {
        [SerializeField] private GameObject horizonObj;
        [SerializeField] private GameObject verticalObj;
        [SerializeField] private GameObject randomObj;
        [SerializeField] private GameObject rotateObj;
        [SerializeField] private GameObject scaleObj;

        // Start is called before the first frame update
        void Start()
        {
            // 簡単な動き
            EasyMoveStart();
        }


        private void EasyMoveStart()
        {
            // 横移動
            Util.media.MoveHorizon(horizonObj, 20, 0.5f, 0, 10, true, true);

            // 縦移動
            Util.media.MoveVertical(verticalObj, 20, 0.5f, 0, 10, true, true);

            // ランダム移動
            Util.media.MoveRandom(randomObj, 20, 0.5f, 0, 10, true, true);

            // 左右の回転
            Util.media.MoveRotate(rotateObj, 10, 0.5f, 0, 10, true);

            // slcaleのアニメ
            Util.media.MoveScale(scaleObj, 1.1f, 0.5f, 0, 10, true);

            //StartCoroutine(StopWait());
        }

        private IEnumerator StopWait()
        {
            yield return new WaitForSeconds(2.5f);

            // 途中で止めるとき
            Util.media.StopMoveHorizon(horizonObj);
            Util.media.StopMoveVertical(verticalObj);
            Util.media.StopMoveHorizon(randomObj);
            Util.media.StopMoveRotate(rotateObj);
            Util.media.StopMoveScale(scaleObj);
        }

    }
}
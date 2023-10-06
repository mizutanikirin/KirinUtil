using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class UtilDemo : MonoBehaviour
    {
        // EquidistantX
        [SerializeField] private List<GameObject> equidistantObjList;

        // SetObjectInFrontOfCamera
        [SerializeField] private GameObject frontObj;

        // GetLinePos
        [SerializeField] private GameObject nowPosObj;
        [SerializeField] private GameObject targetPosObj;
        [SerializeField] private GameObject resultPosObj;

        // Start is called before the first frame update
        void Start()
        {
            // ランキング
            RankingDemo();

            // デバッグ用の時間計測
            StartCoroutine(DebugWatch());

            // 等間隔に整列
            Util.EquidistantX(equidistantObjList, -200, 300, 0);

            // カメラの前にObjectを配置する。
            //frontObj.transform.position = Util.GetPosInFrontOfCamera(Camera.main, 3f, true);
            Util.SetObjectInFrontOfCamera(Camera.main, frontObj, 3f, false, 0, true, false);

            // 指定した区切り文字で区切りListで値を返す
            GetSplitStringList();

            // 文字列のカウント(日本語は2, 英数字は1としてカウント)
            Debug.Log("TextLengthJPN: " + Util.TextLengthJPN("abcdef") + ", " + Util.TextLengthJPN("abcdあef"));

            // 現在の位置から指定した距離だけターゲットの方向に移動
            resultPosObj.transform.localPosition = Util.GetLinePos(
                nowPosObj.transform.localPosition, 
                targetPosObj.transform.localPosition, 
                100
            );
        }

        // Update is called once per frame
        void Update()
        {

        }

        //----------------------------------
        //  ランキング デモ
        //----------------------------------
        #region RankingDemo
        private void RankingDemo()
        {
            // ランキング
            List<string> id = new List<string>
            {
                "aaa",
                "bbb",
                "ccc",
                "ddd",
                "eee"
            };
            List<float> value = new List<float>
            {
                5,
                3,
                2,
                4,
                1
            };
            List<OrderData> data = Util.GetOrderList(id, value, Direction.Down);
            string rank = "[Ranking]" + Environment.NewLine;
            for (int i = 0; i < data.Count; i++)
            {
                rank += data[i].id + ": " + data[i].value + Environment.NewLine;
            }
            Debug.Log(rank);
        }
        #endregion


        //----------------------------------
        //  デバッグ用の時間計測
        //----------------------------------
        #region DebugWatch
        private IEnumerator DebugWatch()
        {
            Util.DebugWatchStart();
            yield return new WaitForSeconds(1.0f);
            Util.DebugWatchStop();
        }
        #endregion


        //----------------------------------
        //  指定した区切り文字で区切りListで
        //  値を返す
        //----------------------------------
        #region 指定した区切り文字で区切りListで値を返す
        private void GetSplitStringList()
        {
            string rawData = "a,b,c,d,e";
            List<string> strs = Util.GetSplitStringList(rawData, ",");
            string printStr = "GetSplitStringList: ";
            for (int i = 0; i < strs.Count; i++)
            {
                if(i != strs.Count -1) printStr += strs[i] + ", ";
                else printStr += strs[i];
            }
            Debug.Log(printStr);
        }
        #endregion
    }
}

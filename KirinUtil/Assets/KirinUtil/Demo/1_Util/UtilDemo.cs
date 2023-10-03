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
            // �����L���O
            RankingDemo();

            // �f�o�b�O�p�̎��Ԍv��
            StartCoroutine(DebugWatch());

            // ���Ԋu�ɐ���
            Util.EquidistantX(equidistantObjList, -200, 300, 0);

            // �J�����̑O��Object��z�u����B
            //frontObj.transform.position = Util.GetPosInFrontOfCamera(Camera.main, 3f, true);
            Util.SetObjectInFrontOfCamera(Camera.main, frontObj, 3f, false, 0, true, false);

            // �w�肵����؂蕶���ŋ�؂�List�Œl��Ԃ�
            GetSplitStringList();

            // ������̃J�E���g(���{���2, �p������1�Ƃ��ăJ�E���g)
            Debug.Log("TextLengthJPN: " + Util.TextLengthJPN("abcdef") + ", " + Util.TextLengthJPN("abcd��ef"));

            // ���݂̈ʒu����w�肵�����������^�[�Q�b�g�̕����Ɉړ�
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
        //  �����L���O �f��
        //----------------------------------
        #region RankingDemo
        private void RankingDemo()
        {
            // �����L���O
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
        //  �f�o�b�O�p�̎��Ԍv��
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
        //  �w�肵����؂蕶���ŋ�؂�List��
        //  �l��Ԃ�
        //----------------------------------
        #region �w�肵����؂蕶���ŋ�؂�List�Œl��Ԃ�
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

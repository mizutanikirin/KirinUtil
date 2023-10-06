using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil.Demo
{
    public class MediaDemo : MonoBehaviour
    {
        [SerializeField] private GameObject parentObj;
        [SerializeField] private GameObject fadeObj;
        [SerializeField] private RawImage colorImage;
        [SerializeField] private GameObject pathObj;
        [SerializeField] private GameObject messageObj;

        // Start is called before the first frame update
        void Start()
        {
            // ������GameObject�̂ݎ擾
            GetChildGameObject();

            // �eGameObject�̉��ɂ���GameObject��List�ŕԂ�
            GetAllGameObject();

            // �t�F�[�h
            FadeStart();

            // #ffffff����Color�ɕϊ�
            colorImage.color = Util.media.HexToColor("#ffcc00");

            // GameObject�̃p�X��Ԃ�
            Debug.Log("Path: " + Util.media.GetGameObjectPath(pathObj));

            // �ړ����Ȃ���UI��\�� / ��\��
            Util.media.UIDisplay(messageObj, FadeType.FadeIn, Direction.Up, 1f, 50f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        //----------------------------------
        //  ������GameObject�̂ݎ擾
        //----------------------------------
        #region ������GameObject�̂ݎ擾
        private void GetChildGameObject()
        {
            List<GameObject> childrenObj = Util.media.GetChildGameObject(parentObj, "child");
            string children = "";
            for (int i = 0; i < childrenObj.Count; i++)
            {
                if(i != childrenObj.Count-1) children += childrenObj[i].name + ", ";
                else children += childrenObj[i].name;
            }
            Debug.Log("GetChildGameObject: " + children);
        }
        #endregion


        //----------------------------------
        //  �eGameObject�̉��ɂ���GameObject��List�ŕԂ�
        //----------------------------------
        #region ������GameObject�̂ݎ擾
        private void GetAllGameObject()
        {
            List<GameObject> childrenObj = Util.media.GetAllGameObject(parentObj);
            string children = "";
            for (int i = 0; i < childrenObj.Count; i++)
            {
                if (i != childrenObj.Count - 1) children += childrenObj[i].name + ", ";
                else children += childrenObj[i].name;
            }
            Debug.Log("GetAllGameObject: " + children);
        }
        #endregion


        //----------------------------------
        //  �t�F�[�h
        //----------------------------------
        #region �t�F�[�h
        private void FadeStart()
        {
            // fade gui
            Util.media.FadeOutUI(fadeObj, 2, 0);
            StartCoroutine(FadeWait());
        }

        private IEnumerator FadeWait()
        {
            yield return new WaitForSeconds(3f);

            Util.media.FadeInUI(fadeObj, 2, 0);
        }
        #endregion
    }
}
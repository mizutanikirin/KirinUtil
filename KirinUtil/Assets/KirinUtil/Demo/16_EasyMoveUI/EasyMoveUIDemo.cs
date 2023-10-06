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
            // �ȒP�ȓ���
            EasyMoveStart();
        }


        private void EasyMoveStart()
        {
            // ���ړ�
            Util.media.MoveHorizon(horizonObj, 20, 0.5f, 0, 10, true, true);

            // �c�ړ�
            Util.media.MoveVertical(verticalObj, 20, 0.5f, 0, 10, true, true);

            // �����_���ړ�
            Util.media.MoveRandom(randomObj, 20, 0.5f, 0, 10, true, true);

            // ���E�̉�]
            Util.media.MoveRotate(rotateObj, 10, 0.5f, 0, 10, true);

            // slcale�̃A�j��
            Util.media.MoveScale(scaleObj, 1.1f, 0.5f, 0, 10, true);

            //StartCoroutine(StopWait());
        }

        private IEnumerator StopWait()
        {
            yield return new WaitForSeconds(2.5f);

            // �r���Ŏ~�߂�Ƃ�
            Util.media.StopMoveHorizon(horizonObj);
            Util.media.StopMoveVertical(verticalObj);
            Util.media.StopMoveHorizon(randomObj);
            Util.media.StopMoveRotate(rotateObj);
            Util.media.StopMoveScale(scaleObj);
        }

    }
}
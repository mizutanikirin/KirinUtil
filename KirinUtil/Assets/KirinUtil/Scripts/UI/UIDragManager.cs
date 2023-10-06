using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KirinUtil
{
    public class UIDragManager : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private Camera uiCamera;
        [SerializeField] private Canvas uiCanvas;
        [Tooltip("�h���b�O�\�ȃG���A")]
        [SerializeField] private Rect dragRect;
        [Tooltip("�z���Ώە�(adsorptionObjs)���w�苗��[pixel]�ȓ��ɂ���Ƌz������")]
        [SerializeField] private float nearDistanceThreshold;
        [Tooltip("Start()����Drag���L���ɂȂ�")]
        [SerializeField] private bool awakeEnable;

        [Tooltip("Specify the GameObject to drag")]
        [SerializeField] private List<UIDrag> uiDrags;
        [Tooltip("Specify the GameObject to be absorbed")]
        [SerializeField] private List<GameObject> adsorptionObjs;

        //----------------------------------
        //  Event
        //----------------------------------
        #region Event
        [Serializable]
        public class DragedEvent : UnityEvent<GameObject> { }
        [Serializable]
        public class FitEvent : UnityEvent<GameObject, GameObject> { }
        [Serializable]
        public class ClickEvent : UnityEvent<GameObject> { }

        [Header("Event")]
        public DragedEvent dragedEvent = new DragedEvent();
        public FitEvent fitEvent = new FitEvent();
        public ClickEvent clickEvent = new ClickEvent();

        void OnEnable()
        {
            dragedEvent.AddListener(DragedAction);
            fitEvent.AddListener(FitAction);
            clickEvent.AddListener(ClickAction);
        }

        void OnDisable()
        {
            dragedEvent.RemoveListener(DragedAction);
            fitEvent.RemoveListener(FitAction);
            clickEvent.RemoveListener(ClickAction);
        }

        // Dummy event
        private void DragedAction(GameObject dragObj) { }
        private void FitAction(GameObject fitObj, GameObject adsorptionObj) { }
        private void ClickAction(GameObject clickObj) { }

        // Invoke
        private void Draged(GameObject dragObj)
        {
            //print("[Draged] " + dragObj.name + ": " + dragObj.transform.position);
            dragedEvent.Invoke(dragObj);
        }
        private void Fit(GameObject fitObj, GameObject adsorptionObj)
        {
            //print("[Fit] " + fitObj.name + ": " + adsorptionObj.name);
            fitEvent.Invoke(fitObj, adsorptionObj);
        }
        private void Clicked(GameObject clickObj)
        {
            //print("[Clicked] " + clickObj.name + ": " + clickObj.transform.position);
            clickEvent.Invoke(clickObj);
        }
        #endregion


        //----------------------------------
        //  Public
        //----------------------------------
        #region Drag
        // �܂Ƃ߂�Set����
        // (�� �͂��߂�SetDrag()��������AddDragObj()�����Ȃ���Drag�ł��Ȃ�)
        public void StartDrag()
        {
            for (int i = 0; i < uiDrags.Count; i++)
            {
                AddDrag(uiDrags[i].gameObject);
            }
        }

        // �h���b�OObj��ǉ�����
        // (�� �͂��߂�SetDrag()��������AddDragObj()�����Ȃ���Drag�ł��Ȃ�)
        public void AddDrag(GameObject dragObj)
        {
            UIDrag uiDrag = dragObj.GetComponent<UIDrag>();
            if (uiDrag == null) uiDrag = dragObj.AddComponent<UIDrag>();

            uiDrag.Init(
                Vector2.zero, dragRect, nearDistanceThreshold,
                uiCanvas, uiCamera, adsorptionObjs,
                Draged, Fit, Clicked
            );

            if (GetDragListNum(dragObj) == -1) uiDrags.Add(uiDrag);
        }

        // �w�肵���h���b�OObj�̃h���b�O�@�\���폜���� or �w�肵���h���b�OObj���̂��폜����
        // �폜�����uiDrags������폜�����
        public void RemoveDrag(GameObject dragObj, bool destroySelf = false)
        {
            int listNum = GetDragListNum(dragObj);
            if (listNum == -1) return;

            if (destroySelf) Destroy(uiDrags[listNum].gameObject);
            else Destroy(uiDrags[listNum]);

            uiDrags.RemoveAt(listNum);
        }
        #endregion

        #region Adsorption
        public void AddAdsorption(GameObject addObj)
        {
            int listNum = GetAdsorptionListNum(addObj);
            if (listNum == -1) adsorptionObjs.Add(addObj);
            else return;
        }

        public void RemoveAdsorption(GameObject removeObj, bool destroySelf = false)
        {
            int listNum = GetAdsorptionListNum(removeObj);
            if (listNum == -1) return;
            else adsorptionObjs.RemoveAt(listNum);

            if (destroySelf) Destroy(removeObj);
        }

        #endregion

        //----------------------------------
        //  Init / Common
        //----------------------------------
        private void Start()
        {
            if (awakeEnable) StartDrag();
        }

        // �w�肵��Obj��uiDrags��List�ԍ���Ԃ�
        // �Ȃ��ꍇ��-1��Ԃ�
        private int GetDragListNum(GameObject dragObj)
        {
            if (dragObj == null) return -1;

            for (int i = 0; i < uiDrags.Count; i++)
            {
                if (uiDrags[i].gameObject == dragObj) return i;
            }

            return -1;
        }

        // �w�肵��Obj��uiDrags��List�ԍ���Ԃ�
        // �Ȃ��ꍇ��-1��Ԃ�
        private int GetAdsorptionListNum(GameObject adsorptionObj)
        {
            if (adsorptionObj == null) return -1;

            for (int i = 0; i < adsorptionObjs.Count; i++)
            {
                if (adsorptionObjs[i].gameObject == adsorptionObj) return i;
            }

            return -1;
        }
    }
}

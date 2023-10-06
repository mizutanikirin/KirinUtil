using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KirinUtil
{
    // Image�R���|�[�l���g��K�v�Ƃ���
    [RequireComponent(typeof(Image))]

    public class UIDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, IPointerDownHandler
    {

        private Image image;
        private RectTransform rectTransform;
        private float nearDistanceThreshold;
        private Vector2 safePosMin;
        private Vector2 safePosMax;

        private bool isDowned;
        private Vector3 prevPos;

        // ��_�i�}�E�X�̊�͍��������A�I�u�W�F�N�g�̊�͉�ʒ����ɂȂ�̂ŕ␳����B�j
        private Vector2 rootPos;

        private Vector2 startTouchLocalPos;
        private Canvas canvas;
        private RectTransform canvasTrf;
        private Camera canvasCamera;
        private Vector2 parentPos;
        private List<GameObject> adsorptionObjs;

        private Action<GameObject> DragAction;
        private Action<GameObject, GameObject> FitAction;
        private Action<GameObject> ClickAction;

        [Header("for confirmation (DON'T INPUT)")]
        [SerializeField] private bool isDragging;

        // Use this for initialization
        void Start()
        {
        }

        public UIDrag Init(
            Vector2 _parentPos, Rect dragArea, float _nearDistanceThreshold, 
            Canvas _canvas, Camera uiCamera, List<GameObject> _adsorptionObjs,
            Action<GameObject> dragAction, Action<GameObject, GameObject> fitAction, Action<GameObject> clickAction)
        {
            image = gameObject.GetComponent<Image>();
            rectTransform = gameObject.GetComponent<RectTransform>();
            nearDistanceThreshold = _nearDistanceThreshold;

            DragAction = dragAction;
            FitAction = fitAction;
            ClickAction = clickAction;

            image.alphaHitTestMinimumThreshold = 1;
            isDragging = false;
            canvasTrf = _canvas.GetComponent<RectTransform>();
            canvasCamera = uiCamera;
            canvas = _canvas;
            parentPos = _parentPos;
            adsorptionObjs = _adsorptionObjs;

            rootPos = new Vector3(parentPos.x, parentPos.y, 0f);

            safePosMin = new Vector2(dragArea.x, dragArea.y);
            safePosMax = new Vector2(dragArea.x + dragArea.width, dragArea.y + dragArea.height);

            return this;
        }


        //----------------------------------
        //  �h���b�O���h���b�v
        //----------------------------------
        #region �h���b�O���h���b�v
        public void OnBeginDrag(PointerEventData eventData)
        {
            // �h���b�O�O�̈ʒu���L�����Ă���
            prevPos = transform.localPosition;
            isDragging = true;
            startTouchLocalPos = GetStartTouchLocalPos();
            rectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // �h���b�O���͈ʒu���X�V����
            if (isDragging) 
                transform.localPosition = GetDragPosition(eventData.position) + startTouchLocalPos;
            else 
                transform.localPosition = prevPos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDragging)
            {
                transform.localPosition = prevPos;
                return;
            }

            transform.localPosition = GetDragPosition(eventData.position) + startTouchLocalPos;

            isDragging = false;

            // Drag�I����̈ʒu����
            AdjustPosition();
        }

        public void OnDrop(PointerEventData eventData)
        {
            //�g���Ă܂���B
            /*var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);


            foreach (var hit in raycastResults)
            {
                // ���� DroppableField �̏�Ȃ�A���̈ʒu�ɌŒ肷��
                if (hit.gameObject.CompareTag("DroppableField"))
                {
                    transform.position = hit.gameObject.transform.position;
                    this.enabled = false;
                }
            }*/
        }

        // �����I�Ƀh���b�O������
        public void ForceEndDrag()
        {
            if (isDragging)
            {
                isDragging = false;
                transform.position = prevPos;
            }
        }

        private Vector2 GetDragPosition(Vector2 eventDataPos)
        {
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTrf,
                eventDataPos,
                canvasCamera,
                out localPosition
            );

            return localPosition - rootPos;
        }

        private Vector2 GetStartTouchLocalPos()
        {
            //float adjustVar = 1;
            //if (Screen.width == 3840) adjustVar = 2;
            float adjustVar = Screen.width / 1920f;

            // �}�E�X�̏ꍇ�̏���
            if (Input.touchCount == 0)
            {
                return new Vector2(
                    transform.localPosition.x - (Input.mousePosition.x / adjustVar - 1920 / 2 - rootPos.x),
                    transform.localPosition.y + ((1080 - Input.mousePosition.y / adjustVar) - 1080 / 2 + rootPos.y)
                );
            }

            // �^�b�`�p�l���̏ꍇ�̏���
            float minDist = 10000;
            int touchNum = -1;
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 adjustTouchPos = new Vector2(
                    touch.position.x / adjustVar - 1920 / 2 - rootPos.x,
                    -((1080 - touch.position.y / adjustVar) - 1080 / 2 + rootPos.y)
                );
                float distance = Vector2.Distance(adjustTouchPos, transform.localPosition);
                if (minDist > distance)
                {
                    minDist = distance;
                    touchNum = i;
                }

            }
            if(touchNum == -1) return Vector2.zero;

            Touch touchNear = Input.GetTouch(touchNum);

            return new Vector2(
                transform.localPosition.x - (touchNear.position.x / adjustVar - 1920 / 2 - rootPos.x),
                transform.localPosition.y + ((1080 - touchNear.position.y / adjustVar) - 1080 / 2 + rootPos.y)
            );
        }

        public bool IsDrag()
        {
            return isDragging;
        }
        #endregion

        #region Drag�I����̈ʒu����
        private void AdjustPosition()
        {
            // �G���A�O�������ꍇ�G���A���Ɉړ�������
            MoveToSafeArea();

            // Map�̃u���b�N�ɋz��
            GameObject fitObj = MoveNearBlock();

            if (fitObj != null)
            {
                // fit�����Ƃ�
                FitAction.Invoke(gameObject, fitObj);
            }
            else
            {
                // fit���Ȃ������Ƃ�(�ʏ�̃h���b�O�I��)
                DragAction.Invoke(gameObject);
            }
        }

        // �G���A�O�������ꍇ�G���A���Ɉړ�������
        private void MoveToSafeArea()
        {
            Vector2 safePos = Vector2.zero;
            Vector2 blockPos = GetBlockPos(gameObject);
            if (safePosMin.x > blockPos.x) safePos.x = safePosMin.x + 1;
            if (safePosMin.y > blockPos.y) safePos.y = safePosMin.y + 1;
            if (safePosMax.x < blockPos.x) safePos.x = safePosMax.x - 1;
            if (safePosMax.y < blockPos.y) safePos.y = safePosMax.y - 1;

            if (safePos.x != 0) Util.LocalPosX(gameObject, safePos.x - parentPos.x);
            if (safePos.y != 0) Util.LocalPosY(gameObject, safePos.y - parentPos.y);

            //if(safePos.x != 0 || safePos.y != 0)
            //    transform.localPosition = prevPos;
        }


        // Map�̃u���b�N�ɋz��
        private GameObject MoveNearBlock()
        {
            // init
            float nearestDistance = 10000;
            Vector2 moveValue = Vector2.zero;
            GameObject nearestObj = null;

            // userBlock�̈ʒu
            Vector3 userBlockPos = GetBlockPos(gameObject);

            // ����������
            if (adsorptionObjs == null) return null;
            for (int j = 0; j < adsorptionObjs.Count; j++)
            {
                Vector3 blankBlockPos = GetBlockPos(adsorptionObjs[j]);
                float dist = Vector3.Distance(userBlockPos, blankBlockPos);
                if (nearestDistance > dist)
                {
                    moveValue = userBlockPos - blankBlockPos;
                    nearestDistance = dist;
                    nearestObj = adsorptionObjs[j];
                }
            }

            if (Mathf.Abs(moveValue.x) < nearDistanceThreshold && Mathf.Abs(moveValue.y) < nearDistanceThreshold)
            {
                //print("moveValue: " + moveValue);
                //print("Before: " + gameObject.transform.localPosition);
                gameObject.transform.localPosition = new Vector3(
                    gameObject.transform.localPosition.x - moveValue.x,
                    gameObject.transform.localPosition.y - moveValue.y,
                    0
                );
                //print("Move: " + gameObject.transform.localPosition);
            }
            else
            {
                nearestObj = null;
            }

            return nearestObj;

        }

        /*private void MovePrePos()
        {
            if (isDragging) return;
            iTween.MoveTo(gameObject,
                iTween.Hash(
                    "x", prevPos.x,
                    "y", prevPos.y,
                    "time", 0.3f,
                    "islocal", true,
                    "EaseType", iTween.EaseType.easeInOutQuart
                )
            );
        }*/
        #endregion


        //----------------------------------
        //  �^�b�v
        //----------------------------------
        #region �^�b�v
        public void OnPointerClick(PointerEventData eventData)
        {
            // �N���b�N���肪�h���b�O���ɋN����Ȃ��悤�ɂ��܂��B
            if (!isDragging && !isDowned)
            {
                ClickAction.Invoke(gameObject);
            }

            isDowned = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDowned = false;
        }


        /*private void Update()
        {
            if (isDown && !isDowned)
            {
                if (Time.realtimeSinceStartup - downStartTime > 0.1f)
                {
                    //Util.sound.PlaySE(0);
                    //isDowned = true;
                }
            }
        }*/
        #endregion


        //----------------------------------
        //  functions
        //----------------------------------
        private Vector3 GetBlockPos(GameObject blockObj)
        {
            if (blockObj == null) return Vector3.zero;

            Vector2 blockPos = Util.GetUIPos(
                canvasCamera, canvasCamera, canvas, blockObj);
            return new Vector3(blockPos.x, blockPos.y, 0);
        }
    }
}
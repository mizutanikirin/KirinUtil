using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil
{
    public class DetectSwipe : MonoBehaviour
    {
        private Vector2 fingerDown;
        private Vector2 fingerUp;

        // 指を離したときだけ処理を行う
        public bool detectSwipeOnlyAfterRelease = false;

        // しきい値以上スワイプするとスワイプとして検知する
        public float SWIPE_THRESHOLD = 20f;

        public bool isSwipeOn;

        //----------------------------------
        //  Event
        //----------------------------------
        #region Event

        [System.Serializable]
        public class SwipeLeftEvent : UnityEngine.Events.UnityEvent<float> { }
        [SerializeField]
        private SwipeLeftEvent swipeLeftEvent = new SwipeLeftEvent();

        [System.Serializable]
        public class SwipeRightEvent : UnityEngine.Events.UnityEvent<float> { }
        [SerializeField]
        private SwipeRightEvent swipeRightEvent = new SwipeRightEvent();

        void OnEnable()
        {
            swipeLeftEvent.AddListener(SwipeLeft);
            swipeRightEvent.AddListener(SwipeRight);
        }

        void OnDisable()
        {
            swipeLeftEvent.RemoveListener(SwipeLeft);
            swipeRightEvent.RemoveListener(SwipeRight);
        }

        private void SwipeLeft(float startPos)
        {
        }

        private void SwipeRight(float startPos)
        {
        }
        #endregion

        // スワイプ機能のOn/Off切り替え
        public void SwipeOn()
        {
            isSwipeOn = true;
        }
        public void SwipeOff()
        {
            isSwipeOn = false;
        }


        //----------------------------------
        //  Update
        //----------------------------------
        #region Update
        void Update()
        {
            if (!isSwipeOn) return;

            // スワイプをし始めた位置を記録する
            if (Input.GetMouseButtonDown(0))
            {
                fingerUp = Input.mousePosition;
                fingerDown = Input.mousePosition;

                print(fingerUp);
            }

            if (Input.GetMouseButton(0))
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = Input.mousePosition;
                    checkSwipe();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                fingerDown = Input.mousePosition;
                checkSwipe();
            }
        }

        void checkSwipe()
        {
            // しきい値以上縦方向にスワイプしたかどうか判定する
            /*if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
            {
                if (fingerDown.y - fingerUp.y > 0)
                {
                    OnSwipeUp();
                }
                else if (fingerDown.y - fingerUp.y < 0)
                {
                    OnSwipeDown();
                }
                fingerUp = fingerDown;
            }*/

            // しきい値以上横方向にスワイプしたかどうか判定する
            if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
            {
                if (fingerDown.x - fingerUp.x > 0)
                {
                    OnSwipeRight();
                }
                else if (fingerDown.x - fingerUp.x < 0)
                {
                    OnSwipeLeft();
                }
                fingerUp = fingerDown;
            }
        }

        float verticalMove()
        {
            return Mathf.Abs(fingerDown.y - fingerUp.y);
        }

        float horizontalValMove()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }
        #endregion


        //----------------------------------
        //  Detect
        //----------------------------------
        #region Detect
        /*void OnSwipeUp()
        {
        }

        void OnSwipeDown()
        {
        }*/

        void OnSwipeLeft()
        {
            swipeLeftEvent.Invoke(fingerUp.y);
        }

        void OnSwipeRight()
        {
            swipeRightEvent.Invoke(fingerUp.y);
        }
        #endregion
    }
}

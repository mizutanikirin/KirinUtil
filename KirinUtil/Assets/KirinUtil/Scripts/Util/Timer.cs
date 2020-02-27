using UnityEngine;
using System.Collections;


namespace KirinUtil {
    public class Timer {

        /// <summary>
        /// 経過時間
        /// </summary>
        /// <value>The current time.</value>
        public float CurrentTime {
            get;
            private set;
        }

        /// <summary>
        /// 残り時間
        /// </summary>
        /// <value>The remaining time.</value>
        public float RemainingTime {
            get {
                return LimitTime - CurrentTime;
            }
            private set {
            }
        }

        /// <summary>
        /// 停止時間
        /// </summary>
        /// <value>The limit time.</value>
        public float LimitTime {
            get;
            set;
        }

        /// <summary>
        /// LimitTimeまで時間が進んだら呼ばれる
        /// </summary>
        /// <value>The fire delegate.</value>
        public TimerDelegate.VoidDelegate FireDelegate {
            get;
            set;
        }

        bool isEnable = true;
        public bool IsEnable {
            get {
                return isEnable;
            }

            set {
                isEnable = value;
                if (!value) {
                    CurrentTime = 0;
                }
            }
        }

        /// <summary>
        /// 駆動中または有効になっていない場合はFalse、
        /// 時間に来たらTrueを返す。
        /// </summary>
        public bool Update() {
            if (IsEnable) {
                CurrentTime += Time.deltaTime;
                if (CurrentTime >= LimitTime) {
                    CurrentTime = 0;
                    if (FireDelegate != null) {
                        FireDelegate();
                    }
                    return true;
                }
                return false;
            } else {
                return false;
            }
        }
    }
}
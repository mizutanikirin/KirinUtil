using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {
    public class CountDown : MonoBehaviour {

        private bool timerFin;
        private float timerTime;
        private int limitTime;
        private Text timerText;
        private int soundSENum;
        private int soundPreNum;
        private bool zeroSoundPlay;

        public int CurrentTime {
            get;
            private set;
        }

        //----------------------------------
        //  init
        //----------------------------------
        // カウントダウン
        public void SetCountDown(int thisLimitTime) {
            timerFin = false;

            limitTime = thisLimitTime + 1;
            timerText = null;
            soundSENum = -1;
            soundPreNum = -1;
            zeroSoundPlay = false;
            CurrentTime = limitTime;
            timerTime = 0;
        }

        // カウントダウン + text
        public void SetCountDown(int thisLimitTime, Text thisTimerText) {
            timerFin = false;

            limitTime = thisLimitTime + 1;
            timerText = thisTimerText;
            soundSENum = -1;
            soundPreNum = -1;
            zeroSoundPlay = false;
            CurrentTime = limitTime;
            timerTime = 0;
            timerText.text = "";
        }

        // カウントダウン + text + 音
        public void SetCountDown(int thisLimitTime, Text thisTimerText, bool thisZeroSoundPlay, int thisSoundSENum, int thisSoundPreNum = -1) {
            timerFin = false;

            limitTime = thisLimitTime + 1;
            timerText = thisTimerText;
            soundSENum = thisSoundSENum;
            if (thisSoundPreNum == -1) soundPreNum = soundSENum;
            else soundPreNum = thisSoundPreNum;
            zeroSoundPlay = thisZeroSoundPlay;
            CurrentTime = limitTime;
            timerTime = 0;
            timerText.text = "";
        }

        // カウントダウン + 音
        public void SetCountDown(int thisLimitTime, bool thisZeroSoundPlay, int thisSoundSENum, int thisSoundPreNum = -1) {
            timerFin = false;

            limitTime = thisLimitTime + 1;
            timerText = null;
            soundSENum = thisSoundSENum;
            if (thisSoundPreNum == -1) soundPreNum = soundSENum;
            else soundPreNum = thisSoundPreNum;
            zeroSoundPlay = thisZeroSoundPlay;
            CurrentTime = limitTime;
            timerTime = 0;
        }

        //----------------------------------
        //  update
        //----------------------------------
        // カウントダウン
        public bool Update2() {
            TimerUpdate();

            int preTime = CurrentTime;
            CurrentTime = (int)(limitTime - timerTime);

            // text
            if (timerText != null) timerText.text = CurrentTime.ToString();

            // sound
            if (soundSENum >= 0) {
                if (preTime != CurrentTime && CurrentTime <= soundPreNum) {

                    if (CurrentTime == 0) {
                        if (zeroSoundPlay) Util.sound.PlaySE(soundSENum);
                    } else {
                        Util.sound.PlaySE(soundSENum);
                    }

                }
            }

            if (CurrentTime > 0) return false;
            else return true;
        }

        //----------------------------------
        //  function
        //----------------------------------
        private void TimerUpdate() {
            if (timerFin) return;

            timerTime += Time.deltaTime;
            if (timerTime >= limitTime) {
                timerTime = limitTime;
                timerFin = true;
            }
        }

    }

}

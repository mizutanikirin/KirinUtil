using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KirinUtil {
    public class SoundRecorder:MonoBehaviour {

        public AudioSource audioSource;
        private string filePath;
        private bool isRec;
        private float recTime;

        // Use this for initialization
        void Start() {
            filePath = "";
            isRec = false;

            audioSource.clip = Microphone.Start(null, true, 60, 44100);
            while (!( Microphone.GetPosition("") > 0 )) {
            }
        }

        // Update is called once per frame
        void Update() {
            if (isRec) {
                recTime += Time.deltaTime;
            }
        }

        public void StartRec(string soundPath) {
            isRec = true;

            filePath = soundPath;
            if (filePath == "" || filePath == null) {
                filePath = Application.dataPath + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".wav";
            }

            print("StartRec: " + filePath);

        }

        public void StopRec() {
            isRec = false;
            Microphone.End(null);

            /*recTime += 5.0f;
            float recTimeMin = recTime / 60.0f;

            AudioClip thisClip =  SavWav.TrimSilence(audioSource.clip, recTimeMin);

            if (thisClip == null) {
                print("StopRec: " + thisClip);
                return;
            }
            SavWav.Save(filePath, thisClip);*/

            SavWav.Save(filePath, audioSource.clip);
        }


        private void OnDestroy() {
            if (isRec) {
                isRec = false;
                Microphone.End(null);
            }
            audioSource.clip = null;
            audioSource = null;
        }
    }
}
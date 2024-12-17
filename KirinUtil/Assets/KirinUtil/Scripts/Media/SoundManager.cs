using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace KirinUtil
{
    [Serializable]
    public class BGMSound
    {
        public string id = "";
        public string fileName = "";
        //public bool loop = true;
        public bool mute = false;
        public float volume = 1.0f;

        public void Init()
        {
            id = "";
            fileName = "";
            //loop = true;
            mute = false;
            volume = 1.0f;
        }
    }

    [Serializable]
    public class SESound
    {
        public string id = "";
        public string fileName = "";
        public bool mute = false;
        public float volume = 1.0f;
        public bool loop = false;

        public void Init()
        {
            id = "";
            fileName = "";
            loop = false;
            mute = false;
            volume = 1.0f;
        }
    }

    // 音管理クラス
    public class SoundManager : MonoBehaviour
    {

        //----------------------------------
        //  var
        //----------------------------------
        protected static SoundManager instance;
        public static SoundManager Instance
        {
            get
            {
                return instance;
            }
        }

        public enum RootPath
        {
            dataPath,
            persistentDataPath,
            temporaryCachePath,
            streamingAssetsPath
        }
        [Separator("Common")]
        [SerializeField] RootPath rootPath;

        public string soundPath = "/../../AppData/Data/Sounds/";
        public bool awakeLoad;
        [SerializeField]
        private UnityEngine.Events.UnityEvent LoadedEvent = new UnityEngine.Events.UnityEvent();

        [Separator("SE")]
        // 音量
        public SESound[] seSound;
        [Separator("BGM")]
        public BGMSound[] bgmSound;

        private AudioSource BGMsource;
        private AudioSource[] SEsources;

        [NonSerialized]
        public List<AudioClip> BGM = new List<AudioClip>();
        [NonSerialized]
        public List<AudioClip> SE = new List<AudioClip>();

        private int allSoundNum = 0;
        private int loadedNum = 0;
        private int oneShotSoundCount;


        //----------------------------------
        //  init
        //----------------------------------
        #region init
        void OnEnable()
        {
            LoadedEvent.AddListener(Loaded);
        }

        void OnDisable()
        {
            LoadedEvent.RemoveListener(Loaded);
        }

        void Loaded()
        {
            print("Loaded All Sound");
        }

        void Awake()
        {

            instance = this;
            oneShotSoundCount = 0;

            /*GameObject[] obj = GameObject.FindGameObjectsWithTag("SoundManager");
            if (obj.Length > 1) {
                // 既に存在しているなら削除
                Destroy(gameObject);
            } else {
                // 音管理はシーン遷移では破棄させない
                DontDestroyOnLoad(gameObject);
            }*/

            // BGM AudioSource
            BGMsource = gameObject.AddComponent<AudioSource>();

            // SE AudioSource
            SEInit();

            if (awakeLoad) LoadSounds();
        }

        public void SEInit()
        {
            SEsources = new AudioSource[seSound.Length];
            for (int i = 0; i < SEsources.Length; i++)
            {
                SEsources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        void Update()
        {
            // ミュート設定
            for (int i = 0; i < bgmSound.Length; i++)
            {
                SetBGMMute(i, bgmSound[i].mute);
            }
            for (int i = 0; i < seSound.Length; i++)
            {
                SetSEMute(i, seSound[i].mute);
            }

            // ボリューム設定
            for (int i = 0; i < bgmSound.Length; i++)
            {
                SetBGMVolume(i, bgmSound[i].volume);
            }
            for (int i = 0; i < seSound.Length; i++)
            {
                SetSEVolume(i, seSound[i].volume);
            }
        }
        #endregion

        //----------------------------------
        //  Common
        //----------------------------------
        #region Load Sound file
        public void LoadSounds()
        {

            string rootDataPath;
            if (rootPath == RootPath.dataPath)
            {
                rootDataPath = Application.dataPath;
            }
            else if (rootPath == RootPath.persistentDataPath)
            {
                rootDataPath = Application.persistentDataPath;
            }
            else if (rootPath == RootPath.streamingAssetsPath)
            {
                rootDataPath = Application.streamingAssetsPath;
            }
            else
            {
                rootDataPath = Application.temporaryCachePath;
            }

            allSoundNum = bgmSound.Length + seSound.Length;
            for (int i = 0; i < bgmSound.Length; i++)
            {
                if (bgmSound[i].fileName != "")
                {
                    BGM.Add(null);
                    StartCoroutine(LoadSoundFile(rootDataPath + soundPath + bgmSound[i].fileName, "bgm", i));
                }
            }

            for (int i = 0; i < seSound.Length; i++)
            {
                if (seSound[i].fileName != "")
                {
                    SE.Add(null);
                    StartCoroutine(LoadSoundFile(rootDataPath + soundPath + seSound[i].fileName, "se", i));
                }
            }
        }

        IEnumerator LoadSoundFile(string path, string type, int soundNum)
        {
            print("LoadSound: " + path);

            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV);

            yield return request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                    if (type == "bgm") BGM[soundNum] = clip;
                    else SE[soundNum] = clip;
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("sound load error [ConnectionError]: " + path);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("sound load error [ProtocolError]: " + path);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("sound load error [DataProcessingError]: " + path);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            loadedNum++;
            if (allSoundNum == loadedNum)
            {
                LoadedEvent.Invoke();
            }
        }
        #endregion

        #region common
        public void AllStop()
        {
            StopBGM();
            StopSE();
        }
        #endregion


        //----------------------------------
        //  BGM
        //----------------------------------
        #region BGM

        #region BGM再生
        private int nowPlayBGMNum = 0;
        public void PlayBGM(int index)
        {
            if (0 > index || BGM.Count <= index)
            {
                return;
            }
            // 同じBGMの場合は何もしない
            if (BGMsource.clip == BGM[index])
            {
                return;
            }

            BGMsource.Stop();
            BGMsource.clip = BGM[index];
            BGMsource.volume = bgmSound[index].volume;
            BGMsource.mute = bgmSound[index].mute;
            //BGMsource.loop = bgmSound[index].loop;
            BGMsource.loop = true;
            BGMsource.Play();
            nowPlayBGMNum = index;
        }

        public void PlayBGM(string id)
        {
            int index = GetBGMListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return;
            }

            PlayBGM(index);
        }
        #endregion

        #region mute設定
        public void SetBGMMute(int index, bool mute)
        {
            if (0 > index || BGM.Count <= index)
            {
                return;
            }

            BGMsource.mute = mute;
            bgmSound[index].mute = mute;
        }

        public void SetBGMMute(string id, bool mute)
        {
            int index = GetBGMListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return;
            }

            SetBGMMute(index, mute);
        }

        // play中のBGMのmute設定
        public void SetBGMMute(bool mute)
        {
            if (BGMsource.isPlaying)
            {
                BGMsource.mute = mute;
                bgmSound[nowPlayBGMNum].mute = mute;
            }
        }
        #endregion

        #region volume設定
        public void SetBGMVolume(int index, float volume)
        {
            if (0 > index || BGM.Count <= index)
            {
                return;
            }

            BGMsource.volume = volume;
            bgmSound[index].volume = volume;
        }

        public void SetBGMVolume(string id, float volume)
        {
            int index = GetBGMListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return;
            }

            SetBGMVolume(index, volume);
        }

        // play中のBGMのvolume設定
        public void SetBGMVolume(float volume)
        {
            if (BGMsource.isPlaying)
            {
                BGMsource.volume = volume;
                bgmSound[nowPlayBGMNum].volume = volume;
            }
        }
        #endregion

        #region BGM停止
        public void StopBGM()
        {
            BGMsource.Stop();
            BGMsource.clip = null;
        }

        // ボリュームを絞って停止
        public void BGMFadeout()
        {
            StopCoroutine("BGMVolumeDown");
            StartCoroutine("BGMVolumeDown");
        }

        private IEnumerator BGMVolumeDown()
        {

            int max = 10;
            float nowVolume = BGMsource.volume;
            for (int i = 0; i < max; i++)
            {
                BGMsource.volume -= 1 / (float)max;
                bgmSound[nowPlayBGMNum].volume = BGMsource.volume;
                yield return new WaitForSeconds(0.1f);
            }
            StopBGM();
            BGMsource.volume = nowVolume;
            bgmSound[nowPlayBGMNum].volume = nowVolume;
        }
        #endregion

        #region function
        private int GetBGMListNum(string id)
        {
            for (int i = 0; i < bgmSound.Length; i++)
            {
                if (bgmSound[i].id == id)
                    return i;
            }

            return -1;
        }
        #endregion

        #endregion


        //----------------------------------
        //  SE
        //----------------------------------
        #region SE

        #region SE再生
        public void PlaySE(int index, bool playingPlay = true, bool oneShot = false)
        {
            if (0 > index || SE.Count <= index)
            {
                return;
            }

            // 再生中でないAudioSouceで鳴らす
            bool playOk = true;
            if (!playingPlay)
            {
                if (SEsources[index].isPlaying) playOk = false;
            }

            if (playOk)
            {
                if (oneShot)
                {
                    SEsources[index].PlayOneShot(SE[index], seSound[index].volume);
                }
                else
                {
                    SEsources[index].clip = SE[index];
                    SEsources[index].clip.name = "se" + index;
                    SEsources[index].volume = seSound[index].volume;
                    SEsources[index].loop = seSound[index].loop;
                    SEsources[index].mute = seSound[index].mute;
                    SEsources[index].Play();
                }
            }
        }

        public void PlaySE(string id, bool playingPlay = true, bool oneShot = false)
        {
            int index = GetSEListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return;
            }

            PlaySE(index, playingPlay, oneShot);
        }
        #endregion

        #region Mute設定
        public void SetSEMute(int index, bool mute)
        {
            if (0 > index || SE.Count <= index)
            {
                return;
            }

            SEsources[index].mute = mute;
            seSound[index].mute = mute;
        }

        public void SetSEMute(string id, bool mute)
        {
            int index = GetSEListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return;
            }

            SetSEMute(index, mute);
        }
        #endregion

        #region volume設定
        public void SetSEVolume(int index, float volume)
        {
            if (0 > index || SE.Count <= index)
            {
                return;
            }

            SEsources[index].volume = volume;
            seSound[index].volume = volume;
        }

        public void SetSEVolume(string id, float volume)
        {
            int index = GetSEListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return;
            }

            SetSEVolume(index, volume);
        }
        #endregion

        #region 全SE停止
        public void StopSE()
        {
            // 全てのSE用のAudioSouceを停止する

            for (int i = 0; i < SEsources.Length; i++)
            {
                SEsources[i].Stop();
                SEsources[i].clip = null;
            }
        }
        #endregion

        #region 指定したSEを停止
        public void StopSE(int index)
        {
            if (0 > index || SE.Count <= index)
            {
                return;
            }

            SEsources[index].Stop();
            SEsources[index].clip = null;
        }

        public void StopSE(string id)
        {
            int index = GetSEListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return;
            }

            StopSE(index);
        }
        #endregion

        #region 指定したSEが再生中かどうか
        public bool IsPlaying(int index)
        {
            if (0 > index || SE.Count <= index)
            {
                return false;
            }

            return SEsources[index].isPlaying;
        }

        public bool IsPlaying(string id)
        {
            int index = GetSEListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return false;
            }

            return IsPlaying(index);
        }
        #endregion

        #region 指定したSEの現在の再生時間取得
        public float GetCurrntTime(int index)
        {
            if (0 > index || SE.Count <= index)
            {
                return 0;
            }

            return SEsources[index].time;
        }

        public float GetCurrntTime(string id)
        {
            int index = GetSEListNum(id);
            if (index == -1)
            {
                Debug.LogWarning("Not found sound id");
                return 0;
            }

            return GetCurrntTime(index);
        }
        #endregion

        #region function
        private int GetSEListNum(string id)
        {
            for (int i = 0; i < seSound.Length; i++)
            {
                if (seSound[i].id == id)
                    return i;
            }

            return -1;
        }
        #endregion

        #endregion


        //----------------------------------
        //  1ファイルずつ読み込み再生
        //----------------------------------
        #region Play one file

        [NonSerialized] public List<AudioSource> audioSourceLoadAndPlay = new List<AudioSource>();
        private List<bool> loadedLoadAndPlay = new List<bool>();
        public void LoadAndPlay(string soundPath, float volume, string id, bool isLocal = true)
        {
            if (soundPath == "")
                return;
            if (isLocal)
            {
                if (!File.Exists(soundPath))
                    return;
            }
            print("------ LoadAndPlay: " + soundPath);
            int listNum = GetAudioSourceListNum(id);
            if (listNum == -1)
            {
                // listになかったら再生する
                loadedLoadAndPlay.Add(false);
                StartCoroutine(LoadOneSoundFile(soundPath, volume, id, isLocal));
            }
            else
            {
                // listにidがあったら前のを再生する
                audioSourceLoadAndPlay[listNum].volume = volume;
                audioSourceLoadAndPlay[listNum].Play();
            }

            GameObject preSoundObj = GameObject.Find(
                gameObject.transform.parent.GetComponent<KRNMedia>().GetGameObjectPath(gameObject) + "/sound" + (oneShotSoundCount - 1)
            );
            if (preSoundObj != null) Destroy(preSoundObj);
        }


        IEnumerator LoadOneSoundFile(string path, float volume, string id, bool isLocal)
        {
            string filePath = "file://" + path;
            if (!isLocal) filePath = path;

            using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
            {
                yield return request.SendWebRequest();

                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        LoadSuccess(request, id, volume);
                        break;
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError("Load error [ConnectionError]: " + path);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("Load error [ProtocolError]: " + path);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError("Load error [DataProcessingError]: " + path);
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void LoadSuccess(UnityWebRequest request, string id, float volume)
        {
            DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)request.downloadHandler;
            AudioClip clip = dlHandler.audioClip;

            /*GameObject soundObj = new GameObject();
            soundObj.name = "sound" + oneShotSoundCount;
            soundObj.transform.SetParent(gameObject.transform);*/

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.clip.name = "oneShotSound_" + id;
            audioSource.volume = volume;
            audioSource.Play();
            loadedLoadAndPlay[loadedLoadAndPlay.Count - 1] = true;
            print("loadedLoadAndPlay: true");
            audioSourceLoadAndPlay.Add(audioSource);

            oneShotSoundCount++;
        }

        public bool LoadedOneShotSound(string id)
        {
            int listNum = GetAudioSourceListNum(id);
            if (listNum == -1) return false;

            if (loadedLoadAndPlay.Count > listNum) return loadedLoadAndPlay[listNum];
            else return false;
        }

        public bool IsPlayingOneShotSound(string id)
        {
            int listNum = GetAudioSourceListNum(id);
            if (listNum == -1) return false;

            if (audioSourceLoadAndPlay[listNum] == null) return false;
            if (loadedLoadAndPlay.Count <= listNum) return false;

            return audioSourceLoadAndPlay[listNum].isPlaying;
        }

        public void StopOneShotSound(string id)
        {
            int listNum = GetAudioSourceListNum(id);

            print(id + "  " + listNum);

            if (listNum == -1) return;
            if (audioSourceLoadAndPlay[listNum] == null) return;
            if (loadedLoadAndPlay.Count <= listNum) return;

            print(audioSourceLoadAndPlay[listNum].isPlaying);
            if (audioSourceLoadAndPlay[listNum].isPlaying)
            {
                audioSourceLoadAndPlay[listNum].Stop();
                Destroy(audioSourceLoadAndPlay[listNum]);
                audioSourceLoadAndPlay[listNum] = null;
            }

            RemovetAudioSource();

        }

        private void RemovetAudioSource()
        {
            for (int i = audioSourceLoadAndPlay.Count - 1; i >= 0; i--)
            {
                if (audioSourceLoadAndPlay[i] == null)
                {
                    audioSourceLoadAndPlay.RemoveAt(i);
                    loadedLoadAndPlay.RemoveAt(i);
                }
            }
        }

        private int GetAudioSourceListNum(string id)
        {

            int listNum = -1;
            for (int i = 0; i < audioSourceLoadAndPlay.Count; i++)
            {
                if (audioSourceLoadAndPlay[i].clip.name == "oneShotSound_" + id)
                {
                    listNum = i;
                    break;
                }
            }


            return listNum;
        }

        #endregion
    }


}
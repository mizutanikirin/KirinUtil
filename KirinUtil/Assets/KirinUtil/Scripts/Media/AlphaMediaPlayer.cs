using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static KirinUtil.AlphaMediaManager;

namespace KirinUtil
{
    public class AlphaMediaPlayer : MonoBehaviour
    {
        // Common
        private Media media;
        private AlphaMediaManager alphaMediaManager;
        private bool isPlaying = false;
        private bool isPaused = false;

        // for image
        private List<GameObject> imageObjs;
        private int playImageCurrentFrame;
        private Coroutine playCoroutine = null;
        private WaitForSeconds waitSec;

        //[Separator("Vido Setting")]
        private RawImage videoImage;
        private VideoPlayer videoPlayer;
        private bool videoPrepareCompleted = false;
        private bool moviePreparePlay;

        //----------------------------------
        //  Public
        //----------------------------------
        #region Public
        public void Init(Media _media, AlphaMediaManager _alphaMediaManager)
        {
            media = _media;
            alphaMediaManager = _alphaMediaManager;
            if (media.type == MediaType.Image) ImageInit();
            else
            {
                if(media.startVisible && !media.playOnAwake) MoviePrepare(false);
                if (media.playOnAwake) Play();
            }
        }

        public void Play()
        {
            if (isPlaying) return;
            gameObject.SetActive(true);

            isPlaying = true;
            if (media.type == MediaType.Image) ImagePlay();
            else MoviePlay();
        }

        public void Pause()
        {
            if (!isPlaying) return;

            isPlaying = false;
            isPaused = true;
            if (media.type == MediaType.Image) ImagePause();
            else MoviePause();
        }

        public void Stop()
        {
            if (!isPlaying) return;

            isPlaying = false;
            isPaused = false;
            if (media.type == MediaType.Image) ImageStop();
            else MovieStop();
        }

        public void Destroy()
        {
            if (playCoroutine != null) StopCoroutine(playCoroutine);
            Destroy(gameObject);
        }
        #endregion


        //----------------------------------
        //  Image
        //----------------------------------
        #region Image
        // init
        private void ImageInit()
        {
            imageObjs = Util.media.GetChildGameObject(gameObject);
            for (int i = 0; i < imageObjs.Count; i++)
            {
                imageObjs[i].SetActive(false);
            }
            imageObjs[0].SetActive(true);
            playImageCurrentFrame = 0;
            waitSec = new WaitForSeconds(1 / media.framerate);

            if (media.playOnAwake) Play();
        }

        // play
        private void ImagePlay()
        {
            if (imageObjs.Count == 1)
            {
                imageObjs[0].SetActive(true);
            }
            else
            {
                // Stopした後のPlay()実行時はVideoPlayerの挙動と
                // 合わせてはじめのフレームから始める
                if (!isPaused) playImageCurrentFrame = 0;
                isPaused = false;
                playCoroutine = StartCoroutine(PlayLoop());
            }
        }

        private IEnumerator PlayLoop()
        {
            if(!isPlaying) yield break;

            // 現在のフレームを表示
            for (int i = 0; i < imageObjs.Count; i++)
            {
                imageObjs[i].SetActive(false);
            }
            imageObjs[playImageCurrentFrame].SetActive(true);

            // 待機
            yield return waitSec;
            if (!isPlaying) yield break;

            // 次のフレーム番号の決定
            bool isEnd = false;
            playImageCurrentFrame++;
            if (playImageCurrentFrame == imageObjs.Count)
            {
                if (media.isLoop) playImageCurrentFrame = media.loopStartNum;
                else isEnd = true;
            }

            // 次のフレームへ進むか / 終わるか
            if (isEnd) alphaMediaManager.PlayEnd(media.id);
            else playCoroutine = StartCoroutine(PlayLoop());
        }

        private void ImagePause()
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
            }
        }

        // Stop
        private void ImageStop()
        {
            if (playCoroutine != null) StopCoroutine(playCoroutine);
        }
        #endregion


        //----------------------------------
        //  Video
        //----------------------------------
        #region Video
        private void MoviePlay()
        {
            if (videoPrepareCompleted)
            {
                if (!isPaused) videoPlayer.frame = 0;
                videoPlayer.Play();  // 再生が1度されている場合
            }
            else MoviePrepare(true);  // 1度も再生されていない場合
        }

        private void MoviePrepare(bool play)
        {
            if (videoPrepareCompleted) return;

            moviePreparePlay = play;
            videoPrepareCompleted = true;
            if (videoPlayer == null) videoPlayer = gameObject.GetComponent<VideoPlayer>();
            if (videoImage == null) videoImage = videoPlayer.GetComponent<RawImage>();

            videoImage.enabled = false;
            SetVideoImage();

            videoPlayer.renderMode = VideoRenderMode.APIOnly;
            videoPlayer.isLooping = media.isLoop;
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += OnCompletePrepare;
            videoPlayer.errorReceived += OnErrorReceived;
            if (!media.isLoop) videoPlayer.loopPointReached += LoopPointReached;
        }

        private void SetVideoImage()
        {
            videoImage.texture = videoPlayer.texture;
            videoImage.rectTransform.sizeDelta = new Vector2(
                (int)videoPlayer.width, (int)videoPlayer.height
            );
            //print("---------- SetVideoImage: " + videoImage.rectTransform.sizeDelta);
        }

        private void OnCompletePrepare(VideoPlayer player)
        {
            // Prepare前にサイズ取得できない場合があるのでここでサイズ変更をする
            SetVideoImage();

            //print("-------- OnCompletePrepare: " + player.isPlaying + ", " + moviePreparePlay);
            videoImage.enabled = true;
            player.prepareCompleted -= OnCompletePrepare;
            if(moviePreparePlay) player.Play();
            else player.Pause();
            moviePreparePlay = false;
        }

        private void OnErrorReceived(VideoPlayer player, string message)
        {
            Debug.LogError(message);
        }

        private void LoopPointReached(VideoPlayer player)
        {
            player.loopPointReached -= LoopPointReached;
            alphaMediaManager.PlayEnd(media.id);
        }

        private void MoviePause()
        {
            videoPlayer.Pause();
        }

        private void MovieStop()
        {
            // API Only設定でStop()にすると消えてしまうのでPause()にする
            videoPlayer.Pause();
            //videoPlayer.Stop();
        }
        #endregion

    }
}

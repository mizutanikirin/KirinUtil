using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class SlideDemo : MonoBehaviour
    {
        [SerializeField] private SlideManager slide;
        [SerializeField] private string slideId;

        // Start is called before the first frame update
        void Start()
        {
            // ※ imageはImageManagerを使用してファイルを読み込んでいます。
            slide.Set();

            // ムービーが必要な場合(スライド登録したあとにムービーをロードしないといけない)
            // ※ Util.movieを使用する場合はMovieManagerを使用できる状態になっていないといけません。
            //Util.movie.LoadUIMovies();

            slide.Play(slideId);
        }

        public void SlideEnd(string slideId)
        {
            print("SlideEnd: " + slideId);
            slide.Play(slideId);
        }
    }
}

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
            // �� image��ImageManager���g�p���ăt�@�C����ǂݍ���ł��܂��B
            slide.Set();

            // ���[�r�[���K�v�ȏꍇ(�X���C�h�o�^�������ƂɃ��[�r�[�����[�h���Ȃ��Ƃ����Ȃ�)
            // �� Util.movie���g�p����ꍇ��MovieManager���g�p�ł����ԂɂȂ��Ă��Ȃ��Ƃ����܂���B
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

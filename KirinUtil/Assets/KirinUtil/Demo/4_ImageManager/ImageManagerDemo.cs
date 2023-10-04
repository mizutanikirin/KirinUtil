using KirinUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManagerDemo : MonoBehaviour
{
    [SerializeField] private RawImage b1Image;

    // Start is called before the first frame update
    void Start()
    {
        // Inspector > UI Image �ɓo�^����Ă���摜��ǂݍ���ŕ\��������
        Util.image.LoadImages();

        // Inspector > Only Texture �ɓo�^����Ă���摜��ǂݍ���
        Util.image.LoadTexture2DList();
        b1Image.texture = Util.image.textures[0];

        // �A�ԉ摜�̓ǂݍ���+�Đ�
        Util.image.LoadPlayImages();
        Util.image.PlayImage("playTest");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Util.image.LoadImages()���I��������Ăяo�����
    // ���̊֐���Inspector > ImageManager > Loaded Image Event �ɓo�^����Ă��܂��B
    public void LoadedImages()
    {
        Debug.Log("LoadedImages");
    }

    // �A�ԉ摜�̍Đ�(Util.image.PlayImage("playTest"))���I��������Ăяo�����
    // ���̊֐���Inspector > ImageManager > Play Image End Event �ɓo�^����Ă��܂��B
    public void PlayImageEnd()
    {
        Debug.Log("PlayImageEnd");
    }

}

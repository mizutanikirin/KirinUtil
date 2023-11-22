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
        // Inspector > UI Image に登録されている画像を読み込んで表示させる
        Util.image.LoadImages();

        // Inspector > Only Texture に登録されている画像を読み込む
        Util.image.LoadTexture2DList();
        b1Image.texture = Util.image.textures[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Util.image.LoadImages()が終了したら呼び出される
    // この関数はInspector > ImageManager > Loaded Image Event に登録されています。
    public void LoadedImages()
    {
        Debug.Log("LoadedImages");
    }

    // 連番画像の再生(Util.image.PlayImage("playTest"))が終了したら呼び出される
    // この関数はInspector > ImageManager > Play Image End Event に登録されています。
    public void PlayImageEnd()
    {
        Debug.Log("PlayImageEnd");
    }

}

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
        // Inspector > UI Image ‚É“o˜^‚³‚ê‚Ä‚¢‚é‰æ‘œ‚ğ“Ç‚İ‚ñ‚Å•\¦‚³‚¹‚é
        Util.image.LoadImages();

        // Inspector > Only Texture ‚É“o˜^‚³‚ê‚Ä‚¢‚é‰æ‘œ‚ğ“Ç‚İ‚Ş
        Util.image.LoadTexture2DList();
        b1Image.texture = Util.image.textures[0];

        // ˜A”Ô‰æ‘œ‚Ì“Ç‚İ‚İ+Ä¶
        Util.image.LoadPlayImages();
        Util.image.PlayImage("playTest");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Util.image.LoadImages()‚ªI—¹‚µ‚½‚çŒÄ‚Ño‚³‚ê‚é
    // ‚±‚ÌŠÖ”‚ÍInspector > ImageManager > Loaded Image Event ‚É“o˜^‚³‚ê‚Ä‚¢‚Ü‚·B
    public void LoadedImages()
    {
        Debug.Log("LoadedImages");
    }

    // ˜A”Ô‰æ‘œ‚ÌÄ¶(Util.image.PlayImage("playTest"))‚ªI—¹‚µ‚½‚çŒÄ‚Ño‚³‚ê‚é
    // ‚±‚ÌŠÖ”‚ÍInspector > ImageManager > Play Image End Event ‚É“o˜^‚³‚ê‚Ä‚¢‚Ü‚·B
    public void PlayImageEnd()
    {
        Debug.Log("PlayImageEnd");
    }

}

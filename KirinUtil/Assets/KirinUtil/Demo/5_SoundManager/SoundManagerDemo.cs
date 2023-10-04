using KirinUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Util.sound.LoadSounds(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadedSounds()
    {
        Util.sound.PlayBGM("bgm");
        StartCoroutine(PlaySE());
    }

    private IEnumerator PlaySE()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1.0f);
            Util.sound.PlaySE("count");
        }
    }

    public void SetBGMVolume(float value)
    {
        Util.sound.SetBGMVolume(value);
    }

    public void StopBGM()
    {
        Util.sound.StopBGM();
    }

    public void PlayBgm()
    {
        Util.sound.PlayBGM("bgm");
    }
}

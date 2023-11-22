using KirinUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaMediaDemo : MonoBehaviour
{
    [SerializeField] private AlphaMediaManager alphaMediaManager;

    // Start is called before the first frame update
    void Start()
    {
        //alphaMediaManager.LoadAlphaMedias();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlphaMediaLoaded()
    {
        print("AlphaMediaLoaded");
    }

    public void AlphaMediaPlayEnd(string id)
    {
        print("AlphaMediaPlayEnd: " + id);
    }

    //----------------------------------
    //  button
    //----------------------------------
    public void PlayBtnClick()
    {
        alphaMediaManager.Play("VideoTest");
        alphaMediaManager.Play("ImageTest");
    }

    public void PauseBtnClick()
    {
        alphaMediaManager.Pause("VideoTest");
        alphaMediaManager.Pause("ImageTest");
    }

    public void StopBtnClick()
    {
        alphaMediaManager.Stop("VideoTest");
        alphaMediaManager.Stop("ImageTest");
    }
}

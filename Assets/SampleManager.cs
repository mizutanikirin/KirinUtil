using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SampleManager : MonoBehaviour {

	public KirinUtil util;
	public GameObject alphaObj;
	public GameObject[] fadeObj;
	public Button[] toggleBtn;
	public GameObject[] ScenesCanvas;
	public GameObject centerObj;

	// Use this for initialization
	void Start () {

		//----------------------------------
		//  base setting
		//----------------------------------
		// BasicSetting( bool cursorVisible )
		util.BasicSetting (false);


		//----------------------------------
		//  ShuffleArray
		//----------------------------------
		// ShuffleArray( int[] arr )
		// ShuffleArray( float[] arr )
		// ShuffleArray( string[] arr )
		int[] testArr = new int[5];
		for(int i = 0; i < 5;i++){
			testArr [i] = i;
		}
		testArr = util.ShuffleArray(testArr);
		for(int i = 0; i < testArr.Length; i++){
			print( "ShuffleArrayList: " + testArr[i] );
		}
		print ("-----------------");


		//----------------------------------
		//  open text file
		//----------------------------------
		// OpenTextFile( string filePath )
		string sampleText = util.OpenTextFile(Application.dataPath + "/openSample.txt");
		print ("OpenTextFile: " + sampleText);
		print ("-----------------");


		//----------------------------------
		//  the range of time
		//----------------------------------
		// TimeArea( string startTime, string nowTime, string endTime ))
		print( "TimeArea: " + util.TimeArea( "8:27", "10:50", "12:30" ));

		// WeekArea( int _startWeek, int _endWeek )
		// 0:Sunday ~ 6:Saturday
		print( "WeekArea: " + util.WeekArea( 0, 6 ));
		print ("-----------------");


		//----------------------------------
		//  alpha texture
		//----------------------------------
		alphaObj.GetComponent<Renderer>().material.SetAlpha (0.5f);


		//----------------------------------
		//  fade gameObject
		//----------------------------------
		//// FadeOut
		// FadeOut( GameObject _obj, float _fadeTime, float _delayTime )
		util.FadeOut(fadeObj[0], 1.0f, 0.0f);

		//// FadeInOut
		// FadeInOut( GameObject _nowObj, GameObject _nextObj, float _fadeTime, float _delayTime )
		//fadeObj[1].GetComponent<Renderer>().material.SetAlpha (0.0f);
		//util.FadeInOut(fadeObj[0], fadeObj[1], 1.0f, 1.5f);

		//// FadeIn
		// FadeIn( GameObject _obj, float _fadeTime, float _delayTime )
		//util.FadeIn(fadeObj[0], 1.0f, 2.5f);


		//----------------------------------
		//  SceneNext
		//----------------------------------
		// 1. Add Component -> CanvasGroup
		// 2. CanvasSceneNext( GameObject preObj, GameObject nextObj, float time )
		util.CanvasSceneNext(ScenesCanvas[0], ScenesCanvas[1], 1.0f);


		//----------------------------------
		//  HttpConnect
		//----------------------------------
		// get
		/*HttpConnect.Get("http://example.com/", www => {
			// ok
			Debug.Log(www.text);	
		}, www => {
			// on error
			Debug.Log(www.error);
		});*/

		// post
		/*var form = new Dictionary<string, string>();
		form.Add("id", "test");
		form.Add("pw", "pass");

		HttpConnect.Post ("http://example.com/", form, www => {
			// ok
			Debug.Log ("HttpConnect ok: " + www.text);
		}, www => {
			// on error
			Debug.Log ("HttpConnect error: " + www.error);
		});*/

		//----------------------------------
		//  CenterGameObject
		//----------------------------------
		util.Center(centerObj, Camera.main);

		//----------------------------------
		//  Run app
		//----------------------------------
		//ProcessApp.Run("/Applications/Processing.app");
		//ProcessApp.Exit ();


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

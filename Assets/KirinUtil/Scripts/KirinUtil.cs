////////////////////////////////////////////////////////////////////////////////
//
//  MIZUTANI KIRIN
//  Copyright 2016 MIZUTANI KIRIN All Rights Reserved.
//
//  NOTICE: Designium permits you to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;

public class KirinUtil : MonoBehaviour {

	//----------------------------------
	//  vars
	//----------------------------------
	private GameObject preSceneObj;
	private GameObject nextSceneObj;
	
	void Awake () {
	}
	
	void Start () {
	}
	
	void Update () {
	}
	
	//----------------------------------
	//  BasicSetting
    //----------------------------------
    #region BasicSetting
	public void BasicSetting(bool cursorVisible){
		
		// Escでアプリ終了
		if (Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		}
		
		// カーソルを非表示
		Cursor.visible = cursorVisible;

        print("INIT KIRIN UTIL");
	}
    #endregion

    //----------------------------------
	//  ShuffleArray
	//----------------------------------
    #region ShuffleArray
	public int[] ShuffleArray( int[] arr ) {
		int length = arr.Length;
		int[] newArr = new int[length];
		newArr = arr;
		
		while( length != 0 ){
			int rnd = (int)Mathf.Floor(UnityEngine.Random.value * length);
			length--;
			var tmp = newArr[length];
			newArr[length] = newArr[rnd];
			newArr[rnd] = tmp;
		}
		
		return newArr;
	}

	public float[] ShuffleArray( float[] arr ) {
		int length = arr.Length;
		float[] newArr = new float[length];
		newArr = arr;

		while( length != 0 ){
			int rnd = (int)Mathf.Floor(UnityEngine.Random.value * length);
			length--;
			var tmp = newArr[length];
			newArr[length] = newArr[rnd];
			newArr[rnd] = tmp;
		}

		return newArr;
	}

	public string[] ShuffleArray( string[] arr ) {
		int length = arr.Length;
		string[] newArr = new string[length];
		newArr = arr;

		while( length != 0 ){
			int rnd = (int)Mathf.Floor(UnityEngine.Random.value * length);
			length--;
			var tmp = newArr[length];
			newArr[length] = newArr[rnd];
			newArr[rnd] = tmp;
		}

		return newArr;
	}
    #endregion
	
	//----------------------------------
	//  OpenTextFile
    //----------------------------------
    #region OpenTextFile
    public string OpenTextFile( string filePath ){
		FileInfo fi = new FileInfo(filePath);
		string returnSt = "";
		
		try {
			using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)){
				returnSt = sr.ReadToEnd();
			}
		} catch (Exception e){
			print (e.Message);
			returnSt = "READ ERROR: " + filePath;
		}
		
		return returnSt;
	}
    #endregion
	
    //----------------------------------
	//  TimeArea
    //----------------------------------
    #region TimeArea
	public bool TimeArea( string startTime, string nowTime, string endTime ){
		
		bool returnFlag = false;

		string[] startTimeTmp = startTime.Split(":"[0]);
		int startHour = int.Parse(startTimeTmp[0]);
		int startMinute = int.Parse(startTimeTmp[1]);

		string[] nowTimeTmp= nowTime.Split(":"[0]);
		int nowHour = int.Parse(nowTimeTmp[0]);
		int nowMinute = int.Parse(nowTimeTmp[1]);
		
		string[] endTimeTmp = endTime.Split(":"[0]);
		int endHour = int.Parse(endTimeTmp[0]);
		int endMinute = int.Parse(endTimeTmp[1]);
		
		// 
		if( nowHour > startHour ){ // 10:50 - 14:45 - 
			if( nowHour < endHour ){
				// (1) 10:50 - 14:45 - 16:20
				returnFlag = true;
			}else if( nowHour == endHour ){
				if( nowMinute > endMinute ){
					// (2) 10:50 - 14:45 - 14:20
					returnFlag = false;
				}else{
					// (3) 10:50 - 14:45 - 14:50
					returnFlag = true;
				}
			}else{
				if( startHour < endHour ){
					// (4) 10:50 - 14:45 - 13:20
					returnFlag = false;
				}else if( startHour == endHour ){
					if( startMinute > endMinute ){
						// (5) 10:50 - 14:45 - 10:40
						returnFlag = true;
					}else{
						// (6) 10:50 - 14:45 - 10:52;
						returnFlag = false;
					}
				}else{
					// (7) 11:50 - 14:45 - 10:50;
					returnFlag = true;
				}
			}
		}else if( nowHour == startHour ){ // 14:20 - 14:45 -
			if( nowMinute >= startMinute ){
				if( nowHour < endHour ){
					// (5) 14:20 - 14:45 - 16:20
					returnFlag = true;
				}else if( nowHour == endHour ){
					if( nowMinute > endMinute ){
						if( startMinute >= endMinute ){
							// (6) 14:20 - 14:45 - 14:10
							returnFlag = true;
						}else{
							// (7) 14:20 - 14:45 - 14:30
							returnFlag = false;
						}
						
					}else{
						// (7) 14:20 - 14:45 - 14:50
						returnFlag = true;
					}
				}else{
					// (4) 14:20 - 14:45 - 13:50
					returnFlag = true;
				}
			}else{
				if( startHour < endHour ){
					// (4) 14:50 - 14:45 - 15:20
					returnFlag = false;
				}else if( startHour == endHour ){
					if( startMinute > endMinute ){
						// (4) 14:50 - 14:45 - 14:20
						returnFlag = false;
					}else if( startMinute == endMinute ){
						// (4) 14:50 - 14:45 - 14:50
						returnFlag = true;
					}else{
						// (4) 14:50 - 14:45 - 14:55
						returnFlag = false;
					}
				}else{
					// (4) 14:50 - 14:45 - 13:20
					returnFlag = false;
				}
			}
		}else{ // 18:20 - 14:45 - 
			if( nowHour < endHour ){
				if( startHour > endHour ){
					// (4) 18:20 - 14:45 - 16:20
					returnFlag = true;
				}else if( startHour == endHour ){
					if( startMinute < endMinute ){
						// 18:20 - 14:45 - 18:30
						returnFlag = false;
					}else if( startMinute == endMinute ){
						// 18:20 - 14:45 - 18:20
						returnFlag = true;
					}else{
						// 18:30 - 14:45 - 18:20
						returnFlag = true;
					}
				}else{
					// 18:20 - 14:45 - 19:20
					returnFlag = false;
				}
				
			}else if( nowHour == endHour ){
				if( nowMinute > endMinute ){
					// (2) 18:20 - 14:45 - 14:20
					returnFlag = false;
				}else{
					// (3) 18:20 - 14:45 - 14:50
					returnFlag = true;
				}
			}else{
				// (3) 18:20 - 14:45 - 12:50
				returnFlag = false;
			}
		}
		
		return returnFlag;
	}
    #endregion

	//----------------------------------
	//  WeekArea
	//----------------------------------
	#region WeekArea
	public bool WeekArea( int _startWeek, int _endWeek ){
		bool returnFlag = false;
		int nowWeek = WeekNum (System.DateTime.Now.DayOfWeek.ToString());

		if( _startWeek < _endWeek ){
			// 通常時
			if( nowWeek >= _startWeek && nowWeek <= _endWeek ){
				returnFlag = true;
			}
		}else if( _startWeek > _endWeek ){
			// 土日を挟む時
			if( nowWeek >= _startWeek ){
				returnFlag = true;
			}else if( nowWeek <= _endWeek ){
				returnFlag = true;
			}
		}else if( _startWeek == _endWeek ){
			// _startWeekと_endWeekが同じ時
			if( nowWeek == _startWeek ){
				returnFlag = true;
			}
		}

		return returnFlag;
	}

	private int WeekNum(string week){
		int weekNum = -1;

		if (week == "Sunday")
			weekNum = 0;
		else if( week == "Monday" )
			weekNum = 1;
		else if( week == "Tuesday" )
			weekNum = 2;
		else if( week == "Wednesday" )
			weekNum = 3;
		else if( week == "Thursday" )
			weekNum = 4;
		else if( week == "Friday" )
			weekNum = 5;
		else if( week == "Saturday" )
			weekNum = 6;

		return weekNum;
	}
	#endregion

    //----------------------------------
    //  FadeIn+FadeOut
    //----------------------------------
    #region FadeInOut

    public void FadeInOut( GameObject _nowObj, GameObject _nextObj, float _fadeTime, float _delayTime ) {
        iTween.FadeTo(_nowObj, iTween.Hash("alpha", 0, "time", _fadeTime));
        iTween.FadeTo(_nextObj, iTween.Hash("alpha", 1, "time", _fadeTime, "delay", _delayTime));
    }

    public void FadeIn( GameObject _obj, float _fadeTime, float _delayTime ) {
        iTween.FadeTo(_obj, iTween.Hash("alpha", 1, "time", _fadeTime, "delay", _delayTime));
    }

    public void FadeOut( GameObject _obj, float _fadeTime, float _delayTime ) {
        iTween.FadeTo(_obj, iTween.Hash("alpha", 0, "time", _fadeTime, "delay", _delayTime));
    }
    #endregion

	//----------------------------------
	//  SceneNext
	//----------------------------------
	#region CanvasSceneNext
	public void CanvasSceneNext( GameObject preObj, GameObject nextObj, float time ){

		preSceneObj = preObj;
		nextSceneObj = nextObj;

		nextObj.SetActive (true);
		nextObj.GetComponent<CanvasGroup>().alpha = 0.0f;

		iTween.ValueTo(gameObject,
			iTween.Hash(
				"from",1.0f,
				"to",0.0f,
				"time",time,
				"easetype","easeOutCubic",
				"onUpdate","FadeUpdate",
				"oncomplete", "FadeComplete"
			)
		);

	}

	private void FadeUpdate(float fade){
		preSceneObj.GetComponent<CanvasGroup>().alpha = fade;
		nextSceneObj.GetComponent<CanvasGroup>().alpha = 1.0f - fade;
	}

	private void FadeComplete(){
		preSceneObj.SetActive (false);
		preSceneObj.GetComponent<CanvasGroup>().alpha = 1.0f;
	}
	#endregion


	//----------------------------------
	//  CenterGameObject
	//----------------------------------
	public void Center( GameObject obj, Camera cam ){
		obj.transform.position = new Vector3(
			cam.transform.position.x, 
			cam.transform.position.y, 
			0
		);
	}

}

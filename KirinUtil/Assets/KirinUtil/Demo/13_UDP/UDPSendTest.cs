using KirinUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UDPSendTest : MonoBehaviour {

    public UDPSendManager uDPSenderManager;
    public string ip;
    public int sendPort;
    public InputField input;

	// Use this for initialization
	void Start () {
        uDPSenderManager.Init(ip, sendPort);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Space)) {
            uDPSenderManager.UDPSend(input.text);
        }
	}
}

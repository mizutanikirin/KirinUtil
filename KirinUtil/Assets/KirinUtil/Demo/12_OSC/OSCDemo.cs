using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class OSCDemo : MonoBehaviour
    {

        [SerializeField] private string ip;
        [SerializeField] private int receivePort;
        [SerializeField] private int sendPort;
        int count = 0;

        // Start is called before the first frame update
        void Start()
        {
            Util.net.OSCStart(ip, sendPort, receivePort);
        }

        // Update is called once per frame
        void Update()
        {
            Util.net.OSCSend("/test", count.ToString());
            count++;
        }

        // InspectorのKRNNetworkのイベントから呼び出される
        public void OSCReceived(List<NetManager.OSCData> oscData)
        {
            for (int i = 0; i < oscData.Count; i++)
            {
                print("OSCReceived: " + oscData[i].Key + "  " + oscData[i].Address + "  " + oscData[i].Data);
            }
        }
    }
}
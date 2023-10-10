using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {

    public class UDPReceiveManager : MonoBehaviour {

        private UdpClient client;

        public int port;
        private bool isRun;

        private string receiveMessage = "";
        private bool received = false;
        private Thread receiveThread;

        [Serializable]
        public class UDPReceivedEvent : UnityEngine.Events.UnityEvent<string> { }

        [SerializeField]
        private UDPReceivedEvent uDPReceivedEvent = new UDPReceivedEvent();

        //----------------------------------
        //  init
        //----------------------------------
        void OnEnable() {
            uDPReceivedEvent.AddListener(Received);
        }
        void Received(string message) {
            print("Received message: " + message);
        }

        // Use this for initialization
        void Start() {
            UDPStart();
        }

        public void UDPStart() {
            print("UDPReceiveManager UDPStart : " + port);

            receiveMessage = "";
            isRun = true;
            received = false;
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        //----------------------------------
        //  Receive
        //----------------------------------
        // Update is called once per frame
        void Update() {
            if (isRun) {
                if (received) {
                    uDPReceivedEvent.Invoke(receiveMessage);
                }
                received = false;
            }
        }

        private void ReceiveData() {

            if (client == null) client = new UdpClient(port);
            while (true) {

                if (!isRun) break;

                try {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

                    byte[] data = client.Receive(ref anyIP);
                    string text = Encoding.UTF8.GetString(data);

                    //receiveMessage = text;
                    SetMessage(text);
                    print("UDP Received: " + text);

                } catch (SocketException err) {
                    CloseUDP();
                    print(err.ToString());
                }

            }
        }

        private void SetMessage(string message) {
            receiveMessage = message;
            received = true;
        }


        //----------------------------------
        //  Exit
        //----------------------------------
        void OnDisable() {
            uDPReceivedEvent.RemoveListener(Received);
        }

        void OnApplicationQuit() {
            print("OnApplicationQuit");
            CloseUDP();
        }

        public void CloseUDP() {
            receiveMessage = "";
            client.Close();
            isRun = false;
        }
    }
}
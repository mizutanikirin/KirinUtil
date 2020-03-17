using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil {
    public class UDPSendManager : MonoBehaviour {

        private IPEndPoint remoteEndPoint;
        private UdpClient client;
        private string ip;
        private int port;

        //----------------------------------
        //  init
        //----------------------------------
        public void Init(string ipAddress, int portNum) {
            print("UDPSender init");
            ip = ipAddress;
            port = portNum;

            if (client == null) client = new UdpClient();
            else CloseUDP();

            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }


        //----------------------------------
        //  send
        //----------------------------------
        public void UDPSend(string message) {
            try {

                byte[] data = Encoding.UTF8.GetBytes(message);

                client.Send(data, data.Length, remoteEndPoint);
                //testText.text = "sended: " + message + " " + data.Length + "\n\n" + ip + " " + port;
                print("sended: " + message + " " + data.Length + "\n" + ip + " " + port);


            } catch (Exception err) {
                Console.WriteLine(err.ToString());
            }
        }


        //----------------------------------
        //  destroy
        //----------------------------------
        void OnApplicationQuit() {
            CloseUDP();
        }

        private void CloseUDP() {
            if (client != null) client.Close();
        }

    }
}
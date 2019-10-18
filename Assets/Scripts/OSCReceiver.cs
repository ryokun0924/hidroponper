using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;
using UniRx;
using System;

// namespace UniRx
// {
    public class OSCReceiver : MonoBehaviour
    {
        // Start is called before the first frame update
        public int port = 5555;
        private OSCServer server;
        private Subject<float> showSubject = new Subject<float>();

        public IObservable<float> OnShowSignal
        {
            get { return showSubject; }
        }

        void Start()
        {
            OSCHandler.Instance.Init();
            server = OSCHandler.Instance.CreateServer("target", port);
            server.ReceiveBufferSize = 1024;
            server.SleepMilliseconds = 10;
        }

        // Update is called once per frame
        void Update()
        {

            for (var i = 0; i < OSCHandler.Instance.packets.Count; i++)
            {
                receivedOSC(OSCHandler.Instance.packets[i]);
                OSCHandler.Instance.packets.Remove(OSCHandler.Instance.packets[i]);
                i--;
            }
        }

        private void receivedOSC(OSCPacket packet)
        {
            if (packet == null) return;

            int serverPort = packet.server.ServerPort;

            string address = packet.Address.Substring(1);

            int eventNumber = int.Parse(packet.Data[0].ToString());
            float duration = float.Parse(packet.Data[1].ToString());
            Debug.Log("Receive Signal. " + "[Event: " + eventNumber + ",Duration: " + duration + "]");
            showSubject.OnNext(duration);

        }
    }

// }
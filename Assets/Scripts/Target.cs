using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

namespace GodTouches
{
    // namespace UniRX
    // {
        public class Target : MonoBehaviour
        {
            OSCReciever m_receiver;
            public int port = 5555;
            // Start is called before the first frame update
            void Start()
            {
                m_receiver = new OSCReciever();
                m_receiver.Open(port);
            }

            // Update is called once per frame
            void Update()
            {
                var phase = GodTouch.GetPhase();
                if (phase == GodPhase.Began)
                {
                    print("touch start");
                }

                if( m_receiver.hasWaitingMessages()){
                    OSCMessage msg = m_receiver.getNextMessage();
                    Debug.Log(string.Format("message received: {0} {1}", msg.Address, msg.Data));
                }

            }
        // }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class DummyOSCSender : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {   
        OSCHandler.Instance.Init();

    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown("space")){
            List<object> values = new List<object>();
            // (type, duration)
            values.AddRange(new object[]{1,3.0});
            OSCHandler.Instance.SendMessage("/detect", values);
        }
    }
}

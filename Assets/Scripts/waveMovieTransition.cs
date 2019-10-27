using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;

public class waveMovieTransition : MonoBehaviour
{
    // Start is called before the first frame update
    float alpha;
    public int waveVideoFadeMilliSeconds;
    float pastTime;
    void Start()
    {
        pastTime = 0.0f;
        alpha = 0.0f;
         GetComponent<VideoPlayer>().targetCameraAlpha = alpha;
        // GetComponent<VideoPlayer>().targetCameraAlpha.DOFade( 1.0f, 2.0f);
        // GetComponent<VideoPlayer>().targetCameraAlpha= 0.5;
        // transform.transform.position =  new Vector3(-2.0f , 0.0f, 1.5f );
        // this.transform.DOMove(endValue: new Vector3(0.0f, 0.0f, 1.5f), duration: 2.0f);
       
    }

    // Update is called once per frame
    void Update()
    {
        if( alpha <= 1.0 ){
            pastTime += Time.deltaTime;
            alpha = pastTime*1000.0f / (float)waveVideoFadeMilliSeconds;
            GetComponent<VideoPlayer>().targetCameraAlpha = alpha;
        }
        
    }
}

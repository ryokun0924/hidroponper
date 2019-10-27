﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using DG.Tweening;


public class ScoreController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI rankText;

    [SerializeField] float resultShowDurationMilliSeconds = 4000;

     private OSCReceiver receiver = OSCReceiver.Instance;
    int dispScore;
    int dummyResult = 86;

    float pastTime;
    void Start()
    {
        scoreText.alpha = 0.0f;
        scoreText.DOFade(1.0f, 5.0f);

        pastTime = 0.0f;
        dispScore = 0;
        rankText.alpha = 0.0f;

        receiver.OnScoreShowSignal.Subscribe(score =>
        {
            showScore(score);
        });
        receiver.OnRankShowSignal.Subscribe(rank =>
        {
            showRank(rank);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
    void showScore(int _score)
    {   
        scoreText.text = "000";
        StartCoroutine(scoreFade());

    }
    IEnumerator scoreFade()
    {

        for(float t = 0; t <= (float)resultShowDurationMilliSeconds / 1000.0f; t += Time.deltaTime){
            dispScore = (int)Mathf.Lerp(0.0f, dummyResult, t * 1000f / resultShowDurationMilliSeconds);
            if (dispScore < 10)
            {
                scoreText.text = "00" + dispScore.ToString();
            }
            else if (dispScore < 100)
            {
                scoreText.text = "0" + dispScore.ToString();
            }
            else
            {
                scoreText.text = dispScore.ToString();
            }
            yield return null;
        }
    }

    void showRank(int _rank)
    {   
        rankText.alpha = 0.0f;
        rankText.DOFade(1.0f, resultShowDurationMilliSeconds/1000.0f);
    }
}
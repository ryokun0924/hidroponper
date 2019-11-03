using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using DG.Tweening;
using System;


public class ScoreController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI rankText;

    [SerializeField] float resultShowDurationMilliSeconds;

     private OSCController oscController;
    int dispScore;
    
     private IDisposable _disposableScore;
      private IDisposable _disposableRank;

    float pastTime;
    void Start()
    {
        oscController = OSCController.Instance;

        scoreText.alpha = 0.0f;
        scoreText.DOFade(1.0f, 5.0f);

        pastTime = 0.0f;
        dispScore = 0;
        rankText.alpha = 0.0f;

        _disposableScore = oscController.OnScoreShowSignal.Subscribe(score =>
        {
            showScore(score);
        });
        _disposableRank = oscController.OnRankShowSignal.Subscribe(rank =>
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
        StartCoroutine(scoreFade(_score));

    }
    IEnumerator scoreFade(int _score)
    {

        for(float t = 0; t <= (float)resultShowDurationMilliSeconds / 1000.0f; t += Time.deltaTime){
            dispScore = (int)Mathf.Lerp(0.0f, _score+1, t * 1000f / resultShowDurationMilliSeconds);
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

    void OnDestroy()
    {
        _disposableScore.Dispose();
        _disposableRank.Dispose();
    }
}

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
    [SerializeField] private GameObject scoreObject;

    [SerializeField] private GameObject rankObject;

    [SerializeField] private GameObject shirouto;
    [SerializeField] private GameObject ace;
    [SerializeField] private GameObject master;
    [SerializeField] private GameObject legend;

    [SerializeField] float resultShowDurationMilliSeconds;

    private OSCController oscController;
    int dispScore;

    private IDisposable _disposableScore;
    private IDisposable _disposableRank;

    private TextMeshPro scoreText;

    float pastTime;
    void Start()
    {
        oscController = OSCController.Instance;
        scoreText = scoreObject.GetComponent<TextMeshPro>();

        scoreText.alpha = 0.0f;
        scoreText.DOFade(1.0f, 5.0f);

        pastTime = 0.0f;
        dispScore = 0;


        _disposableScore = oscController.OnScoreShowSignal.Subscribe(score =>
        {
            showScore(score);
        });
        _disposableRank = oscController.OnRankShowSignal.Subscribe(rank =>
        {
            showRank(rank);
        });


        foreach (Transform child in rankObject.transform)
        {
            child.gameObject.SetActive(false);
            master.transform.DOScale(new Vector3(0f, 0.5f, 0.5f), 0f);
            shirouto.transform.DOScale(new Vector3(0f, 0.5f, 0.5f), 0f);
            legend.transform.DOScale(new Vector3(0f, 0.5f, 0.5f), 0f);
            ace.transform.DOScale(new Vector3(0f, 0.5f, 0.5f), 0f);
        }


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

        for (float t = 0; t <= (float)resultShowDurationMilliSeconds / 1000.0f; t += Time.deltaTime)
        {
            dispScore = (int)Mathf.Lerp(0.0f, _score, t * 1000f / resultShowDurationMilliSeconds);
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
        scoreText.text = _score.ToString();

    }

    void showRank(int _rank)
    {
        
        if (_rank == 0)
        {
            shirouto.SetActive(true);
            shirouto.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2f);
        }
        else if (_rank == 1)
        {
            ace.SetActive(true);
            ace.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2f);
        }
        else if (_rank == 2)
        {
            master.SetActive(true);
            master.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2f);
        }
        else if (_rank == 3)
        {
            legend.SetActive(true);
            legend.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2f);
        }
     
    }

    void OnDestroy()
    {
        _disposableScore.Dispose();
        _disposableRank.Dispose();
    }
}

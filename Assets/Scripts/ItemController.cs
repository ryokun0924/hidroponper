using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using GodTouches;
using TMPro;
using System;

public class ItemController : MonoBehaviour
{
    private OSCController oscController;

    [SerializeField] private GameObject item1;
    [SerializeField] private GameObject item2;

    [SerializeField] private GameObject getText;

    [SerializeField] private int fadeTime;

    [SerializeField] private float getTextShowSeconds;
    // Start is called before the first frame update

    // private Subject<int> itemHitSubject = new Subject<int>();

    // public IObservable<int> onItemHit
    // {
    //     get { return itemHitSubject; }
    // }

    private IDisposable _disposable;


    private bool isActive;
    private int activeKind;
    void Start()
    {

        isActive = false;
        oscController = OSCController.Instance;
        item1.SetActive(false);
        item2.SetActive(false);
        getText.transform.DOScale(new Vector3(0, 1, 1), 0f);
        _disposable = oscController.OnItemShowSignal.Subscribe(signals =>
        {

            item1.SetActive(false);
            item2.SetActive(false);
            getText.transform.DOScale(new Vector3(0, 1, 1), 0f);
            resetCoroutine();
            activate(signals);
        });

    }

    // Update is called once per frame
    void Update()
    {

        var phase = GodTouch.GetPhase();
        if (phase == GodPhase.Began && isActive)
        {

            // itemHitSubject.OnNext(activeKind);
            oscController.sendHitItem(activeKind);
            // StartCoroutine(hitAnimation());
            hitAnimation();
            StartCoroutine(showGetText());
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("クリックアップデート");
            startShow(item1, 2000);
        }


    }

    void activate(Vector2 signals)
    {
        int kind = (int)signals.x;
        int duration = (int)signals.y;

        if (kind == 1)
        {
            activeKind = 1;
            startShow(item1, duration);
        }
        else if (kind == 2)
        {
            activeKind = 2;
            startShow(item2, duration);
        }
    }

    void startShow(GameObject target, float _duration)
    {

        isActive = true;
        target.SetActive(true);
        Image itemImage = target.GetComponent<Image>();
        itemImage.transform.DOScale(new Vector3(1.7f, 1.7f, 1.7f), 0.0f);

        itemImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        DOTween.ToAlpha(
            () => itemImage.color,
            color => itemImage.color = color,
            1f,
            (float)fadeTime / 1000f
        );
        StartCoroutine(hide(target, _duration));
    }

    private IEnumerator hide(GameObject target, float duration)
    {

        yield return new WaitForSeconds((float)duration / 1000.0f);
        Image itemImage = target.GetComponent<Image>();
        DOTween.ToAlpha(
            () => itemImage.color,
            color => itemImage.color = color,
            0f,
            (float)fadeTime / 1000f
        );
        StartCoroutine(deactivate(target));
    }
    private IEnumerator deactivate(GameObject target)
    {
        yield return new WaitForSeconds((float)fadeTime / 1000.0f);
        target.SetActive(false);
        isActive = false;
    }

    private void hitAnimation()
    {


        Image itemImage = item1.GetComponent<Image>();
        if (activeKind == 1)
        {
            itemImage = item1.GetComponent<Image>();
        }
        else if (activeKind == 2)
        {
            itemImage = item2.GetComponent<Image>();
        }

        DOTween.ToAlpha(
      () => itemImage.color,
      color => itemImage.color = color,
      0f,
      (float)fadeTime / 1000f);

        itemImage.transform.DOScale(new Vector3(3.5f, 3.5f, 3.5f), (float)fadeTime / 1000f);

    }
    IEnumerator showGetText()
    {
        yield return new WaitForSeconds(0.3f);

        getText.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        getText.transform.DOScale(new Vector3(2, 2, 2), 0.8f);


        yield return new WaitForSeconds(getTextShowSeconds);
        getText.transform.DOScale(new Vector3(0, 2, 2), 0.2f);

    }

    void resetCoroutine()
    {
        StopCoroutine("showGetText");
        StopCoroutine("hitAnimation");
        StopCoroutine("deactivate");
        StopCoroutine("hide");
    }
    void OnDestroy()
    {
        _disposable.Dispose();
    }

}

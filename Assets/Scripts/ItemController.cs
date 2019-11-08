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




    [SerializeField] private GameObject powerUp;

    [SerializeField] private int fadeTime;

    [SerializeField] private float getTextShowSeconds;
    public AudioClip itemgetSE;
    AudioSource audioSource;
    // Start is called before the first frame update

    // private Subject<int> itemHitSubject = new Subject<int>();

    // public IObservable<int> onItemHit
    // {
    //     get { return itemHitSubject; }
    // }

    private IDisposable _disposable;


    private bool isActive;
    private int activeKind;
    GameObject itemget;
    void Start()
    {

        isActive = false;
        oscController = OSCController.Instance;
        item1.SetActive(false);


        powerUp.transform.DOScale(new Vector3(0, 1, 1), 0f);
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = itemgetSE;
        


        itemget = (GameObject)Resources.Load("effects/itemget");
        // Cubeプレハブを元に、インスタンスを生成、


        _disposable = oscController.OnItemShowSignal.Subscribe(signals =>
        {

            item1.SetActive(false);

         
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

        if (Input.GetKey("a"))
        {
            Debug.Log("クリックアップデート");
            startShow(item1, 2000);
        }


    }

    void activate(Vector2 signals)
    {
        int kind = (int)signals.x;
        int duration = (int)signals.y;
        powerUp.transform.DOScale(new Vector3(0, 1, 1), 0f);

        if (kind == 1)
        {
            activeKind = 1;
            startShow(item1, duration);
        }

    }

    void startShow(GameObject target, float _duration)
    {
        Instantiate(itemget, new Vector3(0.0f, 0.0f, 0.1f), Quaternion.identity);
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
        audioSource.Play();

        powerUp.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        powerUp.transform.DOScale(new Vector3(1f, 1f, 1f), 0.8f);


        yield return new WaitForSeconds(getTextShowSeconds);
        powerUp.transform.DOScale(new Vector3(0f, 1f, 1f), 0.2f);

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

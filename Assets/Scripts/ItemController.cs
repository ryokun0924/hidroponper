using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using GodTouches;
using TMPro;

public class ItemController : MonoBehaviour
{
    private OSCReceiver receiver = OSCReceiver.Instance;

    [SerializeField] private GameObject item1;
    [SerializeField] private GameObject item2;

    [SerializeField] private GameObject getText;

    [SerializeField] private int fadeTime;

    [SerializeField] private float getTextShowSeconds;
    // Start is called before the first frame update

    private bool isActive = false;
    private int activeKind;
    void Start()
    {

        item1.SetActive(false);
        item2.SetActive(false);
         getText.transform.DOScale(new Vector3(0, 1, 1), 0f);
        receiver.OnItemShowSignal.Subscribe(signals =>
        {
            activate(signals);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        var phase = GodTouch.GetPhase();
        if (phase == GodPhase.Began && isActive)
        {
            // StartCoroutine(hitAnimation());
            hitAnimation();
            StartCoroutine(showGetText());
        }

    }

    void activate(Vector2 signals)
    {
        int kind = (int)signals.x;
        int duration = (int)signals.y;
        GameObject target;
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
        itemImage.transform.DOScale(new Vector3(1, 1, 1), 0.0f);

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

        itemImage.transform.DOScale(new Vector3(3, 3, 3), (float)fadeTime/1000f);

    }
    IEnumerator showGetText(){
        yield return new WaitForSeconds(0.3f);

        getText.transform.localScale = new Vector3(0.0f,1.0f,1.0f);
        getText.transform.DOScale(new Vector3(1, 1, 1), 0.8f);
        
        
        yield return new WaitForSeconds(getTextShowSeconds);
        getText.transform.DOScale(new Vector3(0, 1, 1), 0.2f);
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class ItemController : MonoBehaviour
{
    private OSCReceiver receiver = OSCReceiver.Instance;

    [SerializeField] private GameObject item1;
    [SerializeField] private GameObject item2;

    [SerializeField] private int fadeTime;
    // Start is called before the first frame update
    void Start()
    {

        item1.SetActive(false);
        item2.SetActive(false);
        receiver.OnItemShowSignal.Subscribe(signals =>
        {
            activate(signals);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void activate(Vector2 signals)
    {
        int kind = (int)signals.x;
        int duration = (int)signals.y;
        GameObject target;
        if (kind == 1)
        {
            startShow(item1, duration);
        }
        else if (kind == 2)
        {
            startShow(item2, duration);
        }
    }

    void startShow(GameObject target, float _duration)
    {
        target.SetActive(true);
        Image itemImage = target.GetComponent<Image>();
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
    }
}

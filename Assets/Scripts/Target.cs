using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace GodTouches
{
    namespace UniRx
    {
        public class Target : MonoBehaviour
        {
            bool isActive;
            bool isHit;
            public float showDuration;
            public float hitAnimationDuration;

            private Quaternion beforeRotation;
            private Quaternion activeRotation;
            private Quaternion afterRotation;

            private OSCReceiver receiver;
            // Start is called before the first frame update
            void Start()
            {
                receiver = OSCReceiver.Instance;
                beforeRotation = Quaternion.Euler(new Vector3(0, 0, 110));
                activeRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                Quaternion.Euler(new Vector3(0, 0, -110));
                 showDuration = 300;
                hitAnimationDuration = 300;
                transform.rotation = beforeRotation;
                receiver.OnShowSignal.Subscribe(duration =>
                {
                    // print("show start. duration=" + duration);
                    StopCoroutine("hitAnimation");
                    activate(duration);
                });


            }

            // Update is called once per frame
            void Update()
            {
      
                var phase = GodTouch.GetPhase();
                if (phase == GodPhase.Began && isActive)
                {
                    isHit = true;
                    StartCoroutine(hitAnimation());
                    StopCoroutine("deactivate");
                    StopCoroutine("Show");
                }

            }
            void activate(float _duration)
            {
                transform.rotation = beforeRotation;
                StartCoroutine(Show(showDuration));
                StartCoroutine(deactivate(_duration));
            }

            private IEnumerator Show(float _showDuration)
            {
                isActive = true;
                //hitしたら登場アニメーションは終わり
                if (!isHit)
                {
                    Quaternion startRotation = transform.rotation;
                    Quaternion endRotation = activeRotation;
                    for (float t = 0; t < _showDuration; t += Time.deltaTime * 1000.0f)
                    {
                        transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / _showDuration);
                        yield return null;
                    }
                    transform.rotation = endRotation;
                }

            }

            private IEnumerator Hide(float _showDuration)
            {
                //hitしたら終了アニメーションは終わり
                if (!isHit)
                {
                    Quaternion startRotation = transform.rotation;
                    Quaternion endRotation = afterRotation;
                    for (float t = 0; t < _showDuration; t += Time.deltaTime * 1000.0f)
                    {
                        transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / _showDuration);
                        yield return null;
                    }
                }

                isActive = false;
                isHit = false;
            }



            private IEnumerator deactivate(float waitTime)
            {
                yield return new WaitForSeconds((float)waitTime/1000.0f);
                StartCoroutine(Hide(showDuration));
            }

            private IEnumerator hitAnimation()
            {
                Quaternion startRotation = transform.rotation;
                Quaternion endRotation = startRotation * Quaternion.Euler(100, 0, 0);
                for (float t = 0; t < hitAnimationDuration; t += Time.deltaTime * 1000.0f)
                {
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / hitAnimationDuration);
                    yield return null;
                }
                transform.rotation = endRotation;
            }



        }
    }

}
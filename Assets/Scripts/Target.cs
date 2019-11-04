using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using GodTouches;
// namespace GodTouches
// {
    namespace UniRx
    {
        public class Target : MonoBehaviour
        {
     
            bool isActive;
            bool isHit;

            float pastTime;
            public float showDuration;
            public float hitAnimationDuration;

            private Quaternion beforeRotation;
            private Quaternion activeRotation;
            private Quaternion afterRotation;

            private OSCController oscController;
            private ModeController modeController;
            GameObject fiteffect1;
            GameObject fiteffect2;
            GameObject fiteffect3;
            GameObject fiteffect4;
        // Start is called before the first frame update

        // private Subject<int> targetHitSubject = new Subject<int>();


        // public IObservable<int> onTargetHit{
        //     get { return targetHitSubject;}
        // }

        private IDisposable _disposable;

            void Start()
            {

            fiteffect1 = (GameObject)Resources.Load("effects/hit5");
            fiteffect2 = (GameObject)Resources.Load("effects/hit");
            fiteffect3 = (GameObject)Resources.Load("effects/hit1");
            fiteffect4 = (GameObject)Resources.Load("effects/hit2");
            pastTime = 0;
                oscController = OSCController.Instance;
                modeController = ModeController.Instance;
                beforeRotation = Quaternion.Euler(new Vector3(0, 0, 110));
                activeRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                afterRotation = Quaternion.Euler(new Vector3(0, 0, -110));
                showDuration = 300;
                hitAnimationDuration = 300;
                transform.rotation = beforeRotation;
                _disposable = oscController.OnShowSignal.Subscribe(duration =>
                {
                    // print("show start. duration=" + duration);
                    pastTime = 0;
                    resetCoroutine();
                    activate(duration);
                });
                
                if(modeController.isAutoMode){
                    StartCoroutine(autoMode());
                }

            }

            // Update is called once per frame
            void Update()
            {
                pastTime += Time.deltaTime;

                var phase = GodTouch.GetPhase();
                if (phase == GodPhase.Began && isActive)
                {
                    int pastTimeInt = (int)(pastTime*1000f);

                    // targetHitSubject.OnNext(pastTimeInt);
                    if(!modeController.isAutoMode){
                        oscController.sendHitTarget(pastTimeInt);
                    }
                    isHit = true;
              

               
                    StartCoroutine(hitAnimation());
                    StopCoroutine("deactivate");
                    StopCoroutine("Show");
                }

            if (Input.GetKey(KeyCode.Space))
            {
        
                activate(2000.0f);
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
                yield return new WaitForSeconds((float)waitTime / 1000.0f);
                StartCoroutine(Hide(showDuration));
            }



            private IEnumerator hitAnimation()
            {


            Instantiate(fiteffect1, new Vector3(0.0f, 0.0f, -0.3f), Quaternion.identity);
            //Instantiate(fiteffect2, new Vector3(0.0f, 0.0f, -0.3f), Quaternion.identity);
            Instantiate(fiteffect3, new Vector3(0.0f, 0.0f, -0.3f), Quaternion.identity);
            Instantiate(fiteffect4, new Vector3(0.0f, 0.0f, -0.3f), Quaternion.identity);

            yield return new WaitForSeconds(0.3f);
           Quaternion startRotation = transform.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(130, 0, 0);
                for (float t = 0; t < hitAnimationDuration; t += Time.deltaTime * 1000.0f)
                {
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / hitAnimationDuration);
                    yield return null;
                }
                transform.rotation = endRotation;
            }



            void resetCoroutine()
            {
                StopCoroutine("hitAnimation");
                StopCoroutine("deactivate");
                StopCoroutine("Hide");
                StopCoroutine("Show");
                
            }

            void OnDestroy(){
                // Debug.Log("taget destroy");
                 _disposable.Dispose();   
            }

            private IEnumerator autoMode(){
                while(true){
                    int duration = UnityEngine.Random.Range(2000,5000);
                    activate(duration);
                    int waitTime =  UnityEngine.Random.Range(2000,5000);
                    yield return new WaitForSeconds((float)(duration+waitTime) / 1000.0f);
                }
            }

        }
    }


// }
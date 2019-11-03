using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CountDownController : MonoBehaviour
{
    [SerializeField]
    GameObject one;

    [SerializeField]
    GameObject two;

    [SerializeField]
    GameObject three;

    [SerializeField]
    GameObject go;

    // Start is called before the first frame update
    float pastTIme;
    int beforeTimeInt;

    void Start()
    {
        three.GetComponent<Renderer>().enabled = true;
        two.GetComponent<Renderer>().enabled = false;
        one.GetComponent<Renderer>().enabled = false;
        go.GetComponent<Renderer>().enabled = false;
        pastTIme = 0;
        beforeTimeInt = 3;
    }

    // Update is called once per frame
    void Update()
    {
        pastTIme += Time.deltaTime;
        float remainTime = 3.0f - pastTIme;
        int remainTimeInt = (int)Mathf.Ceil(remainTime);
        if (remainTimeInt != beforeTimeInt)
        {
            if (remainTimeInt == 3)
            {
                three.GetComponent<Renderer>().enabled = true;
                two.GetComponent<Renderer>().enabled = false;
                one.GetComponent<Renderer>().enabled = false;
                go.GetComponent<Renderer>().enabled = false;
            }
            else if (remainTimeInt == 2)
            {
                three.GetComponent<Renderer>().enabled = false;
                two.GetComponent<Renderer>().enabled = true;
                one.GetComponent<Renderer>().enabled = false;
                go.GetComponent<Renderer>().enabled = false;
            }
            else if (remainTimeInt == 1)
            {
                three.GetComponent<Renderer>().enabled = false;
                two.GetComponent<Renderer>().enabled = false;
                one.GetComponent<Renderer>().enabled = true;
                go.GetComponent<Renderer>().enabled = false;

            }
            else
            {
                three.GetComponent<Renderer>().enabled = false;
                two.GetComponent<Renderer>().enabled = false;
                one.GetComponent<Renderer>().enabled = false;
                go.GetComponent<Renderer>().enabled = true;
                SceneManager.LoadScene("main");
            }
        }

        beforeTimeInt = remainTimeInt;
    }
}

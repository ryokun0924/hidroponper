using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour
{
     public static　string ipOfMainPC = "192.168.43.50";
     public static bool isAutoMode = false;
     [SerializeField] InputField TargetInputField;
     [SerializeField] Button okButton;
     [SerializeField] Button autoButton;
    // Start is called before the first frame update
    void Start()
    {
        string ip = PlayerPrefs.GetString("ip", "192.168.43.50");
        TargetInputField.text = ip;
        okButton.onClick.AddListener (OnClickButton);
        autoButton.onClick.AddListener (StartAutoModeButton);
    }

    public void OnClickButton() {
        PlayerPrefs.SetString("ip", TargetInputField.text);
        ipOfMainPC = TargetInputField.text;
        SceneManager.LoadScene("title");
    }
    public void StartAutoModeButton() {
        isAutoMode = true;
        SceneManager.LoadScene("main");

    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}

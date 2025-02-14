using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    public Text Name1;
    public Text Name2;

    public GameObject For1;
    public GameObject For2;
    public Button For2Button;
    private void Start()
    {
        //For2Button.Select();
    }
    public void Login()
    {
        GameManager.instance.name1 = Name1.text.ToString();
        if (GameManager.instance.ForNumber == true)
        {
            
            GameManager.instance.name2 = Name2.text.ToString();
        }
    }

    public void ForNumChanger(bool FN)
    {
        if (FN)
        {
            GameManager.instance.ForNumber = true;
            For2.SetActive(true);
        }
        else
        {
            GameManager.instance.ForNumber = false;
            For2.SetActive(false);
        }
    }
}

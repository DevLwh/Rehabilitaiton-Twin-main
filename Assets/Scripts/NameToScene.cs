using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameToScene : MonoBehaviour
{

    public Text nameText;
    // Start is called before the first frame update
    void Start()
    {
        // �̸� �ѱ��
        if(GameManager.instance.ForNumber)
        {
            string p1 = GameManager.instance.name1;
            string p2 = GameManager.instance.name2;
            nameText.text = p1 + "�԰� " + p2 + "���� ���� ��õ ��Դϴ�.";
        }
        else
        {
            string playername = GameManager.instance.name1;
            nameText.text = playername + "���� ���� ��õ ��Դϴ�";
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameToResult : MonoBehaviour
{

    public Text nameText;
    public Text nameText2;
    // Start is called before the first frame update
    void Start()
    {
        string p1 = GameManager.instance.name1;
        string p2 = GameManager.instance.name2;

        nameText.text = p1 + "님의 운동 기록";

        // 이름 넘기기
        if (GameManager.instance.ForNumber)
        {
            
            nameText2.text = p2 + "님의 운동 기록";
        }
    }

}

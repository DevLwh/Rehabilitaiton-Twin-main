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
        // 이름 넘기기
        if(GameManager.instance.ForNumber)
        {
            string p1 = GameManager.instance.name1;
            string p2 = GameManager.instance.name2;
            nameText.text = p1 + "님과 " + p2 + "님을 위한 추천 운동입니다.";
        }
        else
        {
            string playername = GameManager.instance.name1;
            nameText.text = playername + "님을 위한 추천 운동입니다";
        }
    }

}

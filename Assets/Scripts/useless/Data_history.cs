using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]



public class Data_history : MonoBehaviour
{
    public DrawSkeleton_2 DrawSkeleton;
    private Vector2[] Skeleton2d = new Vector2[17];
    private Vector2[] Skeleton2d2 = new Vector2[17];

    float Angle_LArm = 0.0f;
    float Angle_RArm = 0.0f;
    float Angle_LLeg = 0.0f;
    float Angle_RLeg = 0.0f;

    float Angle_LArm2 = 0.0f;
    float Angle_RArm2 = 0.0f;
    float Angle_LLeg2 = 0.0f;
    float Angle_RLeg2 = 0.0f;
    public Animator Ani;
    public Text noticeTxt;


    void Update()
    {
        for (int i = 0; i < 17; i++)
        {
            Skeleton2d[i].x = DrawSkeleton.keypoints[i].transform.position.x;
            Skeleton2d[i].y = DrawSkeleton.keypoints[i].transform.position.y;
            Skeleton2d2[i].x = DrawSkeleton.keypoints2[i].transform.position.x;
            Skeleton2d2[i].y = DrawSkeleton.keypoints2[i].transform.position.y;
        }

        
        //Left_Arm
        Angle_LArm = Vector2.Angle(Skeleton2d[5] - Skeleton2d[7], Skeleton2d[5] - Skeleton2d[11]);
        //Right_Arm
        Angle_RArm = Vector2.Angle(Skeleton2d[6] - Skeleton2d[8], Skeleton2d[6] - Skeleton2d[12]);

        //Left_Leg
        Angle_LLeg = Vector2.Angle(Skeleton2d[11] - Skeleton2d[5], Skeleton2d[11] - Skeleton2d[13]);
        //Right_Leg
        Angle_RLeg = Vector2.Angle(Skeleton2d[12] - Skeleton2d[6], Skeleton2d[12] - Skeleton2d[14]);

        Angle_LArm2 = Vector2.Angle(Skeleton2d2[5] - Skeleton2d2[7], Skeleton2d2[5] - Skeleton2d2[11]);
        //Right_Arm
        Angle_RArm2 = Vector2.Angle(Skeleton2d2[6] - Skeleton2d2[8], Skeleton2d2[6] - Skeleton2d2[12]);

        //Left_Leg
        Angle_LLeg2 = Vector2.Angle(Skeleton2d2[11] - Skeleton2d2[5], Skeleton2d2[11] - Skeleton2d2[13]);
        //Right_Leg
        Angle_RLeg2 = Vector2.Angle(Skeleton2d2[12] - Skeleton2d2[6], Skeleton2d2[12] - Skeleton2d2[14]);

    }
    private void Start()
    {
        Ani.SetInteger("Angle", 0);
    }
    public void checkAngle()
    {
        StartCoroutine("Timer");
    }

    public Text timer;

    // LArm, RArm, LLeg,RLeg
    IEnumerator Timer()
    {
        int AngleCheck = 0;
        // 0 : 측면 앞, 1: 측면 뒤, 2: 정면 옆 
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                AngleCheck++;
                Debug.Log(AngleCheck);
                Ani.SetInteger("Angle", AngleCheck);
                
                switch (i)
                {
                    case 0:
                        noticeTxt.text = "측면을 바라보고 앞으로 뻗어주세요";
                        break;
                    case 1:
                        noticeTxt.text = "측면을 바라보고 뒤로 뻗어주세요";
                        break;
                    case 2:
                        noticeTxt.text = "정면을 바라보고 옆으로 뻗어주세요";
                        break;
                }
                timer.text = "3";
                yield return new WaitForSeconds(1.5f);
                
                timer.text = "2";
                yield return new WaitForSeconds(1.5f);
                timer.text = "1";
                yield return new WaitForSeconds(1.5f);
                Ani.SetInteger("Angle", 0);
                timer.text = "저장";
                
                yield return new WaitForSeconds(3.0f);
                noticeTxt.text = "";
                timer.text = "";
            }

        }
    }

}

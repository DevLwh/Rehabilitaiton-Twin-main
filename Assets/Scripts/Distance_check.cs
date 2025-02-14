 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]

class Data
{
    public string date;
    public List<float> Angle_RArm = new List<float>();
    public List<float> Angle_LArm = new List<float>();
    public List<float> Angle_RLeg = new List<float>();
    public List<float> Angle_LLeg = new List<float>();
}
class JsonData
{
    public List<Data> AngleData = new List<Data>();
}

public class Distance_check : MonoBehaviour
{
    public DrawSkeleton_2 DrawSkeleton;
    public static Distance_check DisCheck;
    private Vector2[] Skeleton2d = new Vector2[17];
    private Vector2[] Skeleton2d2 = new Vector2[17];
    public Text distance_ui;
    public Text distance_check_ui;
    public List<Text> LLegTextList = new List<Text>(3);
    public List<Text> RLegTextList = new List<Text>(3);
    public List<Text> LArmTextList = new List<Text>(3);
    public List<Text> RArmTextList = new List<Text>(3);

    public List<Text> LLegTextList1 = new List<Text>(3);
    public List<Text> RLegTextList1 = new List<Text>(3);
    public List<Text> LArmTextList1 = new List<Text>(3);
    public List<Text> RArmTextList1 = new List<Text>(3);

    public List<Text> LLegTextList2 = new List<Text>(3);
    public List<Text> RLegTextList2 = new List<Text>(3);
    public List<Text> LArmTextList2 = new List<Text>(3);
    public List<Text> RArmTextList2 = new List<Text>(3);

    float Angle_LArm = 0.0f;
    float Angle_RArm = 0.0f;
    float Angle_LLeg = 0.0f;
    float Angle_RLeg = 0.0f;

    float Angle_LArm2 = 0.0f;
    float Angle_RArm2 = 0.0f;
    float Angle_LLeg2 = 0.0f;
    float Angle_RLeg2 = 0.0f;

    float[] Angle_RArm_list = new float[3];
    float[] Angle_Lleg_list = new float[3];
    float[] Angle_Rleg_list = new float[3];
    float[] Angle_LArm_list = new float[3];

    float[] Angle_RArm_list1 = new float[3];
    float[] Angle_Lleg_list1 = new float[3];
    float[] Angle_Rleg_list1 = new float[3];
    float[] Angle_LArm_list1 = new float[3];

    float[] Angle_RArm_list2 = new float[3];
    float[] Angle_Lleg_list2 = new float[3];
    float[] Angle_Rleg_list2 = new float[3];
    float[] Angle_LArm_list2 = new float[3];

    public Text noticeTxt;
    public Text timer;


    public enum state
    {
        Larm, Rarm, Lleg, Rleg
    };
    private void Awake()
    {
        DisCheck = this;
    }

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
        //distance_check();
        //Check_text();
    }

    public void distance_check()
    {
        float DistanceTop = Vector2.Distance(Skeleton2d[5], Skeleton2d[6]);
        float DistanceLeft = Vector2.Distance(Skeleton2d[5], Skeleton2d[11]);
        float DistanceRight = Vector2.Distance(Skeleton2d[6], Skeleton2d[12]);
        if (DistanceTop < 200 && DistanceLeft < 200 && DistanceRight < 200)
        {
            distance_ui.text = "앞으로 오세요";
        }
        /*else if (Distance > 300)
        {
            distance_ui.text = "뒤로 가세요";
        }*/
        else
        {
            distance_ui.text = null;
        }
        //Debug.Log(Distance);
        Debug.Log(DistanceTop);

    }
    public void checkAngle(string states) 
    {
        StartCoroutine("Timer",states);
        

    }

    IEnumerator Timer(string states)
    {
        Debug.Log("hi");
        // 0 : 측면 앞, 1: 측면 뒤, 2: 정면 옆 
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("hi");
            switch (i)
            {
                case 0:
                    Debug.Log("hi");
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
            timer.text = "저장";

            if (GameManager.instance.ForNumber == false)
            {
                switch (states)
                {
                    case "Larm":
                        Angle_LArm_list[i] = Angle_LArm;
                        LArmTextList[i].text = Angle_LArm.ToString("F1");
                        break;
                    case "Rarm":
                        Angle_RArm_list[i] = Angle_RArm;
                        RArmTextList[i].text = Angle_RArm.ToString("F1");
                        break;
                    case "Lleg":
                        Angle_Lleg_list[i] = Angle_LLeg;
                        LLegTextList[i].text = Angle_LLeg.ToString("F1");
                        break;
                    case "Rleg":
                        Angle_Rleg_list[i] = Angle_RLeg;
                        RLegTextList[i].text = Angle_RLeg.ToString("F1");
                        break;

                }
                yield return new WaitForSeconds(1.5f);
                noticeTxt.text = "";
                timer.text = "";
            }
            else
            {
                switch (states)
                {
                    case "Larm":
                        Angle_LArm_list1[i] = Angle_LArm;
                        Angle_LArm_list2[i] = Angle_LArm2;
                        LArmTextList1[i].text = Angle_LArm.ToString("F1");
                        LArmTextList2[i].text = Angle_LArm2.ToString("F1");
                        break;
                    case "Rarm":
                        Angle_RArm_list1[i] = Angle_RArm;
                        Angle_RArm_list2[i] = Angle_RArm2;
                        RArmTextList1[i].text = Angle_RArm.ToString("F1");
                        RArmTextList2[i].text = Angle_RArm2.ToString("F1");
                        break;
                    case "Lleg":
                        Angle_Lleg_list1[i] = Angle_LLeg;
                        Angle_Lleg_list2[i] = Angle_LLeg2;
                        LLegTextList1[i].text = Angle_LLeg.ToString("F1");
                        LLegTextList2[i].text = Angle_LLeg2.ToString("F1");
                        break;
                    case "Rleg":
                        Angle_Rleg_list1[i] = Angle_RLeg;
                        Angle_Rleg_list2[i] = Angle_RLeg2;
                        RLegTextList1[i].text = Angle_RLeg.ToString("F1");
                        RLegTextList2[i].text = Angle_RLeg2.ToString("F1");
                        break;

                }
                yield return new WaitForSeconds(1.5f);
                noticeTxt.text = "";
                timer.text = "";
            }
                

        }

    }
    private void Check_text()
    {
        if(!check_bone())
        {
            distance_check_ui.text = "팔다리가 다 나오도록 카메라 정면에 위치해 주세요.";
        }
        else
        {
            distance_check_ui.text = "";
        }

    }
    private bool check_bone()
    {
        foreach(GameObject A in DrawSkeleton.keypoints)
        {
            if(!A.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    
    public void SaveData()
    {
        JsonData jsondata = new JsonData();
        Data date = new Data();
        string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
        date.date = nowdate;
        foreach(float A in Angle_LArm_list)
        {
            date.Angle_LArm.Add(A);
        }
        foreach (float A in Angle_RArm_list)
        {
            date.Angle_RArm.Add(A);
        }
        foreach (float A in Angle_Lleg_list)
        {
            date.Angle_LLeg.Add(A);
        }
        foreach (float A in Angle_Rleg_list)
        {
            date.Angle_RLeg.Add(A);
        }
        string a = File.ReadAllText(Application.dataPath + "/jointAngle.json");
        jsondata = JsonUtility.FromJson<JsonData>(a);
        for(int i = 0; i < jsondata.AngleData.Count; i++)
        {
            if (jsondata.AngleData[i].date == nowdate)
            {
                jsondata.AngleData.RemoveAt(i);
            }
                
        }
        jsondata.AngleData.Add(date);
        string json = JsonUtility.ToJson(jsondata);

        File.WriteAllText(Application.dataPath + "/jointAngle.json", json);
        Debug.Log(Application.dataPath);
    }

}

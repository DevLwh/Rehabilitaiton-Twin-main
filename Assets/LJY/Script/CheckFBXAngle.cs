using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFBXAngle : MonoBehaviour
{
    public DrawSkeleton_2 DrawSkeleton;
    public List<Transform> ThreeDPose;
    public List<Transform> ThreeDPose2;
    private Vector2[] Skeleton2d = new Vector2[17];
    private Vector2[] Skeleton2d2 = new Vector2[17];
    public Text UIText;
    public Text UIText2;
    public RawImage AvatarImage;
    public Animator ani;
    float deviation = 0;
    float deviation2 = 0;
    Vector3 LLegAngle1;
    Vector3 LLegAngle2;
    float Angle_LLeg;
    float Angle_LLeg2;
    float Angle1 = 180;
    bool isOne;
    int excercise_case = 0;
    // Start is called before the first frame update
    void Start()
    {
        isOne = GameManager.instance.ForNumber;
        StartCoroutine(StartExercise(excercise_case));
        if (!GameManager.instance.ForNumber)
        {
            UIText2.gameObject.SetActive(false);
            UIText.rectTransform.localPosition = new Vector3(400,
                UIText.rectTransform.localPosition.y, 0);
            AvatarImage.rectTransform.localPosition = new Vector3(400,
                AvatarImage.rectTransform.localPosition.y, 0);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if(isOne)
        {
            //Squat2();
            //TwoCheck();
        }
        else
        {
            //Squat();
            //OneCheck();
        }
        
    }


    #region Squat Code
    float MinSquat = 180.0f;
    float MinSquat2 = 180.0f;
    int MaxSquat = 0;
    bool Checking = false;
    /*
    void Squat()
    {
        if (MaxSquat == 9)
            ani.SetBool("Start", false);
        if (Angle1 < 80.0f)
        {
            if (Angle_LLeg < MinSquat)
                MinSquat = Angle_LLeg;
            UIText.text = "";
            Checking = true;
        }
        if(Angle1 > 80.0f)
        {
            if(Checking)
            {
                Debug.Log(MinSquat);
                deviation += 80 - MinSquat;
                if(MinSquat <= 80.0f)
                {
                    UIText.color = Color.green;
                    UIText.text = "Perfect";
                    Debug.Log("Perfect");
                }
                else if(MinSquat <= 100.0f&& MinSquat > 80.0f)
                {
                    UIText.color = Color.blue;
                    UIText.text = "Good";
                    Debug.Log("Good");
                }
                else if(MinSquat > 100.0f)
                {
                    UIText.color = Color.red;
                    UIText.text = "Bad";
                    Debug.Log("Bad");
                }
                MinSquat = 180.0f;
                MaxSquat += 1;
                Checking = false;
            }
        }

    }
    */

    void Squat2()
    {
        if (MaxSquat == 9)
            ani.SetBool("Start", false);
        if (Angle1 < 80.0f)
        {
            if (Angle_LLeg < MinSquat)
                MinSquat = Angle_LLeg;
            if (Angle_LLeg2 < MinSquat2)
                MinSquat2 = Angle_LLeg2;
            UIText.text = "";
            UIText2.text = "";
            Checking = true;
        }
        if (Angle1 > 80.0f)
        {
            if (Checking)
            {
                deviation += 60 - MinSquat;
                deviation2 += 60 - MinSquat2;
                //1st People
                if (MinSquat <= 60.0f)
                {
                    UIText.color = Color.green;
                    UIText.text = "Perfect";
                }
                else if (MinSquat <= 80.0f && MinSquat > 60.0f)
                {
                    UIText.color = Color.blue;
                    UIText.text = "Good";
                }
                else if (MinSquat > 80.0f)
                {
                    UIText.color = Color.red;
                    UIText.text = "Bad";
                }
                //2nd People
                if (MinSquat2 <= 60.0f)
                {
                    UIText2.color = Color.green;
                    UIText2.text = "Perfect";
                }
                else if (MinSquat2 <= 80.0f && MinSquat2 > 60.0f)
                {
                    UIText2.color = Color.blue;
                    UIText2.text = "Good";
                }
                else if (MinSquat2 > 80.0f)
                {
                    UIText2.color = Color.red;
                    UIText2.text = "Bad";
                }

                //초기화
                MinSquat = 180.0f;
                MinSquat2 = 180.0f;
                MaxSquat += 1;
                Checking = false;
            }
        }

    }
    #endregion

    #region CheckingCode
    void OneCheck()
    {
        //2D CheckVersion
        /*for (int i = 0; i < 17; i++)
        {
            Skeleton2d[i].x = DrawSkeleton.keypoints[i].transform.position.x;
            Skeleton2d[i].y = DrawSkeleton.keypoints[i].transform.position.y;
        }*/
        //Angle_LLeg = Vector2.Angle(Skeleton2d[11] - Skeleton2d[5], Skeleton2d[11] - Skeleton2d[13]);

        Angle_LLeg = Vector3.Angle(ThreeDPose[2].position - ThreeDPose[1].position,
            ThreeDPose[2].position - ThreeDPose[3].position);
        LLegAngle1 = (ani.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position -
            ani.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position);
        LLegAngle2 = (ani.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position -
            ani.GetBoneTransform(HumanBodyBones.LeftFoot).position);
        Angle1 = Vector3.Angle(LLegAngle1, LLegAngle2);
    }
    void TwoCheck()
    {
        //2D CheckVersion
        /*for (int i = 0; i < 17; i++)
        {
            Skeleton2d[i].x = DrawSkeleton.keypoints[i].transform.position.x;
            Skeleton2d[i].y = DrawSkeleton.keypoints[i].transform.position.y;
            Skeleton2d2[i].x = DrawSkeleton.keypoints2[i].transform.position.x;
            Skeleton2d2[i].y = DrawSkeleton.keypoints2[i].transform.position.y;
        }*/
        //Angle_LLeg = Vector2.Angle(Skeleton2d[11] - Skeleton2d[5], Skeleton2d[11] - Skeleton2d[13]);


        Angle_LLeg = Vector3.Angle(ThreeDPose[2].position - ThreeDPose[1].position,
            ThreeDPose[2].position - ThreeDPose[3].position);
        Angle_LLeg2 = Vector3.Angle(ThreeDPose2[2].position - ThreeDPose2[1].position,
            ThreeDPose2[2].position - ThreeDPose2[3].position);
        LLegAngle1 = (ani.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position -
            ani.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position);
        LLegAngle2 = (ani.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position -
            ani.GetBoneTransform(HumanBodyBones.LeftFoot).position);
        Angle1 = Vector3.Angle(LLegAngle1, LLegAngle2);
    }
    #endregion
    IEnumerator StartExercise(int i)
    {
        UIText.text = "운동을 시작합니다.";
        UIText2.text = "운동을 시작합니다.";
        
        for(int k=0; k< 8; k++)
        {
            yield return new WaitForSeconds(1.5f);
            UIText.text = "3";
            UIText2.text = "3";
            yield return new WaitForSeconds(1.5f);
            UIText.text = "2";
            UIText2.text = "2";
            yield return new WaitForSeconds(1.5f);
            UIText.text = "1";
            UIText2.text = "1";
            yield return new WaitForSeconds(1.5f);
            UIText.text = "";
            UIText2.text = "";
            ani.SetInteger("case", i);
        }
        

    }
}

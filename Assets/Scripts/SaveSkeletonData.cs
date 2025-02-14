using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class SaveSkeletonData : MonoBehaviour
{
    public DrawSkeleton_2 DrawSkeleton;
    private Vector2[] Skeleton2d = new Vector2[17];
    private Vector2[] Skeleton2d_2 = new Vector2[17];
    private int frame = 0;
    private string filePath;
    public Animator character;
    private Vector3[] Mocopi3d = new Vector3[17];
    public RawImage AvatarImage;
    public Text UIText;
    public Text UIText2;
    public bool ismocopi = false;
    public Text PastTime;
    public Text PastTime2;

    public GameObject PastTimeBar;
    public GameObject PastTimeBar2;

    public Animator ani;
    public RuntimeAnimatorController ani_Squat;
    public RuntimeAnimatorController ani_External;
    public RuntimeAnimatorController ani_Stick_mobility;
    public RuntimeAnimatorController[] ani_Shoulder = new RuntimeAnimatorController[6] ;

    public Text count;
    public Text count2;
    public Text total_count;
    public Text total_count2;
    public Text announcement;

    private float LAngleKnee;
    private float RAngleKnee;

    private float ElbowDistance;
    private float WristDistance;

    private float LAngleElbow;
    private float RAngleElbow;
    private float LAngleWrist2Shoulder;
    private float RAngleWrist2Shoulder;

    public GameObject[] PGB_Text = new GameObject[3];
    public GameObject[] PGB_Text2 = new GameObject[3];

    public AudioClip[] PGB_Audio = new AudioClip[3];
    public AudioClip[] BGM = new AudioClip[2];

    private GameObject BackgroundMusic;
    private AudioSource backgroundMusic;

    private AudioSource Audio;
    private AudioSource Audio2;
    

    private void Start()
    {
        count.text = "0";
        if (GameManager.instance.ForNumber == true)
            count2.text = "0";

        // GameManager의 인스턴스에서 현재 운동을 확인
        // 해당 운동에 맞는 애니메이션 컨트롤러 실행
        switch (GameManager.instance.nowExercise)
        {
            case GameManager.ExerciseList.Squat:
                ani.runtimeAnimatorController = ani_Squat;
                break;

            case GameManager.ExerciseList.ExternalRotation:
                ani.runtimeAnimatorController = ani_External;
                break;

            case GameManager.ExerciseList.StickMobility:
                ani.runtimeAnimatorController = ani_Stick_mobility;
                break;

            case GameManager.ExerciseList.SideShoulderDeltoid:
                ani.runtimeAnimatorController = ani_Shoulder[0];
                total_count.text = "/ 2";
                if (GameManager.instance.ForNumber == true)
                    total_count2.text = "/ 2";
                break;

            case GameManager.ExerciseList.BackShoulderDeltoid:
                ani.runtimeAnimatorController = ani_Shoulder[1];
                total_count.text = "/ 2";
                if (GameManager.instance.ForNumber == true)
                    total_count2.text = "/ 2";
                break;

            case GameManager.ExerciseList.BottomShoulder:
                ani.runtimeAnimatorController = ani_Shoulder[2];
                total_count.text = "/ 2";
                if (GameManager.instance.ForNumber == true)
                    total_count2.text = "/ 2";
                break;

            case GameManager.ExerciseList.ShoulderRotation:
                ani.runtimeAnimatorController = ani_Shoulder[3];
                total_count.text = "/ 2";
                if (GameManager.instance.ForNumber == true)
                    total_count2.text = "/ 2";
                break;

            case GameManager.ExerciseList.FrontShoulder:
                PastTimeBar.GetComponent<CooldownUI>().maxCooldown = 15;
                if (GameManager.instance.ForNumber == true)
                {
                    PastTimeBar2.GetComponent<CooldownUI>().maxCooldown = 15;
                }
                ani.runtimeAnimatorController = ani_Shoulder[4];
                total_count.text = "/ 1";
                if (GameManager.instance.ForNumber == true)
                    total_count2.text = "/ 1";
                break;

            case GameManager.ExerciseList.SpineRotation:
                ani.runtimeAnimatorController = ani_Shoulder[5];
                total_count.text = "/ 2";
                if (GameManager.instance.ForNumber == true)
                    total_count2.text = "/ 2";
                break;
        }
        BackgroundMusic = GameObject.Find("AudioManager");
        backgroundMusic = BackgroundMusic.GetComponent<AudioSource>();
        //backgroundMusic.volume = .5f;
        backgroundMusic.clip = BGM[1];
        backgroundMusic.Play();
        Audio = gameObject.AddComponent<AudioSource>();
        Audio2 = gameObject.AddComponent<AudioSource>();


        //if (!GameManager.instance.ForNumber)
        //    AvatarImage.rectTransform.localPosition = new Vector3(-565, AvatarImage.rectTransform.localPosition.y, 0);

        StartCoroutine("Timer");
    }

    private void Update()
    {
        // GameManager의 운동종류에 따라서 운동 함수 실행
        if (GameManager.instance.is_save != true)
        {
            return;
        }
        switch (GameManager.instance.nowExercise)
        {
            case GameManager.ExerciseList.Squat:
                Squat();
                break;

            case GameManager.ExerciseList.ExternalRotation:
                ExternalRotation();
                break;

            case GameManager.ExerciseList.StickMobility:
                StickMobility();
                break;

            case GameManager.ExerciseList.SideShoulderDeltoid:
                SideShoulderDeltoid(Skeleton2d, ref GameManager.instance.SideShoulderDeltoid_PGBS,
                                    ref ssd_r_time1, ref ssd_r_check1, ref ssd_l_time1, ref ssd_l_check1, ref ssd_PGBS1, PGB_Text, count, ref play_time, ref Audio);
                if (GameManager.instance.ForNumber == true)
                    SideShoulderDeltoid(Skeleton2d_2, ref GameManager.instance.SideShoulderDeltoid2_PGBS,
                                        ref ssd_r_time2, ref ssd_r_check2, ref ssd_l_time2, ref ssd_l_check2, ref ssd_PGBS2, PGB_Text2, count2, ref play_time2, ref Audio2);
                break;

            case GameManager.ExerciseList.BackShoulderDeltoid:
                BackShoulderDeltoid(Skeleton2d, ref GameManager.instance.BackShoulderDeltoid_PGBS,
                                    ref bsd_r_time1, ref bsd_r_check1, ref bsd_l_time1, ref bsd_l_check1, ref bsd_PGBS1, PGB_Text, count, ref play_time, ref Audio);
                if (GameManager.instance.ForNumber == true)
                    BackShoulderDeltoid(Skeleton2d_2, ref GameManager.instance.BackShoulderDeltoid2_PGBS,
                                        ref bsd_r_time2, ref bsd_r_check2, ref bsd_l_time2, ref bsd_l_check2, ref bsd_PGBS2, PGB_Text2, count2, ref play_time2, ref Audio2);
                break;

            case GameManager.ExerciseList.BottomShoulder:
                BottomShoulder(Skeleton2d, ref GameManager.instance.BottomShoulder_PGBS,
                               ref bs_time_l1, ref bs_check_l1, ref bs_time_r1, ref bs_check_r1, ref bs_PGBS1, PGB_Text, count, ref play_time, ref Audio);
                if (GameManager.instance.ForNumber == true)
                    BottomShoulder(Skeleton2d_2, ref GameManager.instance.BottomShoulder2_PGBS,
                        ref bs_time_l2, ref bs_check_l2, ref bs_time_r2, ref bs_check_r2, ref bs_PGBS2, PGB_Text2, count2, ref play_time2, ref Audio2);
                break;

            case GameManager.ExerciseList.ShoulderRotation:
                ShoulderRotation(Skeleton2d, ref GameManager.instance.ShoulderRotation_PGBS,
                                 ref sr_PGBS1, ref sr_r_time1, ref sr_r_check1, ref sr_l_time1, ref sr_l_check1, PGB_Text, count, ref play_time, ref Audio);

                if (GameManager.instance.ForNumber == true)
                    ShoulderRotation(Skeleton2d_2, ref GameManager.instance.ShoulderRotation2_PGBS,
                                     ref sr_PGBS2, ref sr_r_time2, ref sr_r_check2, ref sr_l_time2, ref sr_l_check2, PGB_Text2, count2, ref play_time2, ref Audio2);
                break;

            case GameManager.ExerciseList.FrontShoulder:
               
                FrontShoulder(Skeleton2d, ref GameManager.instance.FrontShoulder_PGBS,
                                ref fs_time1, ref fs_PGBS1, ref fs_check1, PGB_Text, count, ref play_time, ref Audio);

                if (GameManager.instance.ForNumber == true)
                    FrontShoulder(Skeleton2d_2, ref GameManager.instance.FrontShoulder2_PGBS,
                                    ref fs_time2, ref fs_PGBS2, ref fs_check2, PGB_Text2, count2, ref play_time2, ref Audio2);
                break;

            case GameManager.ExerciseList.SpineRotation:
                SpineRotation(Skeleton2d, ref GameManager.instance.SpineRotation_PGBS,
                                    ref SpineR_r_time1, ref SpineR_r_check1, ref SpineR_l_time1, ref SpineR_l_check1, ref SpineR_PGBS1, PGB_Text, count, ref play_time, ref Audio);
                if (GameManager.instance.ForNumber == true)
                    SpineRotation(Skeleton2d_2, ref GameManager.instance.SpineRotation2_PGBS,
                                        ref SpineR_r_time2, ref SpineR_r_check2, ref SpineR_l_time2, ref SpineR_l_check2, ref SpineR_PGBS2, PGB_Text2, count2, ref play_time2, ref Audio2);
                break;


        }
    }
    // 화면이 종료되면 음악 종료
    private void OnDisable()
    {
        // Release the resources allocated for the inference engine
        backgroundMusic.clip = BGM[0];
        backgroundMusic.Play();
    }
    void FixedUpdate()
    {

        if (GameManager.instance.is_save)
        {
            frame += 1;

            // pose estimation position 정보(17개 관절 정보)
            for (int i = 0; i < 17; i++)
            {
                Skeleton2d[i].x = DrawSkeleton.keypoints[i].transform.position.x;
                Skeleton2d[i].y = DrawSkeleton.keypoints[i].transform.position.y;
            }
            LAngleKnee = Vector2.Angle(Skeleton2d[14] - Skeleton2d[12], Skeleton2d[14] - Skeleton2d[16]);
            RAngleKnee = Vector2.Angle(Skeleton2d[13] - Skeleton2d[11], Skeleton2d[13] - Skeleton2d[15]);
            ElbowDistance = Vector2.Distance(Skeleton2d[7], Skeleton2d[8]); // 팔꿈치와 팔꿈치 사이 거리
            WristDistance = Vector2.Distance(Skeleton2d[9], Skeleton2d[10]); // 손목과 손목 사이 거리

            LAngleElbow = Vector2.Angle(Skeleton2d[7] - Skeleton2d[5], Skeleton2d[7] - Skeleton2d[9]); //왼쪽 팔꿈치 각도
            RAngleElbow = Vector2.Angle(Skeleton2d[8] - Skeleton2d[6], Skeleton2d[8] - Skeleton2d[10]); //오른쪽 팔꿈치 각도

            LAngleWrist2Shoulder = Vector2.Angle(Skeleton2d[5] - Skeleton2d[9], Skeleton2d[5] - Skeleton2d[6]); // 왼쪽 손목 어깨 각도
            RAngleWrist2Shoulder = Vector2.Angle(Skeleton2d[6] - Skeleton2d[10], Skeleton2d[6] - Skeleton2d[5]); // 오른쪽 손목 어깨 각도

            // 2명일때 pose estimation position 정보(17개 관절 정보)
            if (GameManager.instance.ForNumber)
            {
                for (int i = 0; i < 17; i++)
                {

                    Skeleton2d_2[i].x = DrawSkeleton.keypoints2[i].transform.position.x;
                    Skeleton2d_2[i].y = DrawSkeleton.keypoints2[i].transform.position.y;
                }
            }
            // 모코피를 사용할 때 모코피 pose estimation position 정보(17개 관절 정보)
            if (ismocopi)
            {
                // Mocopi position 정보
                Mocopi3d[0].x = character.GetBoneTransform(HumanBodyBones.Head).position.x;
                Mocopi3d[0].y = character.GetBoneTransform(HumanBodyBones.Head).position.y;
                Mocopi3d[0].z = character.GetBoneTransform(HumanBodyBones.Head).position.z;

                Mocopi3d[1].x = character.GetBoneTransform(HumanBodyBones.Neck).position.x;
                Mocopi3d[1].y = character.GetBoneTransform(HumanBodyBones.Neck).position.y;
                Mocopi3d[1].z = character.GetBoneTransform(HumanBodyBones.Neck).position.z;

                Mocopi3d[2].x = character.GetBoneTransform(HumanBodyBones.Chest).position.x;
                Mocopi3d[2].y = character.GetBoneTransform(HumanBodyBones.Chest).position.y;
                Mocopi3d[2].z = character.GetBoneTransform(HumanBodyBones.Chest).position.z;

                Mocopi3d[3].x = character.GetBoneTransform(HumanBodyBones.Spine).position.x;
                Mocopi3d[3].y = character.GetBoneTransform(HumanBodyBones.Spine).position.y;
                Mocopi3d[3].z = character.GetBoneTransform(HumanBodyBones.Spine).position.z;

                Mocopi3d[4].x = character.GetBoneTransform(HumanBodyBones.Hips).position.x;
                Mocopi3d[4].y = character.GetBoneTransform(HumanBodyBones.Hips).position.y;
                Mocopi3d[4].z = character.GetBoneTransform(HumanBodyBones.Hips).position.z;

                Mocopi3d[5].x = character.GetBoneTransform(HumanBodyBones.LeftShoulder).position.x;
                Mocopi3d[5].y = character.GetBoneTransform(HumanBodyBones.LeftShoulder).position.y;
                Mocopi3d[5].z = character.GetBoneTransform(HumanBodyBones.LeftShoulder).position.z;

                Mocopi3d[6].x = character.GetBoneTransform(HumanBodyBones.RightShoulder).position.x;
                Mocopi3d[6].y = character.GetBoneTransform(HumanBodyBones.RightShoulder).position.y;
                Mocopi3d[6].z = character.GetBoneTransform(HumanBodyBones.RightShoulder).position.z;

                Mocopi3d[7].x = character.GetBoneTransform(HumanBodyBones.LeftLowerArm).position.x;
                Mocopi3d[7].y = character.GetBoneTransform(HumanBodyBones.LeftLowerArm).position.y;
                Mocopi3d[7].z = character.GetBoneTransform(HumanBodyBones.LeftLowerArm).position.z;

                Mocopi3d[8].x = character.GetBoneTransform(HumanBodyBones.RightLowerArm).position.x;
                Mocopi3d[8].y = character.GetBoneTransform(HumanBodyBones.RightLowerArm).position.y;
                Mocopi3d[8].z = character.GetBoneTransform(HumanBodyBones.RightLowerArm).position.z;

                Mocopi3d[9].x = character.GetBoneTransform(HumanBodyBones.LeftHand).position.x;
                Mocopi3d[9].y = character.GetBoneTransform(HumanBodyBones.LeftHand).position.y;
                Mocopi3d[9].z = character.GetBoneTransform(HumanBodyBones.LeftHand).position.z;

                Mocopi3d[10].x = character.GetBoneTransform(HumanBodyBones.RightHand).position.x;
                Mocopi3d[10].y = character.GetBoneTransform(HumanBodyBones.RightHand).position.y;
                Mocopi3d[10].z = character.GetBoneTransform(HumanBodyBones.RightHand).position.z;

                Mocopi3d[11].x = character.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position.x;
                Mocopi3d[11].y = character.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position.y;
                Mocopi3d[11].z = character.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position.z;

                Mocopi3d[12].x = character.GetBoneTransform(HumanBodyBones.RightUpperLeg).position.x;
                Mocopi3d[12].y = character.GetBoneTransform(HumanBodyBones.RightUpperLeg).position.y;
                Mocopi3d[12].z = character.GetBoneTransform(HumanBodyBones.RightUpperLeg).position.z;

                Mocopi3d[13].x = character.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position.x;
                Mocopi3d[13].y = character.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position.y;
                Mocopi3d[13].z = character.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position.z;

                Mocopi3d[14].x = character.GetBoneTransform(HumanBodyBones.RightLowerLeg).position.x;
                Mocopi3d[14].y = character.GetBoneTransform(HumanBodyBones.RightLowerLeg).position.y;
                Mocopi3d[14].z = character.GetBoneTransform(HumanBodyBones.RightLowerLeg).position.z;

                Mocopi3d[15].x = character.GetBoneTransform(HumanBodyBones.LeftFoot).position.x;
                Mocopi3d[15].y = character.GetBoneTransform(HumanBodyBones.LeftFoot).position.y;
                Mocopi3d[15].z = character.GetBoneTransform(HumanBodyBones.LeftFoot).position.z;

                Mocopi3d[16].x = character.GetBoneTransform(HumanBodyBones.RightFoot).position.x;
                Mocopi3d[16].y = character.GetBoneTransform(HumanBodyBones.RightFoot).position.y;
                Mocopi3d[16].z = character.GetBoneTransform(HumanBodyBones.RightFoot).position.z;
            }
            // 5초 동안 운동한 내역을 저장
            if (frame <= 250)
            {
                SaveDataToCSV();
            }
            else if (frame >250)
            {
                if(GameManager.instance.nowExercise == GameManager.ExerciseList.SideShoulderDeltoid ||
                    GameManager.instance.nowExercise == GameManager.ExerciseList.BackShoulderDeltoid ||
                    GameManager.instance.nowExercise == GameManager.ExerciseList.BottomShoulder ||
                    GameManager.instance.nowExercise == GameManager.ExerciseList.ShoulderRotation ||
                    GameManager.instance.nowExercise == GameManager.ExerciseList.FrontShoulder)
                {
                    // 10초 동안 운동한 내역을 저장

                    if (frame <= 500)
                    {
                        SaveDataToCSV();
                    }
                }
            }

        }
        else
        {
            frame = 0;
        }
    }

    //운동 정보를 CSV로 저장하기 위한 함수
    private void SaveDataToCSV()
    {
        filePath = Application.dataPath + "/" + GameManager.instance.name1 + "_data.csv"; // CSV 파일 경로 설정

        // 데이터를 문자열로 변환
        string rowData = GameManager.instance.Today + "," + GameManager.instance.nowExercise + "," + frame.ToString() + ","
            + string.Join(",", GetFloatArray2d(Skeleton2d))
            + "," + string.Join(",", GetAngleArray2d()) + ","
            + string.Join(",", GetFloatArray3d(Mocopi3d))
            + "," + string.Join(",", GetAngleArray3d());

        if (!File.Exists(filePath))
        {
            StreamWriter sw1 = new StreamWriter(filePath, false);
            sw1.WriteLine("Date," + "Exercise," + "Frame," + GetCsvHeader());
            sw1.Close();

        }
        // CSV 파일에 데이터 쓰기
        StreamWriter sw = new StreamWriter(filePath, true);
        sw.WriteLine(rowData);
        sw.Close();

    }
    // CSV 헤더 만드는 함수
    private string GetCsvHeader()
    {
        string[] headerData = new string[Skeleton2d.Length * 2 + 12 + Mocopi3d.Length * 3 + 12]; //34+12 + 51 + 12
        List<string> jointList = new List<string>(new string[] {"Nose","LEye", "REye", "LEar","REar",
                                                                "Lshoulder","RShoulder","LElbow","RElbow",
                                                                "LWrist","RWrist","LHip","RHip",
                                                                "LKnee","RKnee","LAnkle","RAnkle"});
        for (int i = 0; i < Skeleton2d.Length; i++)
        {
            headerData[i * 2] = jointList[i] + "_X";
            headerData[i * 2 + 1] = jointList[i] + "_Y";
        }
        List<string> AngleList = new List<string>(new string[] {"Angle_Lshoulder1", "Angle_Lshoulder2", "Angle_Rshoulder1",
                                                                "Angle_Rshoulder2","Angle_LElbow","Angle_RElbow",
                                                                "Angle_LHip1","Angle_LHip2","Angle_RHip1",
                                                                "Angle_RHip2","Angle_LKnee","Angle_RKnee",});

        int k = 0;
        foreach (string exercise in AngleList)
        {

            headerData[34 + k] = exercise;
            k += 1;
        }

        List<string> MjointList = new List<string>(new string[] {"M_Head","M_Neck", "M_Chest", "M_Spine","M_Hips",
                                                                "M_Lshoulder","M_RShoulder","M_LElbow","M_RElbow",
                                                                "M_LWrist","M_RWrist","M_LHip","M_RHip",
                                                                "M_LKnee","M_RKnee","M_LAnkle","M_RAnkle"});
        for (int i = 0; i < Mocopi3d.Length; i++) // 46~ 
        {
            headerData[(i + 15) * 3 + 1] = MjointList[i] + "_X";
            headerData[(i + 15) * 3 + 2] = MjointList[i] + "_Y";
            headerData[(i + 15) * 3 + 3] = MjointList[i] + "_Z";
        }

        List<string> AngleList_M = new List<string>(new string[] {"M_Angle_Lshoulder1", "M_Angle_Lshoulder2", "M_Angle_Rshoulder1",
                                                                "M_Angle_Rshoulder2","M_Angle_LElbow","M_Angle_RElbow",
                                                                "M_Angle_LHip1","M_Angle_LHip2","M_Angle_RHip1",
                                                                "M_Angle_RHip2","M_Angle_LKnee","M_Angle_RKnee",});

        int j = 0;
        foreach (string ex in AngleList_M)
        {

            headerData[97 + j] = ex;
            j += 1;
        }

        return string.Join(",", headerData);
    }

    // (x,y)를 2차원 벡터로 저장하는 함수
    private float[] GetFloatArray2d(Vector2[] vectorArray)
    {
        float[] floatArray = new float[vectorArray.Length * 2];
        for (int i = 0; i < vectorArray.Length; i++)
        {
            floatArray[i * 2] = vectorArray[i].x;
            floatArray[i * 2 + 1] = vectorArray[i].y;
        }
        return floatArray;
    }
    // (x,y,z)를 3차원 벡터로 저장하는 함수

    private float[] GetFloatArray3d(Vector3[] vectorArray)
    {
        float[] floatArray = new float[vectorArray.Length * 3];
        for (int i = 0; i < vectorArray.Length; i++)
        {
            floatArray[i * 3] = vectorArray[i].x;
            floatArray[i * 3 + 1] = vectorArray[i].y;
            floatArray[i * 3 + 2] = vectorArray[i].z;

        }
        return floatArray;
    }
    // (x,y) 2차원 벡터로 관절 각도 계산하는 함수

    private float[] GetAngleArray2d()
    {
        float[] floatAngleArray = new float[12];
        //Left_Shoulder
        floatAngleArray[0] = Vector2.Angle(Skeleton2d[5] - Skeleton2d[7], Skeleton2d[5] - Skeleton2d[6]); //Lshoulder 1
        floatAngleArray[1] = Vector2.Angle(Skeleton2d[5] - Skeleton2d[7], Skeleton2d[5] - Skeleton2d[11]); // Lshoulder 2
        //Right_Shoulder
        floatAngleArray[2] = Vector2.Angle(Skeleton2d[6] - Skeleton2d[8], Skeleton2d[6] - Skeleton2d[7]); // RShoulder 1
        floatAngleArray[3] = Vector2.Angle(Skeleton2d[6] - Skeleton2d[8], Skeleton2d[6] - Skeleton2d[12]); //Rshoulder 2

        //Left_Elbow
        floatAngleArray[4] = Vector2.Angle(Skeleton2d[7] - Skeleton2d[5], Skeleton2d[7] - Skeleton2d[9]); // LElbow

        //Right_Elbow
        floatAngleArray[5] = Vector2.Angle(Skeleton2d[8] - Skeleton2d[6], Skeleton2d[8] - Skeleton2d[10]); // RElbow

        //Left_Leg
        floatAngleArray[6] = Vector2.Angle(Skeleton2d[11] - Skeleton2d[5], Skeleton2d[11] - Skeleton2d[13]); // LHip1
        floatAngleArray[7] = Vector2.Angle(Skeleton2d[11] - Skeleton2d[12], Skeleton2d[11] - Skeleton2d[13]); // LHip2
        //Right_Leg
        floatAngleArray[8] = Vector2.Angle(Skeleton2d[12] - Skeleton2d[6], Skeleton2d[12] - Skeleton2d[14]); // RHip1
        floatAngleArray[9] = Vector2.Angle(Skeleton2d[12] - Skeleton2d[11], Skeleton2d[12] - Skeleton2d[14]); // RHip2

        //Left_Knee
        floatAngleArray[10] = Vector2.Angle(Skeleton2d[13] - Skeleton2d[11], Skeleton2d[13] - Skeleton2d[15]); //LKnee

        //Right_Knee
        floatAngleArray[11] = Vector2.Angle(Skeleton2d[14] - Skeleton2d[12], Skeleton2d[14] - Skeleton2d[16]); //RKnee
        return floatAngleArray;
    }
    // (x,y,z) 3차원 벡터로 관절 각도 계산하는 함수

    private float[] GetAngleArray3d()
    {
        float[] floatAngleArray_3d = new float[12];
        //Left_Shoulder
        floatAngleArray_3d[0] = Vector3.Angle(Mocopi3d[5] - Mocopi3d[7], Mocopi3d[5] - Mocopi3d[6]); //Lshoulder 1
        floatAngleArray_3d[1] = Vector3.Angle(Mocopi3d[5] - Mocopi3d[7], Mocopi3d[5] - Mocopi3d[11]); // Lshoulder 2
        //Right_Shoulder
        floatAngleArray_3d[2] = Vector3.Angle(Mocopi3d[6] - Mocopi3d[8], Mocopi3d[6] - Mocopi3d[7]); // RShoulder 1
        floatAngleArray_3d[3] = Vector3.Angle(Mocopi3d[6] - Mocopi3d[8], Mocopi3d[6] - Mocopi3d[12]); //Rshoulder 2

        //Left_Elbow
        floatAngleArray_3d[4] = Vector3.Angle(Mocopi3d[7] - Mocopi3d[5], Mocopi3d[7] - Mocopi3d[9]); // LElbow

        //Right_Elbow
        floatAngleArray_3d[5] = Vector3.Angle(Mocopi3d[8] - Mocopi3d[6], Mocopi3d[8] - Mocopi3d[10]); // RElbow

        //Left_Leg
        floatAngleArray_3d[6] = Vector3.Angle(Mocopi3d[11] - Mocopi3d[5], Mocopi3d[11] - Mocopi3d[13]); // LHip1
        floatAngleArray_3d[7] = Vector3.Angle(Mocopi3d[11] - Mocopi3d[12], Mocopi3d[11] - Mocopi3d[13]); // LHip2
        //Right_Leg
        floatAngleArray_3d[8] = Vector3.Angle(Mocopi3d[12] - Mocopi3d[6], Mocopi3d[12] - Mocopi3d[14]); // RHip1
        floatAngleArray_3d[9] = Vector3.Angle(Mocopi3d[12] - Mocopi3d[11], Mocopi3d[12] - Mocopi3d[14]); // RHip2

        //Left_Knee
        floatAngleArray_3d[10] = Vector3.Angle(Mocopi3d[13] - Mocopi3d[11], Mocopi3d[13] - Mocopi3d[15]); //LKnee

        //Right_Knee
        floatAngleArray_3d[11] = Vector3.Angle(Mocopi3d[14] - Mocopi3d[12], Mocopi3d[14] - Mocopi3d[16]); //RKnee
        return floatAngleArray_3d;
    }
    // perfect, good, bad를 판단하고 score만드는 함수
    int PGBS(float time)
    {
        int score = 0;
        if (time < 5f) score = 2;
        else if (time < 8f) score = 1;
        return score;
    }
    //운동을 수행하며 시간에 맞춰 안내 UI를 나타냄
    IEnumerator Timer()
    {
        int min = 0;
        int sec = 0;
        announcement.text = "운동 자세를 준비해 주세요";

        const float interval = 1.0f;
        for (int k = 5; k > 0; k--)
        {
            sec += 1;
            if (sec >= 60)
            {
                sec -= 60;
                min += 1;
            }
            PastTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
            UIText.text = string.Format("{0}", k);
            if (GameManager.instance.ForNumber == true)
            {
                PastTime2.text = string.Format("{0:D2}:{1:D2}", min, sec);
                UIText2.text = string.Format("{0}", k);
            }
            

            yield return new WaitForSeconds(interval);

        }
        announcement.text = "시작합니다";

        PastTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
        if (GameManager.instance.ForNumber == true)
        {
            PastTime2.text = string.Format("{0:D2}:{1:D2}", min, sec);
        }
        
        /*
        if (GameManager.instance.nowExercise == GameManager.ExerciseList.Squat
            || GameManager.instance.nowExercise == GameManager.ExerciseList.ExternalRotation
            || GameManager.instance.nowExercise == GameManager.ExerciseList.StickMobility)
        {
            for (int i = 0; i < 8; i++)
            {
                ani.SetTrigger("Start");
                GameManager.instance.is_save = true;

                for (int j = 5; j > 0; j--)
                {
                    sec += 1;
                    if (sec >= 60)
                    {
                        sec -= 60;
                        min += 1;
                    }

                    UIText.text = string.Format("{0}", j);
                    if (GameManager.instance.ForNumber == true)
                    {
                        UIText2.text = string.Format("{0}", j);
                        PastTime2.text = string.Format("{0:D2}:{1:D2}", min, sec);

                    }
                    PastTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
                    yield return new WaitForSeconds(interval);
                }


                GameManager.instance.is_save = false;
                
                //count.text = (i+1).ToString();
            }
        }
        */

        // 특정 운동일 경우에 따라서 특정 애니메이션이 실행될 수 있도록 함
        if (GameManager.instance.nowExercise == GameManager.ExerciseList.SideShoulderDeltoid
            || GameManager.instance.nowExercise == GameManager.ExerciseList.BackShoulderDeltoid
            || GameManager.instance.nowExercise == GameManager.ExerciseList.BottomShoulder
            || GameManager.instance.nowExercise == GameManager.ExerciseList.ShoulderRotation
            || GameManager.instance.nowExercise == GameManager.ExerciseList.SpineRotation)
        {
            ani.SetTrigger("Left");
            GameManager.instance.is_save = true;

            for (int j = 10; j > 0; j--)
            {
                sec += 1;
                if (sec >= 60)
                {
                    sec -= 60;
                    min += 1;
                }

                UIText.text = string.Format("{0}", j);
                if (GameManager.instance.ForNumber == true)
                {
                    UIText2.text = string.Format("{0}", j);
                    PastTime2.text = string.Format("{0:D2}:{1:D2}", min, sec);

                }
                PastTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
                yield return new WaitForSeconds(interval);
            }
            ani.SetTrigger("Right_Idle");

            GameManager.instance.is_save = false;


            announcement.text = "운동 자세를 준비해 주세요";

            for (int k = 5; k > 0; k--)
            {
                sec += 1;
                if (sec >= 60)
                {
                    sec -= 60;
                    min += 1;
                }

                UIText.text = string.Format("{0}", k);
                PastTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
                if (GameManager.instance.ForNumber == true)
                {
                    UIText2.text = string.Format("{0}", k);
                    PastTime2.text = string.Format("{0:D2}:{1:D2}", min, sec);
                }
                
                yield return new WaitForSeconds(interval);

            }

            ani.SetTrigger("Right");

            GameManager.instance.is_save = true;

            for (int j = 10; j > 0; j--)
            {
                sec += 1;
                if (sec >= 60)
                {
                    sec -= 60;
                    min += 1;
                }

                UIText.text = string.Format("{0}", j);
                PastTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
                if (GameManager.instance.ForNumber == true)
                {
                    UIText2.text = string.Format("{0}", j);
                    PastTime2.text = string.Format("{0:D2}:{1:D2}", min, sec);
                }
                
                yield return new WaitForSeconds(interval);
            }
            GameManager.instance.is_save = false;
            

        }
        else if (GameManager.instance.nowExercise == GameManager.ExerciseList.FrontShoulder)
        {
            ani.SetTrigger("Start");
            GameManager.instance.is_save = true;

            for (int j = 10; j > 0; j--)
            {
                sec += 1;
                if (sec >= 60)
                {
                    sec -= 60;
                    min += 1;
                }

                UIText.text = string.Format("{0}", j);
                PastTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
                if (GameManager.instance.ForNumber == true)
                {
                    UIText2.text = string.Format("{0}", j);
                    PastTime2.text = string.Format("{0:D2}:{1:D2}", min, sec);
                }
                

                yield return new WaitForSeconds(interval);
            }
            GameManager.instance.is_save = false;
            
        }
        
        UIText.text = "0";
        if (GameManager.instance.ForNumber == true)
        {
            UIText2.text = "0";
        }

    }
    // 퍼펙트, 굿, 배드에 따라 이미지를 나타내는 함수
    IEnumerator PrintPGB(int pgb, GameObject[] PGB_Text, AudioSource audio)
    {
        for(int i = 0; i < 3; i++)
        {
            PGB_Text[i].SetActive(false);
        }
        audio.PlayOneShot(PGB_Audio[pgb], 1f);
        PGB_Text[pgb].SetActive(true);
        Image image = PGB_Text[pgb].GetComponent<Image>();
        Color color = image.color;

        for (int i = 20; i >= 0; i--)
        {
            float f = i / 20.0f;
            color.a = f;
            image.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 0; i < 3; i++)
        {
            PGB_Text[i].SetActive(false);
        }
    }

    // 스쿼트 운동 함수
    float squat_count = 0;
    float MinSquat = 180.0f;

    float[] s_PGBS = new float[4];
    bool s_check = false;

    private void Squat()
    {

        if (LAngleKnee < 120.0f && RAngleKnee < 120.0f)
        {
            if (LAngleKnee < MinSquat)
            {
                MinSquat = LAngleKnee;
            }
            s_check = true;
        }
        if (LAngleKnee > 140.0f && RAngleKnee > 140.0f && s_check == true && squat_count < 8)
        {
            if (MinSquat < 80)
            {
                s_PGBS[0]++;
                announcement.text = "완벽해요";
                StartCoroutine(PrintPGB(0, PGB_Text, Audio));
            }
            else if (MinSquat < 100)
            {
                s_PGBS[1]++;
                announcement.text = "좋습니다.";
                StartCoroutine(PrintPGB(1, PGB_Text, Audio));
            }
            else if (MinSquat < 120)
            {
                s_PGBS[2]++;
                announcement.text = "끝까지 앉아 주세요";
                StartCoroutine(PrintPGB(2, PGB_Text, Audio));
            }
            MinSquat = 180.0f;
            s_check = false;
        }
        squat_count = s_PGBS[0] + s_PGBS[1] + s_PGBS[2];
        count.text = squat_count.ToString();
        s_PGBS[3] = s_PGBS[0] * 12.5f * 1.0f + s_PGBS[1] * 12.5f * 0.75f + s_PGBS[2] * 12.5f * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            GameManager.instance.Squat_PGBS[i] = s_PGBS[i];
        }

    }

    // External Rotation 함수
    float external_count = 0;
    float MaxExternal = 0.0f;
    float[] e_PGBS = new float[4];
    bool e_check = false;

    private void ExternalRotation()
    {

        if (WristDistance > ElbowDistance * 1.2f)
        {
            if (WristDistance > MaxExternal)
            {
                MaxExternal = WristDistance;
            }
            e_check = true;
        }
        if (WristDistance < ElbowDistance * 1.2 && e_check == true && external_count < 8)
        {
            if (MaxExternal > ElbowDistance * 2.1)
            {
                e_PGBS[0]++;
                announcement.text = "완벽해요";
                StartCoroutine(PrintPGB(0, PGB_Text, Audio));
            }
            else if (MaxExternal > ElbowDistance * 1.7)
            {
                e_PGBS[1]++;
                announcement.text = "좋습니다.";
                StartCoroutine(PrintPGB(1, PGB_Text, Audio));
            }
            else if (MaxExternal > ElbowDistance * 1.3)
            {
                e_PGBS[2]++;
                announcement.text = "어깨를 더 열어 주세요";
                StartCoroutine(PrintPGB(2, PGB_Text, Audio));
            }
            MaxExternal = 0.0f;
            e_check = false;
        }
        external_count = e_PGBS[0] + e_PGBS[1] + e_PGBS[2];
        count.text = external_count.ToString();
        e_PGBS[3] = e_PGBS[0] * 12.5f * 1.0f + e_PGBS[1] * 12.5f * 0.75f + e_PGBS[2] * 12.5f * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            GameManager.instance.External_PGBS[i] = e_PGBS[i];
        }
    }

    // StickMobility 운동 함수
    int stick_state = 0; // 0~5까지 
    float state_score = 0;
    bool sideChecked = false;

    float stick_count = 0;
    float[] st_PGBS = new float[4];

    private void StickMobility()
    {
        // 팔의 곧은 정도를 평가하는 함수 (이론이랑 실제 작동에서 각도 차이가 괜찮은지?)
        float scoreStraightness(float LAngle, float RAngle)
        {
            float score;
            if (LAngle > 150.0f && RAngle > 150.0f)
                score = 1.0f;
            else if (LAngle > 110.0f && RAngle > 110.0f)
                score = 0.75f;
            else
                score = 0.5f;
            return score;
        }

        // 어깨 - 어깨 기준 벡터와 어깨 - 손목 벡터 이용 해서 각도 측정 - 각도의 변화에 따라 측정 
        float scoreAngleCorrectness(float Wrist2ShoulderAngle)
        {
            float score;
            if (Wrist2ShoulderAngle < 90f)
                score = 1.0f;
            else if (Wrist2ShoulderAngle > 110f)
                score = 0.75f;
            else
                score = 0.5f;
            return score;
        }

        // 팔이 중간에 있는지 확인하는 함수 -- 위 각도가 차이가 적을때 -- 
        bool isMiddle(float LAngle, float RAngle)
        {
            //Debug.Log("Left Shoulder: " + LAngle + "    Right Shoulder: " + RAngle + "    difference: " + Mathf.Abs(LAngle - RAngle) + "    is middle?: " + (Mathf.Abs(LAngle - RAngle) < 20f));

            return Mathf.Abs(LAngle - RAngle) < 30f;
        }

        // 팔이 옆으로 가 있는지 확인하는 함수
        bool isSide(float LAngle, float RAngle) {
            //Debug.Log("Left Shoulder: " + LAngle + "    Right Shoulder: " + RAngle + "    difference: " + Mathf.Abs(LAngle - RAngle) + "      is side?: " + (Mathf.Abs(LAngle - RAngle) > 70f));

            return Mathf.Abs(LAngle - RAngle) > 60f;
        }

        // 팔이 한쪽을 다녀온 곳을 확인 하는 용도의 함수... 
        bool isChecked() { return (Skeleton2d[9].y - Skeleton2d[10].y) > 0; }

        void scoreAnnouncement(float score)
        {
            //if (score > 0.95f)
            //    announcement.text = "완벽해요";
            //else if (score > 0.5f)
            //    announcement.text = "좋습니다";
            //else
            //    announcement.text = "팔을 더 뻗어보세요";
        }

       
        void Log()
        {
            //Debug.Log("Stick Count = " + stick_count + "    Stick State = " + stick_state);
        }

        int checkState(int state)
        {
            switch (state)
            {
                case 0:
                    if (isMiddle(LAngleWrist2Shoulder, RAngleWrist2Shoulder))
                    {
                        float score = scoreStraightness(LAngleElbow, RAngleElbow);
                        scoreAnnouncement(score);
                        state_score += score;
                        state++;

                        Log();
                    }
                    else
                    {
                        // announcement.text = "팔을 중간에 위치하세요";
                    }
                    break;
                case 1:
                    if (isSide(LAngleWrist2Shoulder, RAngleWrist2Shoulder))
                    {

                        float score = scoreStraightness(LAngleElbow, RAngleElbow) * scoreAngleCorrectness(LAngleWrist2Shoulder < RAngleWrist2Shoulder ? LAngleWrist2Shoulder : RAngleWrist2Shoulder);
                        scoreAnnouncement(score);
                        state_score += score;
                        sideChecked = isChecked();
                        state++;

                        Log();
                    }
                    else
                    {
                        if (isMiddle(LAngleWrist2Shoulder, RAngleWrist2Shoulder))
                        {
                            //  announcement.text = "팔을 한쪽으로 기울이세요";
                        }
                        // announcement.text = "팔을 더 기울이세요";
                    }
                    break;
                case 2:
                    if (isMiddle(LAngleWrist2Shoulder, RAngleWrist2Shoulder))
                    {

                        float score = scoreStraightness(LAngleElbow, RAngleElbow);
                        scoreAnnouncement(score);
                        state_score += score;
                        state++;

                        Log();
                    }
                    break;
                case 3:
                    if (isSide(LAngleWrist2Shoulder, RAngleWrist2Shoulder))
                    {
                        if (sideChecked != isChecked()) // 처음에 한곳과 반대일 때만 카운트
                        {
                            //Debug.Log("Another side");
                            float score = scoreStraightness(LAngleElbow, RAngleElbow) * scoreAngleCorrectness(LAngleWrist2Shoulder < RAngleWrist2Shoulder ? LAngleWrist2Shoulder : RAngleWrist2Shoulder);
                            scoreAnnouncement(score);
                            state_score += score;
                            state++;

                            Log();
                        }
                        else
                        {
                            // announcement.text = "팔을 다른쪽으로 기울이세요";
                        }
                    }
                    else
                    {
                        if (isMiddle(LAngleWrist2Shoulder, RAngleWrist2Shoulder))
                        {
                            // announcement.text = "팔을 한쪽으로 기울이세요";
                        }
                        // announcement.text = "팔을 더 기울이세요";
                    }
                    break;
                case 4:
                    if (isMiddle(LAngleWrist2Shoulder, RAngleWrist2Shoulder))
                    {
                        float score = scoreStraightness(LAngleElbow, RAngleElbow);
                        scoreAnnouncement(score);
                        state_score += score;
                        state = 5;

                        Log();
                    }
                    else
                    {
                        // announcement.text = "팔을 중간에 위치하세요";
                    }
                    break;
                case 5:
                    if (state_score > 4.75f)
                    {
                        st_PGBS[0]++;
                        announcement.text = "완벽해요";
                        StartCoroutine(PrintPGB(0, PGB_Text, Audio));
                    }


                    else if (state_score > 3.5)
                    {
                        st_PGBS[1]++;
                        announcement.text = "좋습니다";
                        StartCoroutine(PrintPGB(1, PGB_Text, Audio));
                    }


                    else
                    {
                        st_PGBS[2]++;
                        announcement.text = "보통이에요";
                        StartCoroutine(PrintPGB(2, PGB_Text, Audio));
                    }
                    state_score = 0;
                    state = 0;

                    Log();
                    break;
                default:
                    break;
            }
            return state;
        }

        if (stick_count < 8)
        {
            stick_state = checkState(stick_state);
        }

        stick_count = st_PGBS[0] + st_PGBS[1] + st_PGBS[2];
        count.text = stick_count.ToString();
        st_PGBS[3] = st_PGBS[0] * 12.5f * 1.0f + st_PGBS[1] * 12.5f * 0.75f + st_PGBS[2] * 12.5f * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            GameManager.instance.Stick_PGBS[i] = st_PGBS[i];
        }
    }

    // 측면 어깨 운동 함수
    float ssd_r_time1 = 0.0f;
    bool? ssd_r_check1 = null;
    float ssd_l_time1 = 0.0f;
    bool? ssd_l_check1 = null;
    float[] ssd_PGBS1 = new float[4]; //perfect, good, bad, score

    float ssd_r_time2 = 0.0f;
    bool? ssd_r_check2 = null;
    float ssd_l_time2 = 0.0f;
    bool? ssd_l_check2 = null;
    float[] ssd_PGBS2 = new float[4]; //perfect, good, bad, score

    float play_time = 0.0f;
    float play_time2 = 0.0f;

    private void SideShoulderDeltoid(Vector2[] Skeleton2d, ref float[] SideShoulderDeltoid_PGBS,
        ref float ssd_r_time, ref bool? ssd_r_check, ref float ssd_l_time, ref bool? ssd_l_check, 
        ref float[] ssd_PGBS, GameObject[] PGB_Text, Text count, ref float play_time, ref AudioSource Audio)
    {
        play_time += Time.deltaTime * 1;
        int PGBS(float time)
        {
            int score = 0;
            if (time < 5f) score = 2;
            else if (time < 8f) score = 1;
            return score;
        }

        // 두 어깨 사이의 중심 x좌표 
        float s_m_x = (Skeleton2d[6].x + Skeleton2d[5].x) / 2;

        // 시작 판정
        // 어깨보다 손목이 위로 가면 && 아직 시작하지 않았다면 측정 시작

        // 오른쪽 손목이 왼쪽 어깨 위에
        if(Skeleton2d[10].y >= Skeleton2d[7].y && Skeleton2d[0].x >= s_m_x && ssd_r_check != false && ssd_l_time ==0.0f)
        {
            ssd_r_check = true;
            ssd_r_time += Time.deltaTime * 1;
            Debug.Log(ssd_r_time);
        }

        // 왼쪽 손목이 오른쪽 어깨 위에 
        else if (Skeleton2d[9].y >= Skeleton2d[8].y && Skeleton2d[0].x <= s_m_x && ssd_l_check != false && ssd_r_time == 0.0f)
        {
            ssd_l_check = true;
            ssd_l_time += Time.deltaTime * 1;
            Debug.Log(ssd_l_time);
        }

        if (play_time >9.5f)
        {
            if (ssd_l_time == 0.0f)
            {
                ssd_PGBS[PGBS(ssd_r_time)]++;
                StartCoroutine(PrintPGB(PGBS(ssd_r_time),PGB_Text, Audio));
                ssd_r_time = 0;
            }
            else if(ssd_r_time == 0.0f)
            {
                ssd_PGBS[PGBS(ssd_l_time)]++;
                StartCoroutine(PrintPGB(PGBS(ssd_l_time), PGB_Text, Audio));
                ssd_l_time = 0;
            }
            play_time = 0.0f;
            GameManager.instance.is_save = false;

        }

        ssd_PGBS[3] = 50 * 1.0f * ssd_PGBS[0] + 50 * 0.75f * ssd_PGBS[1] + 50 * 0.5f * ssd_PGBS[2];
        count.text = (ssd_PGBS[0] + ssd_PGBS[1] + ssd_PGBS[2]).ToString();
        for (int i = 0; i < 4; i++)
        {
            SideShoulderDeltoid_PGBS[i] = ssd_PGBS[i];
        }
    }

    /*------------------------------------------------------------------------*/
    // 후면 어깨 운동 함수

    float bsd_r_time1 = 0.0f;
    bool? bsd_r_check1 = null;
    float bsd_l_time1 = 0.0f;
    bool? bsd_l_check1 = null;
    float[] bsd_PGBS1 = new float[4]; //perfect, good, bad, score

    float bsd_r_time2 = 0.0f;
    bool? bsd_r_check2 = null;
    float bsd_l_time2 = 0.0f;
    bool? bsd_l_check2 = null;
    float[] bsd_PGBS2 = new float[4]; //perfect, good, bad, score
    private void BackShoulderDeltoid(Vector2[] Skeleton2d,ref float[] BackShoulderDeltoid_PGBS,
         ref float bsd_r_time, ref bool? bsd_r_check, ref float bsd_l_time, ref bool? bsd_l_check, 
         ref float[] bsd_PGBS, GameObject[] PGB_Text, Text count, ref float play_time, ref AudioSource Audio)
    {
        // 팔이 교차하면 인식을 못함 
        // 양 어깨와 팔꿈치의 각도? 
        // 오른쪽이면 오른쪽 팔꿈치 - 오른쪽 어깨 - 왼쪽 어깨의 각도가 예각인지.... ? 60도, 30도 정도...?
        // 처음 판정을 그렇게 하고 계속 유지하는걸로 측정 해보자... -> 저 좋은 생각 있으면 수정 바람 
        float l_s_angle = Vector2.Angle(Skeleton2d[5] - Skeleton2d[6], Skeleton2d[5] - Skeleton2d[7]); // 오른쪽 어깨 - 왼쪽 어깨 - 왼쪽 팔꿈치 각도
        float r_s_angle = Vector2.Angle(Skeleton2d[6] - Skeleton2d[5], Skeleton2d[6] - Skeleton2d[8]); // 왼쪽 어깨 - 오른쪽 어깨 - 오른쪽 팔꿈치 각도
        play_time += Time.deltaTime * 1;

        if (bsd_r_check == false && bsd_l_check == false)
        {
            bsd_l_check = null;
            bsd_r_check = null;
        }
        if(count.text != "2")
        {
            // 어깨의 각도가 30도 이하 && bool 값이 null일시 측정 시작
            if (l_s_angle < 30f && bsd_l_check == null)
            {
                Debug.Log("left 측정시작");
                bsd_l_check = true;
            }
            else if (r_s_angle < 30f && bsd_r_check == null)
            {
                Debug.Log("right 측정시작");
                bsd_r_check = true;
            }
        }
        
        if(bsd_l_check == true && bsd_r_time == 0.0f && l_s_angle < 70f )
        {
            bsd_l_time += Time.deltaTime;
            Debug.Log("왼쪽 시간 " + bsd_l_time + " 초 경과, 어깨 각도" + l_s_angle);
        }
        if (bsd_r_check == true && bsd_l_time == 0.0f && r_s_angle < 70f)
        {
            bsd_r_time += Time.deltaTime;
            Debug.Log("오른쪽 시간 " + bsd_r_time + " 초 경과,  어깨 각도" + r_s_angle);
        }

        if (play_time > 9.5f)
        {
            if (bsd_l_time == 0.0f)
            {
                bsd_PGBS[PGBS(bsd_r_time)]++;
                StartCoroutine(PrintPGB(PGBS(bsd_r_time), PGB_Text, Audio));
                bsd_r_time = 0;
            }
            else if (bsd_r_time == 0.0f)
            {
                bsd_PGBS[PGBS(bsd_l_time)]++;
                StartCoroutine(PrintPGB(PGBS(bsd_l_time), PGB_Text, Audio));
                bsd_l_time = 0;
            }
            play_time = 0.0f;
            GameManager.instance.is_save = false;

        }
        bsd_PGBS[3] = 50 * bsd_PGBS[0] * 1.0f + 50 * bsd_PGBS[1] * 0.75f + 50 * bsd_PGBS[2] * 0.5f;
        count.text = (bsd_PGBS[0] + bsd_PGBS[1] + bsd_PGBS[2]).ToString();
        for (int i = 0; i < 4; i++)
        {
            BackShoulderDeltoid_PGBS[i] = bsd_PGBS[i];
        }
    }

    /*------------------------------------------------------------------------*/
    // 하부 어깨 운동 함수

    float bs_time_l1 = 0.0f;
    bool? bs_check_l1 = null;
    float bs_time_r1 = 0.0f;
    bool? bs_check_r1 = null;
    float[] bs_PGBS1 = new float[4]; //perfect, good, bad, score

    float bs_time_l2 = 0.0f;
    bool? bs_check_l2 = null;
    float bs_time_r2 = 0.0f;
    bool? bs_check_r2 = null;
    float[] bs_PGBS2 = new float[4]; //perfect, good, bad, score

    private void BottomShoulder(Vector2[] Skeleton2d, ref float[] BottomShoulder_PGBS,
        ref float bs_time_l, ref bool? bs_check_l, ref float bs_time_r, ref bool? bs_check_r, 
        ref float[] bs_PGBS, GameObject[] PGB_Text, Text count, ref float play_time, ref AudioSource Audio)
    {
        //코드 짜기 (수영) 핑핑 안녕히계세요 저는 여기까지입니다 핑핑
        // 팔꿈치 - 골반 각도 150도 이상, 팔꿈치 각도 90도 이하 -> 준비 자세
        // 팔꿈치의 y 좌표가 어느 정도 떨어지면 -> 운동 중?
        float elbowhipAngle_l = Vector2.Angle(Skeleton2d[5] - Skeleton2d[7], Skeleton2d[5] - Skeleton2d[11]); // 왼쪽 겨드랑이 각도
        float elbowhipAngle_r = Vector2.Angle(Skeleton2d[6] - Skeleton2d[8], Skeleton2d[6] - Skeleton2d[12]); // 오른쪽 겨드랑이 각도

        float e_h_r_angle = Vector2.Angle(Skeleton2d[11] - Skeleton2d[7], Skeleton2d[11] - Skeleton2d[12]); // 왼팔 - 왼골 - 오골
        float e_h_l_angle = Vector2.Angle(Skeleton2d[12] - Skeleton2d[8], Skeleton2d[12] - Skeleton2d[11]); // 오팔 - 오골 - 왼골 

        //Debug.Log("왼팔골골 " + e_h_l_angle + ", 오팔골골 " + e_h_r_angle + ", 왼겨 " + elbowhipAngle_l + ", 오겨" + elbowhipAngle_r + ", 왼꿈 " + LAngleElbow + ", 오꿈 " + RAngleElbow);

        play_time += Time.deltaTime * 1;

        // 왼쪽 측정
        if (e_h_l_angle <= 105.0f && Skeleton2d[6].y < Skeleton2d[8].y)
        {
            bs_check_l = true;
            bs_time_l += Time.deltaTime;
            Debug.Log("왼쪽 시간: " + bs_time_l + ", angle" + e_h_l_angle);
        }


        // 오른쪽 측정
        if (e_h_r_angle <= 105.0f && Skeleton2d[5].y < Skeleton2d[7].y)
        {
            bs_check_r = true;
            bs_time_r += Time.deltaTime;
            Debug.Log("오른쪽 시간: " + bs_time_r+ ", angle" + e_h_r_angle);
        }

        if (play_time > 9.5f)
        {
            if (bs_time_r > bs_time_l )
            {
                bs_PGBS[PGBS(bs_time_r + 1f)]++;
                StartCoroutine(PrintPGB(PGBS(bs_time_r + 1f), PGB_Text, Audio));
                bs_time_r = 0;
            }
            else
            {
                bs_PGBS[PGBS(bs_time_l + 1f)]++;
                StartCoroutine(PrintPGB(PGBS(bs_time_l + 1f), PGB_Text, Audio));
                bs_time_l = 0;
            }
            play_time = 0.0f;
            GameManager.instance.is_save = false;

        }
        bs_PGBS[3] = 50 * bs_PGBS[0] * 1.0f + 50 * bs_PGBS[1] * 0.75f + 50 * bs_PGBS[2] * 0.5f;
        count.text = (bs_PGBS[0] + bs_PGBS[1] + bs_PGBS[2]).ToString();
        for (int i = 0; i < 4; i++)
        {
            BottomShoulder_PGBS[i] = bs_PGBS[i];
        }
    }

    /*------------------------------------------------------------------------*/

    // 어꺠 회전 운동 함수

    float[] sr_PGBS1 = new float[4]; //perfect, good, bad, score
    float sr_r_time1 = 0.0f;
    bool? sr_r_check1 = null;
    float sr_l_time1 = 0.0f;
    bool? sr_l_check1 = null;

    float[] sr_PGBS2 = new float[4]; //perfect, good, bad, score
    float sr_r_time2 = 0.0f;
    bool? sr_r_check2 = null;
    float sr_l_time2 = 0.0f;
    bool? sr_l_check2 = null;
    private void ShoulderRotation(Vector2[] Skeleton2d, ref float[] ShoulderRotation_PGBS,
            ref float[] sr_PGBS, ref float sr_r_time, ref bool? sr_r_check,
            ref float sr_l_time, ref bool? sr_l_check, 
            GameObject[] PGB_Text, Text count, ref float play_time, ref AudioSource Audio)
                                    // 횟수 제한주기
    {

        play_time += Time.deltaTime * 1;
 

        if (Skeleton2d[9].y > Skeleton2d[11].y && Vector2.Distance(Skeleton2d[10],Skeleton2d[7])< Vector2.Distance(Skeleton2d[5],Skeleton2d[6]) && sr_l_time == 0.0f) 
        {
            sr_r_time += Time.deltaTime * 1;
            Debug.Log("sr_r " + sr_r_time);
        }


        if (Skeleton2d[10].y > Skeleton2d[12].y && Vector2.Distance(Skeleton2d[9], Skeleton2d[8]) < Vector2.Distance(Skeleton2d[6], Skeleton2d[5]) && sr_r_time == 0.0f)
        {
            sr_l_time += Time.deltaTime * 1;
            Debug.Log("sr_l " + sr_l_time);
        }

        if (play_time > 9.5f)
        {
            if (sr_l_time == 0.0f)
            {
                sr_PGBS[PGBS(sr_r_time)]++;
                StartCoroutine(PrintPGB(PGBS(sr_r_time), PGB_Text, Audio));
                sr_r_time = 0;
            }
            else if (sr_r_time == 0.0f)
            {
                sr_PGBS[PGBS(sr_l_time)]++;
                sr_l_time = 0;
                StartCoroutine(PrintPGB(PGBS(sr_l_time), PGB_Text, Audio));
            }
            play_time = 0.0f;
            GameManager.instance.is_save = false;

        }
        sr_PGBS[3] = 50 * sr_PGBS[0] * 1.0f + 50 * sr_PGBS[1] * 0.75f + 50 * sr_PGBS[2] * 0.5f;
        count.text = (sr_PGBS[0] + sr_PGBS[1] + sr_PGBS[2]).ToString();

        for (int i = 0; i < 4; i++)
        {
            ShoulderRotation_PGBS[i] = sr_PGBS[i];
        }


        //한쪽 팔꿈치의 위치가 몸통의 3/1 이상에 위치하며, 다른 쪽 손목과 팔꿈치의 거리가 일정 이하일때, 팔꿈치가 몸통에 가까우면?
        //시간초 증가
    }


    /*------------------------------------------------------------------------*/
    
    // 전면 어깨 운동 함수
    float fs_time1 = 0.0f;
    float[] fs_PGBS1 = new float[4];
    bool? fs_check1 = null;

    float fs_time2 = 0.0f;
    float[] fs_PGBS2 = new float[4];
    bool? fs_check2 = null;

    private void FrontShoulder(Vector2[] Skeleton2d, ref float[] FrontShoulder_PGBS,
            ref float fs_time, ref float[] fs_PGBS, ref bool? fs_check, 
            GameObject[] PGB_Text, Text count, ref float play_time, ref AudioSource Audio)
    {
        play_time += Time.deltaTime * 1;
        //Debug.Log("Play time" + play_time);
        //if (fs_check != false && Skeleton2d[9].x < (Skeleton2d[5].x + Skeleton2d[11].x) / 2 && Skeleton2d[10].x < (Skeleton2d[6].x + Skeleton2d[12].x) / 2 )
        if (Skeleton2d[9].x < (Skeleton2d[5].x + Skeleton2d[11].x) / 2 && Skeleton2d[10].x < (Skeleton2d[6].x + Skeleton2d[12].x) / 2)
            {
            fs_check = true;
            fs_time += Time.deltaTime * 1;
            Debug.Log("fs time" + fs_time);

            
        }
        
        if (play_time > 9.5f)
        {
            fs_PGBS[PGBS(fs_time)]++;
            StartCoroutine(PrintPGB(PGBS(fs_time), PGB_Text, Audio));
            play_time = 0.0f;
            fs_time = 0;
            GameManager.instance.is_save = false;
        }
        fs_PGBS[3] = 100 * fs_PGBS[0] * 1.0f + 100 * fs_PGBS[1] * 0.75f + 100 * fs_PGBS[2] * 0.5f;
        count.text = (fs_PGBS[0] + fs_PGBS[1] + fs_PGBS[2]).ToString();
        for (int i = 0; i < 4; i++)
        {
            FrontShoulder_PGBS[i] = fs_PGBS[i];
        }
    }


    /*------------------------------------------------------------------------*/
    // 척추 회전 운동 함수

    float SpineR_r_time1 = 0.0f;
    bool? SpineR_r_check1 = null;
    float SpineR_l_time1 = 0.0f;
    bool? SpineR_l_check1 = null;
    float[] SpineR_PGBS1 = new float[4]; //perfect, good, bad, score

    float SpineR_r_time2 = 0.0f;
    bool? SpineR_r_check2 = null;
    float SpineR_l_time2 = 0.0f;
    bool? SpineR_l_check2 = null;
    float[] SpineR_PGBS2 = new float[4]; //perfect, good, bad, score
    private void SpineRotation(Vector2[] Skeleton2d, ref float[] SpineRotation_PGBS,
         ref float SpineR_r_time, ref bool? SpineR_r_check, ref float SpineR_l_time, ref bool? SpineR_l_check,
         ref float[] SpineR_PGBS, GameObject[] PGB_Text, Text count, ref float play_time, ref AudioSource Audio)
    {

        float l_s_angle = Vector2.Angle(Skeleton2d[5] - Skeleton2d[6], Skeleton2d[5] - Skeleton2d[7]); // 오른쪽 어깨 - 왼쪽 어깨 - 왼쪽 팔꿈치 각도
        float r_s_angle = Vector2.Angle(Skeleton2d[6] - Skeleton2d[5], Skeleton2d[6] - Skeleton2d[8]); // 왼쪽 어깨 - 오른쪽 어깨 - 오른쪽 팔꿈치 각도
        play_time += Time.deltaTime * 1;

        if (SpineR_r_check == false && SpineR_l_check == false)
        {
            SpineR_l_check = null;
            SpineR_r_check = null;
        }
        if (count.text != "2")
        {
            // 어깨의 각도가 30도 이하 && bool 값이 null일시 측정 시작
            if (l_s_angle < 30f && SpineR_l_check == null)
            {
                Debug.Log("left 측정시작");
                SpineR_l_check = true;
            }
            else if (r_s_angle < 30f && SpineR_r_check == null)
            {
                Debug.Log("right 측정시작");
                SpineR_r_check = true;
            }
        }

        if (SpineR_l_check == true && SpineR_r_time == 0.0f && l_s_angle < 70f)
        {
            SpineR_l_time += Time.deltaTime;
            Debug.Log("왼쪽 시간 " + SpineR_l_time + " 초 경과, 어깨 각도" + l_s_angle);
        }
        if (SpineR_r_check == true && SpineR_l_time == 0.0f && r_s_angle < 70f)
        {
            SpineR_r_time += Time.deltaTime;
            Debug.Log("오른쪽 시간 " + SpineR_r_time + " 초 경과,  어깨 각도" + r_s_angle);
        }

        if (play_time > 9.5f)
        {
            if (SpineR_l_time == 0.0f)
            {
                SpineR_PGBS[PGBS(SpineR_r_time)]++;
                StartCoroutine(PrintPGB(PGBS(SpineR_r_time), PGB_Text, Audio));
                SpineR_r_time = 0;
            }
            else if (SpineR_r_time == 0.0f)
            {
                SpineR_PGBS[PGBS(SpineR_l_time)]++;
                StartCoroutine(PrintPGB(PGBS(SpineR_l_time), PGB_Text, Audio));
                SpineR_l_time = 0;
            }
            play_time = 0.0f;
            GameManager.instance.is_save = false;

        }
        SpineR_PGBS[3] = 50 * SpineR_PGBS[0] * 1.0f + 50 * SpineR_PGBS[1] * 0.75f + 50 * SpineR_PGBS[2] * 0.5f;
        count.text = (SpineR_PGBS[0] + SpineR_PGBS[1] + SpineR_PGBS[2]).ToString();
        for (int i = 0; i < 4; i++)
        {
            SpineRotation_PGBS[i] = SpineR_PGBS[i];
        }
    }
}
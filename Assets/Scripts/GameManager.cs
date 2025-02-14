using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // UI 관련 라이브러리
using UnityEngine.SceneManagement; // 씬 관리 관련 라이브러리
using System;
using System.IO;


[Serializable]

public class ContentData
{
    public float Squat_Deviation;
    public float Yoga_Deviation;
}


public class GameManager : MonoBehaviour
{
    // 각 운동 저장할 수 있는 실수 배열 선언
    public static GameManager instance;

    public string Today;
    public string name1;            // 플레이어1 이름
    public string name2;            // 플레이어2 이름

    public bool ForNumber = false; // flag for 2p player mode
    public bool is_save = false;

    public ExerciseList nowExercise;

    public float[] Squat_PGBS = new float[4]; // perfect, good, bad, score
    
    public float[] External_PGBS = new float[4]; 
    
    public float[] Stick_PGBS = new float[4];

    public float[] SideShoulderDeltoid_PGBS = new float[4]; 
    public float[] SideShoulderDeltoid2_PGBS = new float[4]; 

    public float[] BackShoulderDeltoid_PGBS = new float[4];
    public float[] BackShoulderDeltoid2_PGBS = new float[4];

    public float[] BottomShoulder_PGBS = new float[4];
    public float[] BottomShoulder2_PGBS = new float[4];

    public float[] ShoulderRotation_PGBS = new float[4];
    public float[] ShoulderRotation2_PGBS = new float[4];

    public float[] FrontShoulder_PGBS = new float[4];
    public float[] FrontShoulder2_PGBS = new float[4];

    public float[] SpineRotation_PGBS = new float[4];
    public float[] SpineRotation2_PGBS = new float[4];


    void Awake()
    {
        //오늘 날짜 저장

        Today = DateTime.Now.ToString("yyyy-MM-dd");

        // 게임 오브젝트 DontDestroy 만들기
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Scene 전환시 버튼 내용 변환용 enum

    public enum ExerciseList
    {
        Ready, Squat, ExternalRotation, StickMobility, 
        SideShoulderDeltoid, BackShoulderDeltoid, BottomShoulder,ShoulderRotation, FrontShoulder, SpineRotation
    }


}

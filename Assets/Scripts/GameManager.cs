using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // UI ���� ���̺귯��
using UnityEngine.SceneManagement; // �� ���� ���� ���̺귯��
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
    // �� � ������ �� �ִ� �Ǽ� �迭 ����
    public static GameManager instance;

    public string Today;
    public string name1;            // �÷��̾�1 �̸�
    public string name2;            // �÷��̾�2 �̸�

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
        //���� ��¥ ����

        Today = DateTime.Now.ToString("yyyy-MM-dd");

        // ���� ������Ʈ DontDestroy �����
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

    // Scene ��ȯ�� ��ư ���� ��ȯ�� enum

    public enum ExerciseList
    {
        Ready, Squat, ExternalRotation, StickMobility, 
        SideShoulderDeltoid, BackShoulderDeltoid, BottomShoulder,ShoulderRotation, FrontShoulder, SpineRotation
    }


}

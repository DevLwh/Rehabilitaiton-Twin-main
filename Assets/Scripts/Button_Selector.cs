using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Selector : MonoBehaviour
{
    // 운동 선택 스크립트
    // GameManager 인스턴스에 접근하여 운동 상태 확인
    public void ExerciseSelcetor(string Es)
    {
        switch (Es)
        {
            case "Ready":
                GameManager.instance.nowExercise = GameManager.ExerciseList.Ready;
                break;
            case "Squat":
                GameManager.instance.nowExercise = GameManager.ExerciseList.Squat;
                break;
            case "ExternalRotation":
                GameManager.instance.nowExercise = GameManager.ExerciseList.ExternalRotation;
                break;
            case "StickMobility":
                GameManager.instance.nowExercise = GameManager.ExerciseList.StickMobility;
                break;
            case "SideShoulderDeltoid":
                PlayerPrefs.SetFloat("ButtonClickDelay", 33f);
                GameManager.instance.nowExercise = GameManager.ExerciseList.SideShoulderDeltoid;
                break;
            case "BackShoulderDeltoid":
                PlayerPrefs.SetFloat("ButtonClickDelay", 33f);
                GameManager.instance.nowExercise = GameManager.ExerciseList.BackShoulderDeltoid;
                break;
            case "BottomShoulder":
                PlayerPrefs.SetFloat("ButtonClickDelay", 33f);
                GameManager.instance.nowExercise = GameManager.ExerciseList.BottomShoulder;
                break;
            case "ShoulderRotation":
                PlayerPrefs.SetFloat("ButtonClickDelay", 33f);
                GameManager.instance.nowExercise = GameManager.ExerciseList.ShoulderRotation;
                break;
            case "FrontShoulder":
                PlayerPrefs.SetFloat("ButtonClickDelay", 18f);
                GameManager.instance.nowExercise = GameManager.ExerciseList.FrontShoulder;
                break;
            case "SpineRotation":
                PlayerPrefs.SetFloat("ButtonClickDelay", 33f);
                GameManager.instance.nowExercise = GameManager.ExerciseList.SpineRotation;
                break;
        }

    }
}